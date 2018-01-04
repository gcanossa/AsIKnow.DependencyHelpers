using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public interface IDependencyCheck
    {
        string Name { get; }
        DateTimeOffset CheckUntil { get; }
        Func<Task> CustomPostCheckOperation { get; set; }
        bool Check();
        Task<bool> CheckAsync();
        void PostCheckOperation();
        Task PostCheckOperationAsync();
    }
}
