using Neo4j.Driver.V1;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers.Neo4j
{
    public abstract class DriverBuilder
    {
        public abstract string Uri { get; }
        public abstract IAuthToken AuthToken { get; }
        public abstract Config Config { get; }

        public virtual IDriver GetDriver()
        {
            return GraphDatabase.Driver(Uri,AuthToken, Config);
        }
    }
}
