using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Neo4j.Driver.V1;

namespace AsIKnow.DependencyHelpers.Neo4j
{
    public class Neo4jDependencyCheck : DependencyCheckBase
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_builder.Uri}:{Name}";
        }

        protected DriverBuilder _builder;

        public Neo4jDependencyCheck(DriverBuilder builder, string name, TimeSpan timeBeforeFail)
            :base(name, timeBeforeFail)
        {
            _builder = builder;
            _builder.Config.ConnectionAcquisitionTimeout = CheckUntil - DateTimeOffset.Now;
        }
        
        public override async Task<bool> CheckAsync()
        {
            try
            {
                using (IDriver driver = _builder.GetDriver())
                using (ISession s = driver.Session())
                {
                    await s.RunAsync("MATCH (p) RETURN id(p) limit 0");
                }
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
