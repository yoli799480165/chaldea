using System;
using Volo.Abp.Domain.Entities;

namespace Chaldea.Fate.Identity.Domain
{
    public abstract class Claim : Entity<Guid>
    {
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }
    }
}
