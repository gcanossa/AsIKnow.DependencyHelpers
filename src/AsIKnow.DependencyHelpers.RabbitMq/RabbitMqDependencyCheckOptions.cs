using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers.RabbitMq
{
    public class RabbitMqDependencyCheckOptions : DependencyCheckParameterBase
    {
        public RabbitMqDependencyCheckOptions()
            : base(new Dictionary<string, string>())
        {

        }
        public RabbitMqDependencyCheckOptions(Dictionary<string, string> obj)
            : base(obj)
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));
            if (obj.ContainsKey(nameof(Uri)))
                Uri = obj[nameof(Uri)];
        }
        public string Uri { get; set; }
    }
}
