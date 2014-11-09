using System;
using System.Diagnostics.Contracts;

namespace Rikrop.Core.Zabbix
{
    public class GenericZabbixAgentDataSource : IZabbixAgentDataSource
    {
        private readonly Func<string> _getDataDelegate;

        public GenericZabbixAgentDataSource(Func<string> getDataDelegate)
        {
            Contract.Requires<ArgumentNullException>(getDataDelegate != null);

            _getDataDelegate = getDataDelegate;
        }

        public string GetData()
        {
            return _getDataDelegate();
        }
    }
}