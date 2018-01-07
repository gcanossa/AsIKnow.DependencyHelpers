using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Redis
{
    public class DistributedCacheDependencyChecker : DependencyCheckBase
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }
        protected IDistributedCache _ctx;
        public DistributedCacheDependencyChecker(IDistributedCache ctx, string name, TimeSpan timeBeforeFail)
            :base(name, timeBeforeFail)
        {
            _ctx = ctx;
        }
        
        public override async Task<bool> CheckAsync()
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
    }
}
