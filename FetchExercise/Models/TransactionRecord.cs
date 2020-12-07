using System;

namespace FetchExercise.Models
{
    public class TransactionRecord
    {
        public string Payer { get; private set; }
        public int Points { get; private set; }
        public DateTime TransactionDate { get; private set; }

        public TransactionRecord(string payer, int points, DateTime transactionDate)
        {
            Payer = payer;
            Points = points;
            TransactionDate = transactionDate;
        }

        public override string ToString()
        {
            return $"{Payer}: {Points} ({TransactionDate})";
        }
    }
}
