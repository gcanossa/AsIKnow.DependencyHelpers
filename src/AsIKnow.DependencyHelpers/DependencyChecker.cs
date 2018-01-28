using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public class DependencyChecker
    {
        protected List<IDependencyCheck> _checks;
        protected DependencyCheckerOptions _options;
        protected ILogger<DependencyChecker> _logger;
        
        internal DependencyChecker(IEnumerable<IDependencyCheck> checks, DependencyCheckerOptions options, ILogger<DependencyChecker> logger)
        {
            _checks = new List<IDependencyCheck>(checks);
            _options = options;
            _logger = logger;
        }

        public void WaitForDependencies()
        {
            WaitForDependenciesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        public async Task WaitForDependenciesAsync()
        {
            List<Task> _tmp = new List<Task>();

            foreach (IDependencyCheck item in _checks)
            {
                _tmp.Add(Task.Run(async () =>
                {
                    _logger.LogInformation($"Checking \"{item.Name}\"");
                    while (!await item.CheckAsync())
                    {
                        if (item.CheckUntil < DateTimeOffset.Now)
                        {
                            _logger.LogCritical($"Checker \"{item.Name}\" failed. Out of time.");
                            throw new UnavailableDependencyException($"{item.Name}: {item.GetType().FullName}", item.FailureReport??new List<Exception>());
                        }
                        else
                        {
                            _logger.LogInformation($"Checking \"{item.Name}\"");
                            await Task.Delay(TimeSpan.FromSeconds(_options.CheckInterval));
                        }
                    }
                }));
            }
            
            foreach (Task item in _tmp)
            {
                await item;
            }

            if(_tmp.Any(p=>p.IsFaulted))
            {
                throw new AggregateException(_tmp.Where(p=>p.IsFaulted).Select(p=>p.Exception.InnerException).ToArray());
            }

            foreach (IDependencyCheck item in _checks.Where(p=>p.CustomPostCheckOperation != null))
            {
                _logger.LogInformation($"Performing post check operation for \"{item.Name}\"");
                await item.CustomPostCheckOperation();
            }
        }
    }
}
