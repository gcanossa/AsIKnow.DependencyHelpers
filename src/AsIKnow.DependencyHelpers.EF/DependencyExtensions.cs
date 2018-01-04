using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers.EF
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilderStage
        public static DependencyCheckerBuilder WithEntityFrameworkPostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext, Func<T, Task> operation) where T : DbContext
        {
            T arg = ext.Builder.ServiceProvider.GetRequiredService<T>();
            ext.Check.CustomPostCheckOperation = async () =>
            {
                await operation(arg);
            };
            return ext.Builder;
        }
        public static DependencyCheckerBuilder WithoutEntityFrameworkPostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext) where T : DbContext
        {
            ext.Check.CustomPostCheckOperation = null;
            return ext.Builder;
        }
        
        #endregion
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<T> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name, bool migrate = false) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<T>(ext, new DbContextDependencyCheck(ext.ServiceProvider.GetRequiredService<T>(), name, TimeSpan.FromSeconds(ext.Options.CheckTimeout), migrate));
        }

        public static DependencyCheckerBuilderStage<T> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail, bool migrate = false) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<T>(ext, new DbContextDependencyCheck(ext.ServiceProvider.GetRequiredService<T>(), name, timeBeforeFail, migrate));
        }

        #endregion
    }
}
