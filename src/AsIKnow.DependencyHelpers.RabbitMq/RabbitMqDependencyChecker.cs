using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers.RabbitMq
{
    public class RabbitMqDependencyChecker : DependencyCheckBase
    {
        public override string ToString()
        {
            return $"{GetType().FullName}:{_ctx.GetType().FullName}:{Name}";
        }
        protected ConnectionFactory _ctx;
        public RabbitMqDependencyChecker(ConnectionFactory ctx, string name, TimeSpan timeBeforeFail)
            : base(name, timeBeforeFail)
        {
            _ctx = ctx;
        }

        public override Task<bool> CheckAsync()
        {
            IConnection conn = null;
            try
            {
                conn = _ctx.CreateConnection();

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                List<Exception> report = FailureReport as List<Exception> ?? new List<Exception>();
                report.Add(e);

                return Task.FromResult(false);
            }
            finally
            {
                if (conn != null && conn.IsOpen)
                    conn.Close();
            }
        }
    }
}
