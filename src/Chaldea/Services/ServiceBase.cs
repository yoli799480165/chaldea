using System;
using System.Linq;
using Chaldea.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services
{
    public class ServiceBase : ControllerBase
    {
        public string UserId
        {
            get { return User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value; }
        }

        public UserRoles UserRole
        {
            get
            {
                var role = User.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
                if (!string.IsNullOrEmpty(role))
                    if (Enum.TryParse(role, out UserRoles userRole))
                        return userRole;

                return UserRoles.Human;
            }
        }
    }
}