using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public class FileExistsDependencyCheck : DependencyCheckBase
    {
        public string FilePath { get; protected set; }
        public FileExistsDependencyCheck(string filePath, string name, TimeSpan timeBeforeFail) : base(name, timeBeforeFail)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public override Task<bool> CheckAsync()
        {
            return Task.FromResult(File.Exists(FilePath));
        }
    }
}
