using FetchExercise.Controllers;
using FetchExercise.Requests;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;

namespace UnitTests
{
    public class PointsControllerTests
    {
        /// <summary>
        /// Test that the controller works as expected
        /// </summary>
        [Test]
        public void Controller_AddPoints_Ok()
        {
            PointsController controller = new PointsController();
            var result = controller.AddPoints(
                "Tester",
                new AddPointsRequest
                {
                    Payer = "Payer",
                    Points = 1,
                    TransactionDate = DateTime.Now
                });
            Assert.AreEqual(typeof(OkResult), result.GetType());
        }

        /// <summary>
        /// Test that it fails to continue into the User method due to parsing
        /// </summary>
        [Test]
        public void Controller_AddPoints_Invalid_Format()
        {
            PointsController controller = new PointsController();
            var result = controller.AddPoints(
                "Tester",
                new AddPointsRequest
                {
                    Payer = null,
                    Points = 0,
                    TransactionDate = new DateTime()
                });
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }

        /// <summary>
        /// Test that the controller works as expected
        /// </summary>
        [Test]
        public void Controller_DeductPoints_Ok()
        {
            PointsController controller = new PointsController();
            controller.AddPoints(
                "Tester",
                new AddPointsRequest
                {
                    Payer = "Payer",
                    Points = 1,
                    TransactionDate = DateTime.Now
                });
            var result = controller.DeductPoints(
                "Tester",
                new DeductPointsRequest
                {
                    Points = 1
                });
            Assert.AreEqual(typeof(OkObjectResult), result.Result.GetType());
        }

        /// <summary>
        /// Test that it fails to continue into the User method due to invalid points
        /// </summary>
        [Test]
        public void Controller_DeductPoints_Invalid_Format()
        {
            PointsController controller = new PointsController();
            var result = controller.DeductPoints(
                "Tester",
                new DeductPointsRequest
                {
                    Points = -1
                });
            Assert.AreEqual(typeof(BadRequestObjectResult), result.Result.GetType());
        }

        /// <summary>
        /// Test that the controller works as expected
        /// </summary>
        [Test]
        public void Controller_GetPoints_Ok()
        {
            PointsController controller = new PointsController();
            var result = controller.GetBalance("Tester");
            Assert.AreEqual(typeof(OkObjectResult), result.Result.GetType());
        }
    }
}
