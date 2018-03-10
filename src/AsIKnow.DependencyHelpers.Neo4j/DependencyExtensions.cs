using Microsoft.Extensions.DependencyInjection;
using Neo4j.Driver.V1;
using System;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Neo4j
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<Neo4jDependencyCheck> AddNeo4jServer<T>(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name) where T : DriverBuilder
        {
            return new DependencyCheckerBuilderStage<Neo4jDependencyCheck>(
                ext, 
                ext.AddDependencyCheck(
                    new Neo4jDependencyCheck(
                        ext.ServiceProvider.GetRequiredService<T>(),
                        name, 
                        TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }

        public static DependencyCheckerBuilderStage<Neo4jDependencyCheck> AddNeo4jServer<T>(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name, TimeSpan timeBeforeFail) where T : DriverBuilder
        {
            return new DependencyCheckerBuilderStage<Neo4jDependencyCheck>(
                ext, 
                ext.AddDependencyCheck(
                    new Neo4jDependencyCheck(
                        ext.ServiceProvider.GetRequiredService<T>(),
                        name, 
                        timeBeforeFail)));
        }
        #endregion
    }
}
