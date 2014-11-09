using System;
using System.Collections.Generic;

namespace Rikrop.Core.Zabbix
{
    public class ZabbixMessageHandler : IZabbixMessageHandler, IZabbixMessageHandlerConfigurator
    {
        private readonly Dictionary<string, IZabbixAgentDataSource> _factories;

        public ZabbixMessageHandler()
        {
            _factories = new Dictionary<string, IZabbixAgentDataSource>();
        }

        public void Register(string key, IZabbixAgentDataSource zabbixAgentMessageFactory)
        {
            _factories.Add(key, zabbixAgentMessageFactory);
        }

        public void Register(string key, Func<string> zabbixMessageFactory)
        {
            _factories.Add(key, new GenericZabbixAgentDataSource(zabbixMessageFactory));
        }

        public ZabbixMessage Handle(string request)
        {
            var factory = _factories[request];
            var data = factory.GetData();
            return new ZabbixMessage
                       {
                           Data = data
                       };
        }
    }
}