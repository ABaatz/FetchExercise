using FetchExercise.Models;
using FetchExercise.Requests;
using NUnit.Framework;
using System;
using System.Linq;

namespace UnitTests
{
    public class UserTests
    {
        /// <summary>
        /// Ensure that adding points works properly
        /// </summary>
        [Test]
        public void User_AddPoints_Ok()
        {
            var user = new User("TestUser");
            string payer = "TestPayer1";
            var points = 100;
            user.AddPoints(new AddPointsRequest { Payer = payer, Points = points, TransactionDate = DateTime.Parse("12/01/2020") });
            
            Assert.True(user.TransactionHistory.Any(r => r.TransactionRecord.Payer == payer));
            var record = user.TransactionHistory.First(r => r.TransactionRecord.Payer == payer);
            Assert.AreEqual(points, record.TransactionRecord.Points);
        }

        /// <summary>
        /// Ensures that the properly record is marked as used (RemainingPoints == 0)
        /// </summary>
        [Test]
        public void User_DeductPoints_Ok()
        {
            var user = new User("TestUser");
            string payer1 = "TestPayer1";
            string payer2 = "TestPayer2";
            var points = 100;
            var endPoints = 0;
            user.AddPoints(
                new AddPointsRequest 
                { 
                    Payer = payer1, 
                    Points = points, 
                    TransactionDate = DateTime.Parse("12/01/2020") 
                });

            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = payer2,
                    Points = points,
                    TransactionDate = DateTime.Parse("12/02/2020")
                });

            user.DeductPoints(new DeductPointsRequest { Points = points });
            var records = user.TransactionHistory.OrderBy(r => r.TransactionRecord.TransactionDate);
            var record = records.First(r => r.TransactionRecord.Payer == payer1);

            Assert.AreEqual(endPoints, record.RemainingPoints);
            Assert.AreEqual(points * -1, records.Last().TransactionRecord.Points);
        }

        /// <summary>
        /// Ensures that the balances are being added up properly.
        /// </summary>
        [Test]
        public void User_GetPoints_Ok()
        {
            var user = new User("TestUser");
            string payer = "TestPayer1";
            var points = 100;
            user.AddPoints(
                new AddPointsRequest 
                { Payer = payer, 
                    Points = points, 
                    TransactionDate = DateTime.Parse("12/01/2020") 
                });
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = payer,
                    Points = points,
                    TransactionDate = DateTime.Parse("12/02/2020")
                });

            var result = user.GetPoints()?.PayerBalances;
            Assert.IsNotNull(result?[payer]);
            Assert.AreEqual(points * 2, result[payer]);
        }

        /// <summary>
        /// Quickly test example scenario given in document
        /// </summary>
        [Test]
        public void User_TestGivenScenario_Ok()
        {
            var dannonPoints = 1000;
            var unileverPoints = 0;
            var millerPoints = 5300;
            var user = new User("TestUser");
            #region Add Points
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = "DANNON",
                    Points = 300,
                    TransactionDate = DateTime.Parse("10/31/2020 10:00:00")
                });
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = "UNILEVER",
                    Points = 200,
                    TransactionDate = DateTime.Parse("10/31/2020 11:00:00")
                });
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = "DANNON",
                    Points = -200,
                    TransactionDate = DateTime.Parse("10/31/2020 15:00:00")
                });
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = "MILLER COORS",
                    Points = 10000,
                    TransactionDate = DateTime.Parse("11/1/2020 14:00:00")
                });
            user.AddPoints(
                new AddPointsRequest
                {
                    Payer = "DANNON",
                    Points = 1000,
                    TransactionDate = DateTime.Parse("11/2/2020 14:00:00")
                });
            #endregion

            user.DeductPoints(new DeductPointsRequest { Points = 5000 });
            var result = user.GetPoints().PayerBalances;

            Assert.IsNotNull(result?["DANNON"]);
            Assert.IsNotNull(result?["UNILEVER"]);
            Assert.IsNotNull(result?["MILLER COORS"]);

            Assert.AreEqual(dannonPoints, result["DANNON"]);
            Assert.AreEqual(unileverPoints, result["UNILEVER"]);
            Assert.AreEqual(millerPoints, result["MILLER COORS"]);
        }
    }
}