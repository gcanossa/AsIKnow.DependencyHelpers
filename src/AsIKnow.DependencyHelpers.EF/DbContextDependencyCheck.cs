using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.EF
{
    public class DbContextDependencyCheck : DependencyCheckBase
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }

        protected DbContext _ctx;
        protected bool _doMigrations;
        public DbContextDependencyCheck(DbContext ctx, string name, TimeSpan timeBeforeFail, bool doMigrations = false)
            :base(name, timeBeforeFail)
        {
            _ctx = ctx;
            _doMigrations = doMigrations;
        }
        
        public override async Task<bool> CheckAsync()
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
        
        public override Task PostCheckOperationAsync()
        {
            if (_doMigrations)
            {
                using (IDbContextTransaction tx = _ctx.Database.BeginTransaction())
                {
                    return _ctx.Database.MigrateAsync();
                }
            }
            else
                return Task.CompletedTask;
        }
    }
}
