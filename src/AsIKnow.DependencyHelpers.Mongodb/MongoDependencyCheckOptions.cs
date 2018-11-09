using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers.Mongodb
{
    public class MongoDependencyCheckOptions : DependencyCheckParameterBase
    {
        public MongoDependencyCheckOptions()
            : base(new Dictionary<string, string>())
        {

        }
        public MongoDependencyCheckOptions(Dictionary<string, string> obj)
            : base(obj)
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));
            if (obj.ContainsKey(nameof(ConnectionStringName)))
                ConnectionStringName = obj[nameof(ConnectionStringName)];
        }
        public string ConnectionStringName { get; set; }
    }
}
