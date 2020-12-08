using System;

namespace FetchExercise.Requests
{
    public class AddPointsRequest
    {
        public string Payer { get; set; }
        public int Points { get; set; }
        public DateTime TransactionDate { get; set; }

        internal bool IsValid
        {
            get
            {
                return !(string.IsNullOrWhiteSpace(Payer) || TransactionDate == new DateTime());
            }
        }
    }
}
