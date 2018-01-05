using Neo4j.Driver.V1;
using System;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Neo4j
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilderStage
        public static DependencyCheckerBuilder WithNeo4jServerPostCheckOperation(this DependencyCheckerBuilderStage<IDriver> ext, Func<IDriver, Task> operation)
        {
            ext.Check.CustomPostCheckOperation = async () =>
            {
                await operation(null);
            };
            return ext.Builder;
        }
        public static DependencyCheckerBuilder WithoutNeo4jServerPostCheckOperation(this DependencyCheckerBuilderStage<IDriver> ext)
        {
            ext.Check.CustomPostCheckOperation = null;
            return ext.Builder;
        }
        #endregion
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<IDriver> AddNeo4jServer(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name)
        {
            return new DependencyCheckerBuilderStage<IDriver>(ext, ext.AddDependencyCheck(new Neo4jDependencyCheck(serverUri, token, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }

        public static DependencyCheckerBuilderStage<IDriver> AddNeo4jServer<T>(this DependencyCheckerBuilder ext, Uri serverUri, IAuthToken token, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<IDriver>(ext, ext.AddDependencyCheck(new Neo4jDependencyCheck(serverUri, token, name, timeBeforeFail)));
        }

        #endregion
    }
}
