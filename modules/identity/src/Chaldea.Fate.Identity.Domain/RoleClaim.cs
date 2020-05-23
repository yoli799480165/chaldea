using System;

namespace Chaldea.Fate.Identity.Domain
{
    public class RoleClaim : Claim
    {
        public virtual Guid RoleId { get; set; }


    }
}