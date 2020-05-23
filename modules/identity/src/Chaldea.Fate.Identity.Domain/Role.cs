using System;
using Volo.Abp.Domain.Entities;

namespace Chaldea.Fate.Identity.Domain
{
    public class Role : AggregateRoot<Guid>
    {
        public virtual string Name { get; set; }

        public virtual string NormalizedName { get; set; }
    }
}