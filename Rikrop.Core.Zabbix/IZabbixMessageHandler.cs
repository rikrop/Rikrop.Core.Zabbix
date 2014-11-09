namespace Rikrop.Core.Zabbix
{
    public interface IZabbixMessageHandler
    {
        ZabbixMessage Handle(string request);
    }
}