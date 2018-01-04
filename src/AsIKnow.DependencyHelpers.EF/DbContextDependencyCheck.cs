using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.EF
{
    public class DbContextDependencyCheck : IDependencyCheck
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }

        protected DbContext _ctx;
        protected bool _doMigrations;
        public DbContextDependencyCheck(DbContext ctx, string name, TimeSpan timeBeforeFail, bool doMigrations = false)
        {
            _ctx = ctx;
            Name = name;
            CheckUntil = DateTimeOffset.Now + timeBeforeFail;
            _doMigrations = doMigrations;
        }

        public DateTimeOffset CheckUntil { get; protected set; }
        public Func<Task> CustomPostCheckOperation { get; set; }
        public string Name { get; protected set; }

        public bool Check()
        {
            return CheckAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<bool> CheckAsync()
        {
            try
            {
                await _ctx.Database.OpenConnectionAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (_ctx.Database.GetDbConnection() != null)
                    _ctx.Database.CloseConnection();
            }
        }

        public void PostCheckOperation()
        {
            PostCheckOperationAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task PostCheckOperationAsync()
        {
            if (_doMigrations)
            {
                return _ctx.Database.MigrateAsync();
            }
            else
                return Task.CompletedTask;
        }
    }
}
