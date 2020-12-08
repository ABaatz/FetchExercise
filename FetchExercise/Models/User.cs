
using FetchExercise.Requests;
using FetchExercise.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FetchExercise.Models
{
    public class User
    {
        public string Name { get; internal set; }
        public List<InternalTransactionRecord> TransactionHistory { get; internal set; }

        public User(string name)
        {
            Name = name;
            TransactionHistory = new List<InternalTransactionRecord>();
        }

        /// <summary>
        /// Adds transaction records that reflec the point change
        /// If the points added is negative, the points will be deducted properly
        /// </summary>
        /// <param name="request">Json request including the payer, the amount of points, and the timestamp</param>
        public void AddPoints(AddPointsRequest request)
        {
            if (request.Points < 0)
            {
                DeductPoints(new DeductPointsRequest { Points = request.Points * -1 }, request.TransactionDate);
            }
            else
            {
                var record = new TransactionRecord(request.Payer, request.Points, request.TransactionDate);
                TransactionHistory.Add(new InternalTransactionRecord(record));
            }
        }

        /// <summary>
        /// Deducts points from the oldest remaining balance
        /// </summary>
        /// <param name="request">Json request containing the point value to be deducted</param>
        /// <param name="transactionDate">Optional datetime to specify a date to be logged</param>
        public DeductPointsResponse DeductPoints(DeductPointsRequest request, DateTime? transactionDate = null)
        {
            if (request.Points < 0)
            {
                throw new ArgumentException("Cannot remove negative points.");
            }
            var deductionRecords = new List<TransactionRecord>();
            var tempPoints = request.Points;
            while (tempPoints > 0)
            {
                var record = TransactionHistory.OrderBy(r => r.TransactionRecord.TransactionDate).FirstOrDefault(r => r.RemainingPoints > 0);
                if (record == null)
                {
                    throw new InvalidOperationException();
                }
                var pointChange = record.RemainingPoints > tempPoints ? tempPoints : record.RemainingPoints;
                var dateTime = transactionDate ?? DateTime.Now;
                var newTransactionRecord = new TransactionRecord(record.TransactionRecord.Payer, (pointChange * -1), dateTime);
                deductionRecords.Add(newTransactionRecord);
                TransactionHistory.Add(new InternalTransactionRecord(newTransactionRecord, 0));
                tempPoints -= pointChange;
                record.RemainingPoints -= pointChange;
            }
            return new DeductPointsResponse() { TransactionRecords = deductionRecords };
        }

        /// <summary>
        /// Returns a dictionary of payers and total balance excluding deducted points.
        /// </summary>
        public PointsBalanceResponse GetPoints()
        {
            var balanceDict = new Dictionary<string, int>();
            foreach (var record in TransactionHistory)
            {
                if (balanceDict.ContainsKey(record.TransactionRecord.Payer))
                {
                    balanceDict[record.TransactionRecord.Payer] += record.RemainingPoints;
                }
                else
                {
                    balanceDict.Add(record.TransactionRecord.Payer, record.RemainingPoints);
                }
            }
            return new PointsBalanceResponse { PayerBalances = balanceDict };
        }
    }
}
