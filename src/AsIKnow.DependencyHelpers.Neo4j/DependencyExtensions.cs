using Neo4j.Driver.V1;
using System;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Neo4j
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<Neo4jDependencyCheck> AddNeo4jServer(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name)
        {
            return new DependencyCheckerBuilderStage<Neo4jDependencyCheck>(ext, ext.AddDependencyCheck(new Neo4jDependencyCheck(serverUri, token, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }

        public static DependencyCheckerBuilderStage<Neo4jDependencyCheck> AddNeo4jServer<T>(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<Neo4jDependencyCheck>(ext, ext.AddDependencyCheck(new Neo4jDependencyCheck(serverUri, token, name, timeBeforeFail)));
        }
        #endregion
    }
}
