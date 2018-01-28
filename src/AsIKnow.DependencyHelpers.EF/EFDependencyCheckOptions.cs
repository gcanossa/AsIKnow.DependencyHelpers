using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.DependencyHelpers.EF
{
    public class EFDependencyCheckOptions : DependencyCheckParameterBase
    {
        public EFDependencyCheckOptions()
            :base(new Dictionary<string, string>())
        {

        }
        public EFDependencyCheckOptions(Dictionary<string, string> obj)
            :base(obj)
        {
            obj = obj ?? throw new ArgumentNullException(nameof(obj));
            if (obj.ContainsKey(nameof(Migrate)))
                Migrate = Convert.ToBoolean(obj[nameof(Migrate)]);
        }
        public bool Migrate { get; set; } = false;
    }
}
