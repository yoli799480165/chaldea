using System;
using Volo.Abp.Domain.Entities;

namespace Chaldea.Fate.Identity.Domain
{
    public class Secret: Entity<Guid>
    {
        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; } = "SharedSecret";
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
