using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers.EF
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<DbContextDependencyCheck> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<DbContextDependencyCheck>(
                ext, 
                ext.AddDependencyCheck(
                    new DbContextDependencyCheck(
                        ext.ServiceProvider.GetRequiredService<T>(), 
                        name, 
                        TimeSpan.FromSeconds(ext.Options.CheckTimeout),
                        ext.Options.GetCheckParameter<EFDependencyCheckOptions>(name) ?? new EFDependencyCheckOptions())));
        }

        public static DependencyCheckerBuilderStage<DbContextDependencyCheck> AddEntityFramewrokDbContext<T>(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail) where T : DbContext
        {
            return new DependencyCheckerBuilderStage<DbContextDependencyCheck>(
                ext, 
                ext.AddDependencyCheck(
                    new DbContextDependencyCheck(
                        ext.ServiceProvider.GetRequiredService<T>(), 
                        name, 
                        timeBeforeFail,
                        ext.Options.GetCheckParameter<EFDependencyCheckOptions>(name) ?? new EFDependencyCheckOptions())));
        }

        #endregion
    }
}
