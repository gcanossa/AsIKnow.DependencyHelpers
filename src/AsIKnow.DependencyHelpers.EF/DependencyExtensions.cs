using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers.EF
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<DbContextDependencyCheck> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name, bool migrate = false) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<DbContextDependencyCheck>(ext, ext.AddDependencyCheck(new DbContextDependencyCheck(ext.ServiceProvider.GetRequiredService<T>(), name, TimeSpan.FromSeconds(ext.Options.CheckTimeout), migrate)));
        }

        public static DependencyCheckerBuilderStage<DbContextDependencyCheck> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail, bool migrate = false) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<DbContextDependencyCheck>(ext, ext.AddDependencyCheck(new DbContextDependencyCheck(ext.ServiceProvider.GetRequiredService<T>(), name, timeBeforeFail, migrate)));
        }

        #endregion
    }
}
