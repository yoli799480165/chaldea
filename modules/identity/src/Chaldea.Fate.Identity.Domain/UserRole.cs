using System;
using Volo.Abp.Domain.Entities;

namespace Chaldea.Fate.Identity.Domain
{
    public class UserRole : Entity
    {
        public virtual Guid UserId { get; set; }

        public virtual Guid RoleId { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { UserId, RoleId };
        }
    }
}