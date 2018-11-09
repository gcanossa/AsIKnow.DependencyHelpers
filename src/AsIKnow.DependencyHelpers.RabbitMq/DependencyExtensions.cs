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
            return new DependencyCheckerBuilderStage<RabbitMqDependencyChecker>(ext,
                ext.AddDependencyCheck(new RabbitMqDependencyChecker(
                    new ConnectionFactory() { Uri = new Uri(options.Uri) }, name, timeBeforeFail)));
        }

        #endregion
    }
}
