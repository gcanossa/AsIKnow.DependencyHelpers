using AsIKnow.DependencyHelpers;
using AsIKnow.DependencyHelpers.EF;
using AsIKnow.DependencyHelpers.Mongodb;
using AsIKnow.DependencyHelpers.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class UnitTest1
    {
        [Trait("Category", "Confiuration")]
        [Fact(DisplayName = nameof(Configuration))]
        public void Configuration()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            ServiceCollection sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(Configuration);
            sc.AddOptions();
            sc.Configure<DependencyCheckerOptions>(Configuration.GetSection("DependencyCheck"));

            IServiceProvider services = sc.BuildServiceProvider();

            IOptions<DependencyCheckerOptions> options = services.GetRequiredService<IOptions<DependencyCheckerOptions>>();

            DependencyCheckerOptions result = options.Value;

            EFDependencyCheckOptions opt = result.GetCheckParameter<EFDependencyCheckOptions>("test");
            RabbitMqDependencyCheckOptions opt1 = result.GetCheckParameter<RabbitMqDependencyCheckOptions>("rabbit");
            MongoDependencyCheckOptions opt2 = result.GetCheckParameter<MongoDependencyCheckOptions>("mongo");
        }

        [Trait("Category", "Check")]
        [Fact(DisplayName = nameof(RabbitmqCheck))]
        public void RabbitmqCheck()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            ServiceCollection sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(Configuration);
            sc.AddOptions();
            sc.Configure<DependencyCheckerOptions>(Configuration.GetSection("DependencyCheck"));

            IServiceProvider services = sc.BuildServiceProvider();

            IOptions<DependencyCheckerOptions> options = services.GetRequiredService<IOptions<DependencyCheckerOptions>>();

            DependencyCheckerOptions result = options.Value;
            RabbitMqDependencyCheckOptions opt1 = result.GetCheckParameter<RabbitMqDependencyCheckOptions>("rabbit");
            
            var check = new RabbitMqDependencyChecker(
                    new ConnectionFactory() { Uri = new Uri(Configuration.GetConnectionString(opt1.ConnectionStringName)) }, "test", TimeSpan.FromSeconds(10));

            Assert.False(check.Check());
        }

        [Trait("Category", "Check")]
        [Fact(DisplayName = nameof(MongoCheck))]
        public void MongoCheck()
        {
            IConfigurationRoot Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            ServiceCollection sc = new ServiceCollection();
            sc.AddSingleton<IConfiguration>(Configuration);
            sc.AddOptions();
            sc.Configure<DependencyCheckerOptions>(Configuration.GetSection("DependencyCheck"));

            IServiceProvider services = sc.BuildServiceProvider();

            IOptions<DependencyCheckerOptions> options = services.GetRequiredService<IOptions<DependencyCheckerOptions>>();

            DependencyCheckerOptions result = options.Value;
            MongoDependencyCheckOptions opt2 = result.GetCheckParameter<MongoDependencyCheckOptions>("mongo");
            
            var mongoUrl = new MongoUrl(Configuration.GetConnectionString(opt2.ConnectionStringName));
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrl);

            var check = new MongoDependencyChecker(
                    new MongoClient(mongoClientSettings), "test", TimeSpan.FromSeconds(10));

            Assert.False(check.Check());
        }
    }
}
