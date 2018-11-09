using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers.Redis
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<DistributedCacheDependencyChecker> AddDistributedCache<T>(this DependencyCheckerBuilder ext, string name) where T : IDistributedCache
        {
            IDistributedCache cache = ext.ServiceProvider.GetRequiredService<IDistributedCache>();
            if (cache is T == false)
                throw new InvalidOperationException($"Check cannot be applied for cache of type \"{cache.GetType().FullName}\". Expected {typeof(T).FullName}");

            return new DependencyCheckerBuilderStage<DistributedCacheDependencyChecker>(ext, ext.AddDependencyCheck(new DistributedCacheDependencyChecker(cache, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }

        public static DependencyCheckerBuilderStage<DistributedCacheDependencyChecker> AddDistributedCache<T>(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail) where T : IDistributedCache
        {
            return new DependencyCheckerBuilderStage<DistributedCacheDependencyChecker>(ext, ext.AddDependencyCheck(new DistributedCacheDependencyChecker(ext.ServiceProvider.GetRequiredService<T>(), name, timeBeforeFail)));
        }

        #endregion
    }
}
