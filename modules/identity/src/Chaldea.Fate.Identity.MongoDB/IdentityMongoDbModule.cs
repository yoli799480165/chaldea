using Chaldea.Fate.Identity.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Chaldea.Fate.Identity.MongoDB
{
    [DependsOn(
        typeof(IdentityDomainModule),
        typeof(AbpMongoDbModule)
    )]
    public class IdentityMongoDbModule : AbpModule
    {
    }
}
