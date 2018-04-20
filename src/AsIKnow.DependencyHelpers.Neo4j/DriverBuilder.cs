using N4pper;
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

    public class DriverBuilder<T> : DriverBuilder where T : DriverProvider
    {
        public T Provider { get; protected set; }
        public DriverBuilder(T provider)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public override IDriver GetDriver()
        {
            return Provider.GetDriver();
        }

        public override string Uri => Provider.Uri;

        public override IAuthToken AuthToken => Provider.AuthToken;

        public override Config Config => Provider.Config;
    }
}
