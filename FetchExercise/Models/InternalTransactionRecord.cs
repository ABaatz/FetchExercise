namespace FetchExercise.Models
{
    public class InternalTransactionRecord
    {
        public TransactionRecord TransactionRecord { get; set; }
        /// <summary>
        /// Allows a record to be marked as "empty" so the transaction history does not get messed up
        /// </summary>
        public int RemainingPoints { get; set; }

        public InternalTransactionRecord(TransactionRecord transactionRecord)
            : this(transactionRecord, transactionRecord.Points)
        { }

        public InternalTransactionRecord(TransactionRecord record, int remainingPoints)
        {
            TransactionRecord = record;
            if (remainingPoints < 0)
            {
                remainingPoints = 0;
            }
            RemainingPoints = remainingPoints;
        }
    }
}
