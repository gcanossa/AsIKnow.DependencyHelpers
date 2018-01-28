using AsIKnow.DependencyHelpers;
using AsIKnow.DependencyHelpers.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        }
    }
}
