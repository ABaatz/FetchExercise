using FetchExercise.Models;
using FetchExercise.Requests;
using FetchExercise.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FetchExercise.Controllers
{
    [ApiController]
    [Route("points")]
    public class PointsController : ControllerBase
    {
        /// <summary>
        /// Adds points to a user's balance
        /// </summary>
        /// <param name="user">Target user to add points to</param>
        /// <param name="request">Json request including the payer, the amount of points, and the timestamp</param>
        [HttpPost]
        [Route("add/{user}")]
        public IActionResult AddPoints(string user, AddPointsRequest request)
        {
            try
            {
                var userObject = GetUser(user);
                userObject.AddPoints(request);
                return Ok();
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is ArgumentException)
                {
                    return new BadRequestResult();
                }
                throw;
            }
        }

        /// <summary>
        /// Deducts points from a user's balance
        /// </summary>
        /// <param name="user">Target user to add points to</param>
        /// <param name="request">Json request containing the point value to be deducted</param>
        [HttpPost]
        [Route("deduct/{user}")]
        public ActionResult<DeductPointsResponse> DeductPoints(string user, DeductPointsRequest request)
        {
            try
            {
                var userObject = GetUser(user);
                var result = userObject.DeductPoints(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException || e is ArgumentException)
                {
                    return new BadRequestResult();
                }
                throw;
            }
        }

        /// <summary>
        /// Returns a dictionary of payers and total balance excluding deducted points.
        /// </summary>
        [HttpGet]
        [Route("{user}")]
        public ActionResult<PointsBalanceResponse> GetBalance(string user)
        {
            try
            {
                var userObject = GetUser(user);
                var result = userObject.GetPoints();
                return Ok(result);
            }
            catch (ArgumentException)
            {
                return new BadRequestResult();
            }
        }

        /// <summary>
        /// Debugging method to clear current users stored in memory
        /// </summary>
        [HttpPatch]
        [Route("clear")]
        public IActionResult ClearUsers()
        {
            Startup.Users.Clear();
            return Ok();
        }

        /// <summary>
        /// Helper method to create a user if they don't exist, and to ensure that the pathing works properly
        /// </summary>
        private User GetUser(string user)
        {
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentException("User cannot be null or whitespace.");
            }
            if (!Startup.Users.Any(u => u.Name == user))
            {
                Startup.Users.Add(new User(user));
            }
            return Startup.Users.FirstOrDefault(u => u.Name == user);
        }
    }
}
