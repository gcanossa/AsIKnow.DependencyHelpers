using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers
{
    public class DependencyCheckerBuilder
    {
        protected Dictionary<string, IDependencyCheck> _checks = new Dictionary<string, IDependencyCheck>();

        public IServiceProvider ServiceProvider { get; private set; }
        public DependencyCheckerOptions Options { get; private set; }
        protected ILogger<DependencyChecker> Logger { get; set; }
        public DependencyCheckerBuilder(IServiceProvider provider, DependencyCheckerOptions options)
        {
            ServiceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            Options = options ?? throw new ArgumentNullException(nameof(options));
            Logger = ServiceProvider.GetRequiredService<ILogger<DependencyChecker>>();
        }

        public IDependencyCheck AddDependencyCheck(IDependencyCheck check)
        {
            if (check == null)
                throw new ArgumentNullException(nameof(check));
            _checks.Add(check.ToString(), check);
            return check;
        }

        public DependencyChecker Build()
        {
            return new DependencyChecker(_checks.Values, Options, Logger);
        }
    }
}
