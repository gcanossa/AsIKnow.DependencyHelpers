using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public abstract class DependencyCheckBase : IDependencyCheck
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{Name}";
        }
        
        public DependencyCheckBase(string name, TimeSpan timeBeforeFail)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            CheckUntil = DateTimeOffset.Now + timeBeforeFail;
        }

        public DateTimeOffset CheckUntil { get; protected set; }
        public Func<Task> CustomPostCheckOperation { get; set; }
        public string Name { get; protected set; }

        public IEnumerable<Exception> FailureReport { get; protected set; }

        public virtual bool Check()
        {
            return CheckAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public abstract Task<bool> CheckAsync();
    }
}
