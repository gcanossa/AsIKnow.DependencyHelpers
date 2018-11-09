using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace AsIKnow.DependencyHelpers.RabbitMq
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<RabbitMqDependencyChecker> AddRabbitMq(this DependencyCheckerBuilder ext, string name)
        {
            return ext.AddRabbitMq(name, TimeSpan.FromSeconds(ext.Options.CheckTimeout));
        }

        public static DependencyCheckerBuilderStage<RabbitMqDependencyChecker> AddRabbitMq(this DependencyCheckerBuilder ext, string name, TimeSpan timeBeforeFail)
        {
            var options = ext.Options.GetCheckParameter<RabbitMqDependencyCheckOptions>(name) ?? new RabbitMqDependencyCheckOptions();
            var url = ext.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString(options.ConnectionStringName);
            return new DependencyCheckerBuilderStage<RabbitMqDependencyChecker>(ext,
                ext.AddDependencyCheck(new RabbitMqDependencyChecker(
                    new ConnectionFactory() { Uri = new Uri(url) }, name, timeBeforeFail)));
        }

        #endregion
    }
}
