using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsIKnow.DependencyHelpers
{
    public class TcpConnectionDependencyCheck : DependencyCheckBase
    {
        public IPEndPoint Endpoint { get; protected set; }
        public TcpConnectionDependencyCheck(IPEndPoint endpoint, string name, TimeSpan timeBeforeFail) : base(name, timeBeforeFail)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint)); 
        }

        public override async Task<bool> CheckAsync()
        {
            TcpClient client = new TcpClient();
            try
            {
                await client.ConnectAsync(Endpoint.Address, Endpoint.Port);

                return true;
            }
            catch(Exception e)
            {
                List<Exception> report = FailureReport as List<Exception> ?? new List<Exception>();
                report.Add(e);

                return false;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
