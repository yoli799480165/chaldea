using System;
using System.Linq;
using Chaldea.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Chaldea.Services
{
    public class ServiceBase : ControllerBase
    {
        public bool IsLogin => !string.IsNullOrEmpty(UserId);

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

        public int MaxLevel
        {
            get
            {
                if (string.IsNullOrEmpty(UserId)) return (int) AnimeLevel.R0;

                if (UserRole == UserRoles.Admin) return (int) AnimeLevel.R999;

                if (UserRole == UserRoles.Master) return (int) AnimeLevel.R18;

                if (UserRole == UserRoles.Servant) return (int) AnimeLevel.R15;

                return (int) AnimeLevel.R12;
            }
        }
    }
}