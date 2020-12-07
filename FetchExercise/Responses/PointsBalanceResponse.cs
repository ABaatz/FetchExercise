using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FetchExercise.Responses
{
    public class PointsBalanceResponse
    {
        public Dictionary<string, int> PayerBalances { get; set; }
    }
}
