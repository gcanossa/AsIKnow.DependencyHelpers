using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public class CustomDependencyCheck : DependencyCheckBase
    {
        public Func<Task<bool>> CustomCheck { get; protected set; }
        public CustomDependencyCheck(Func<Task<bool>> customCheck, string name, TimeSpan timeBeforeFail) : base(name, timeBeforeFail)
        {
            CustomCheck = customCheck ?? throw new ArgumentNullException(nameof(customCheck));
        }

        public override async Task<bool> CheckAsync()
        {
            return await CustomCheck();
        }
    }
}
