using System;
using System.Diagnostics.Contracts;
using Rikrop.Core.Zabbix.Contracts;

namespace Rikrop.Core.Zabbix
{
    [ContractClass(typeof(ContractIZabbixMessageHandlerConfigurator))]
    public interface IZabbixMessageHandlerConfigurator
    {
        void Register(string key, IZabbixAgentDataSource zabbixAgentMessageFactory);
        void Register(string key, Func<string> zabbixMessageFactory);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IZabbixMessageHandlerConfigurator))]
        public abstract class ContractIZabbixMessageHandlerConfigurator : IZabbixMessageHandlerConfigurator
        {
            public void Register(string key, IZabbixAgentDataSource zabbixAgentMessageFactory)
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
                Contract.Requires<ArgumentNullException>(zabbixAgentMessageFactory != null);
            }

            public void Register(string key, Func<string> zabbixMessageFactory)
            {
                Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
                Contract.Requires<ArgumentNullException>(zabbixMessageFactory != null);
            }
        }
    }
}