using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.Mongodb
{
    public class MongoDependencyChecker : DependencyCheckBase
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }
        protected MongoClient _ctx;
        public MongoDependencyChecker(MongoClient ctx, string name, TimeSpan timeBeforeFail)
            : base(name, timeBeforeFail)
        {
            _ctx = ctx;
        }

        public override async Task<bool> CheckAsync()
        {
            try
            {
                var lst = await _ctx.ListDatabaseNamesAsync();

                return true;
            }
            catch (Exception e)
            {
                List<Exception> report = FailureReport as List<Exception> ?? new List<Exception>();
                report.Add(e);

                return false;
            }
        }
    }
}
