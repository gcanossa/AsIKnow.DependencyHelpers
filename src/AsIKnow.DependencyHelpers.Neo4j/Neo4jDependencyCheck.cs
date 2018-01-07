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
            return $"{GetType().FullName}:{_uri}:{Name}";
        }

        protected Uri _uri;
        protected IAuthToken _token;

        public Neo4jDependencyCheck(Uri uri, IAuthToken token, string name, TimeSpan timeBeforeFail)
            :base(name, timeBeforeFail)
        {
            _uri = uri;
            _token = token;
        }
        
        public override async Task<bool> CheckAsync()
        {
            try
            {
                using (IDriver driver = GraphDatabase.Driver(_uri, _token, new Config() { ConnectionAcquisitionTimeout = CheckUntil - DateTimeOffset.Now }))
                using (ISession s = driver.Session())
                {
                    await s.RunAsync("MATCH (p) RETURN id(p) limit 0");
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
