namespace Rikrop.Core.Zabbix
{
    public class ZabbixMessage
    {
        public string Data { get; set; }
        public byte Version { get; set; }

        public ZabbixMessage()
        {
            Version = 1;
        }

        public static ZabbixMessage ZbxError()
        {
            return new ZabbixMessage
                   {
                       Data = "ZBX_ERROR"
                   };
        }

        public static ZabbixMessage ZbxNotsupported()
        {
            return new ZabbixMessage
                   {
                       Data = "ZBX_NOTSUPPORTED"
                   };
        }
    }
}