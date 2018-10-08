using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer4.TestingClient.Controllers
{
    public class TestController : Controller
    {
        [Authorize]
        [Route("api/test/auth1")]
        [HttpGet]
        public void Auth1()
        {
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }

        [Authorize(Roles = "admin")]
        [Route("api/test/auth2")]
        [HttpGet]
        public void Auth2()
        {
            foreach (var claim in HttpContext.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }
        }
    }
}