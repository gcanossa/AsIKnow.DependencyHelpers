using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public class DirectoryExistsDependencyCheck : DependencyCheckBase
    {
        public string DirectoryPath { get; protected set; }
        public DirectoryExistsDependencyCheck(string directoryPath, string name, TimeSpan timeBeforeFail) : base(name, timeBeforeFail)
        {
            DirectoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
        }

        public override Task<bool> CheckAsync()
        {
            return Task.FromResult(Directory.Exists(DirectoryPath));
        }
    }
}
