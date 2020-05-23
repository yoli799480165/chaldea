using Chaldea.Fate.Identity.Domain;
using Volo.Abp.Modularity;

namespace Chaldea.Fate.Identity.Application
{
    [DependsOn(
        typeof(IdentityDomainModule)
    )]
    public class IdentityApplicationModule : AbpModule
    {
    }
}