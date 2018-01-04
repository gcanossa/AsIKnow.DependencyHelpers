using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Redis
{
    public class DistributedCacheDependencyChecker : IDependencyCheck
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }
        protected IDistributedCache _ctx;
        public DistributedCacheDependencyChecker(IDistributedCache ctx, string name, TimeSpan timeBeforeFail)
        {
            _ctx = ctx;
            Name = name;
            CheckUntil = DateTimeOffset.Now + timeBeforeFail;
        }

        public DateTimeOffset CheckUntil { get; protected set; }
        public Func<Task> CustomPostCheckOperation { get; set; }
        public string Name { get; protected set; }

        public bool Check()
        {
            return CheckAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> CheckAsync()
        {
            try
            {
                await _ctx.SetStringAsync($"check:{typeof(DistributedCacheDependencyChecker).FullName}:{Name}", new Guid().ToString(),
                    new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(500) });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void PostCheckOperation()
        {
            
        }

        public Task PostCheckOperationAsync()
        {
            return Task.CompletedTask;
        }
    }
}
