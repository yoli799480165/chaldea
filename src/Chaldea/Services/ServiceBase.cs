using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services
{
    public class ServiceBase : ControllerBase
    {
        public string UserId
        {
            get { return User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value; }
        }
    }
}