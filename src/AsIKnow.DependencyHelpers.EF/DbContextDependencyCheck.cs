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
        public DbContextDependencyCheck(DbContext ctx, string name, TimeSpan timeBeforeFail, EFDependencyCheckOptions options)
            :base(name, timeBeforeFail)
        {
            _ctx = ctx;
            _doMigrations = options.Migrate;
        }
        
        public override async Task<bool> CheckAsync()
        {
            try
            {
                await _ctx.Database.OpenConnectionAsync();

                if (_doMigrations)
                {
                    await _ctx.Database.MigrateAsync();
                }

                return true;
            }
            catch (Exception e)
            {
                List<Exception> report = FailureReport as List<Exception> ?? new List<Exception>();
                report.Add(e);

                return false;
            }
            finally
            {
                if (_ctx.Database.GetDbConnection() != null)
                    _ctx.Database.CloseConnection();
            }
        }
    }
}
