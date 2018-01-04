using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AsIKnow.DependencyHelpers
{
    public static class WebHostExtensions
    {
        public static IWebHost CheckDependencies(this IWebHost webHost, Func<DependencyCheckerBuilder, DependencyCheckerBuilder> builder)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                IOptions<DependencyCheckerOptions> options = services.GetRequiredService<IOptions<DependencyCheckerOptions>>();
                ILogger<DependencyChecker> logger = services.GetRequiredService<ILogger<DependencyChecker>>();

                DependencyCheckerBuilder tmp = builder(new DependencyCheckerBuilder(services, options.Value));

                var checker = tmp.Build(options.Value, logger);

                checker.WaitForDependencies();

                return webHost;
            }
        }
    }
}
