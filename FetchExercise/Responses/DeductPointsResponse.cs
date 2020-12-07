using FetchExercise.Models;
using System.Collections.Generic;

namespace FetchExercise.Responses
{
    public class DeductPointsResponse
    {
        public List<TransactionRecord> TransactionRecords { get; set; }
    }
}
