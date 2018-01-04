using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers.Redis
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilderStage
        public static DependencyCheckerBuilder WithDistributedCachePostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext, Func<T, Task> operation) where T : IDistributedCache
        {
            T arg = ext.Builder.ServiceProvider.GetRequiredService<T>();
            ext.Check.CustomPostCheckOperation = async () =>
            {
                await operation(arg);
            };
            return ext.Builder;
        }
        public static DependencyCheckerBuilder WithoutDistributedCachePostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext) where T : IDistributedCache
        {
            ext.Check.CustomPostCheckOperation = null;
            return ext.Builder;
        }
        #endregion
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<T> AddDistributedCache<T>(this DependencyCheckerBuilder ext, string name) where T : IDistributedCache
        {
            IDistributedCache cache = ext.ServiceProvider.GetRequiredService<IDistributedCache>();
            if (cache is T == false)
                throw new InvalidOperationException($"Check caanot be applied for cache of type \"{cache.GetType().FullName}\". Expected {typeof(T).FullName}");

            return new DependencyCheckerBuilderStage<T>(ext, new DistributedCacheDependencyChecker(cache, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout)));
        }

        public static DependencyCheckerBuilderStage<T> AddDistributedCache<T>(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail) where T : IDistributedCache
        {
            return new DependencyCheckerBuilderStage<T>(ext, new DistributedCacheDependencyChecker(ext.ServiceProvider.GetRequiredService<T>(), name, timeBeforeFail));
        }

        #endregion
    }
}
