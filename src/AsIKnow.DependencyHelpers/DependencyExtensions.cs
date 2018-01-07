using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AsIKnow.DependencyHelpers
{
    public static class DependencyExtensions
    {
        #region DependencyCheckerBuilderStage
        public static DependencyCheckerBuilder WithPostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext, Func<T, Task> operation) where T : class, IDependencyCheck
        {
            ext.Check.CustomPostCheckOperation = async () =>
            {
                await operation(ext.Check);
            };
            return ext.Builder;
        }
        public static DependencyCheckerBuilder WithoutPostCheckOperation<T>(this DependencyCheckerBuilderStage<T> ext) where T : class, IDependencyCheck
        {
            ext.Check.CustomPostCheckOperation = null;
            return ext.Builder;
        }
        #endregion
        #region DependencyCheckerBuilder
        public static DependencyCheckerBuilderStage<FileExistsDependencyCheck> AddFileExists(this DependencyCheckerBuilder ext, string filePath, string name)
        {
            return new DependencyCheckerBuilderStage<FileExistsDependencyCheck>(ext, ext.AddDependencyCheck(new FileExistsDependencyCheck(filePath, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }
        public static DependencyCheckerBuilderStage<FileExistsDependencyCheck> AddFileExists(this DependencyCheckerBuilder ext, string filePath, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<FileExistsDependencyCheck>(ext, ext.AddDependencyCheck(new FileExistsDependencyCheck(filePath, name, timeBeforeFail)));
        }

        public static DependencyCheckerBuilderStage<DirectoryExistsDependencyCheck> AddDirectoryExists(this DependencyCheckerBuilder ext, string directoryPath, string name)
        {
            return new DependencyCheckerBuilderStage<DirectoryExistsDependencyCheck>(ext, ext.AddDependencyCheck(new DirectoryExistsDependencyCheck(directoryPath, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }
        public static DependencyCheckerBuilderStage<DirectoryExistsDependencyCheck> AddDirectoryExists(this DependencyCheckerBuilder ext, string directoryPath, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<DirectoryExistsDependencyCheck>(ext, ext.AddDependencyCheck(new DirectoryExistsDependencyCheck(directoryPath, name, timeBeforeFail)));
        }

        public static DependencyCheckerBuilderStage<TcpConnectionDependencyCheck> AddTcpConnection(this DependencyCheckerBuilder ext, IPEndPoint endpoint, string name)
        {
            return new DependencyCheckerBuilderStage<TcpConnectionDependencyCheck>(ext, ext.AddDependencyCheck(new TcpConnectionDependencyCheck(endpoint, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }
        public static DependencyCheckerBuilderStage<TcpConnectionDependencyCheck> AddTcpConnection(this DependencyCheckerBuilder ext, IPEndPoint endpoint, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<TcpConnectionDependencyCheck>(ext, ext.AddDependencyCheck(new TcpConnectionDependencyCheck(endpoint, name, timeBeforeFail)));
        }

        public static DependencyCheckerBuilderStage<CustomDependencyCheck> AddCustom(this DependencyCheckerBuilder ext, Func<Task<bool>> customCheck, string name)
        {
            return new DependencyCheckerBuilderStage<CustomDependencyCheck>(ext, ext.AddDependencyCheck(new CustomDependencyCheck(customCheck, name, TimeSpan.FromSeconds(ext.Options.CheckTimeout))));
        }
        public static DependencyCheckerBuilderStage<CustomDependencyCheck> AddCustom(this DependencyCheckerBuilder ext, Func<Task<bool>> customCheck, string name, TimeSpan timeBeforeFail)
        {
            return new DependencyCheckerBuilderStage<CustomDependencyCheck>(ext, ext.AddDependencyCheck(new CustomDependencyCheck(customCheck, name, timeBeforeFail)));
        }

        #endregion
    }
}
