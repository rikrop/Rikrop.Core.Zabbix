using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rikrop.Core.Zabbix
{
    internal class ZabbixProtocol : IDisposable
    {        
        private static readonly Encoding Encoding = Encoding.ASCII;
        private static readonly byte[] Header = new byte[] {0x5a, 0x42, 0x58, 0x44};

        private readonly Stream _stream;

        public ZabbixProtocol(Stream stream)
        {
            _stream = stream;
        }       

        public void Dispose()
        {
            _stream.Dispose();
        }

        public async Task<string> ReceiveAsync()
        {
            var reader = new StreamReader(_stream, Encoding);
            var result = await reader.ReadLineAsync();
            return result;
        }

        public async Task SendAsync(ZabbixMessage message)
        {            
            // ≈сли отправить не одной последовательностью байтов, zabbix_get не распознает ответ,
            // вот такой кривой zabbix_get. ¬ zabbix server проблемы может и не быть, но нет времени вы€сн€ть.
            
            var memoryStream = new MemoryStream();
            var binaryWriter = new BinaryWriter(memoryStream, Encoding);
            binaryWriter.Write(Header);
            binaryWriter.Write(message.Version);

            var str = message.Data + '\n';
            var strBytes = Encoding.GetBytes(str);

            binaryWriter.Write((long)strBytes.Length);
            // ћетод Write(string value) из BinaryWriter использовать нельз€:
            // This method first writes the length of the string as a UTF-7 encoded unsigned integer.
            binaryWriter.Write(strBytes);
            var bytes = memoryStream.ToArray();
            await _stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}