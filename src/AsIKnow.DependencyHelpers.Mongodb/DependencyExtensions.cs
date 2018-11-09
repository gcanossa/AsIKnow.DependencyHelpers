using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;

namespace AsIKnow.DependencyHelpers.Mongodb
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<MongoDependencyChecker> AddMongodb(this DependencyCheckerBuilder ext, string name)
        {
            return ext.AddMongodb(name, TimeSpan.FromSeconds(ext.Options.CheckTimeout));
        }

        public static DependencyCheckerBuilderStage<MongoDependencyChecker> AddMongodb(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail)
        {
            var options = ext.Options.GetCheckParameter<MongoDependencyCheckOptions>(name) ?? new MongoDependencyCheckOptions();

            var url = ext.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString(options.ConnectionStringName);
            var mongoUrl = new MongoUrl(url);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);

            return new DependencyCheckerBuilderStage<MongoDependencyChecker>(ext,
                ext.AddDependencyCheck(new MongoDependencyChecker(
                    new MongoClient(mongoClientSettings), name, timeBeforeFail)));
        }

        #endregion
    }
}
