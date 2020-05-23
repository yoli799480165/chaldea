using System;

namespace Chaldea.Fate.Identity.Domain
{
    public class UserClaim : Claim
    {
        public virtual Guid UserId { get; set; }
    }
}
