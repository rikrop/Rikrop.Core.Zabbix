using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Rikrop.Core.Zabbix
{
    public class ZabbixAgent
    {
        private readonly IZabbixMessageHandler _messageHandler;
        private readonly TcpListener _listener;
        public EventHandler<GenericEventArgs<Exception>> Error;
        private volatile bool _active;

        public ZabbixAgent(int port, IZabbixMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Stop()
        {
            _active = false;
        }

        public void Start()
        {            
            _listener.Start();
            _active = true;
            var task = new Task(Loop);
            task.Start();
        }

        protected virtual void OnError(Exception exception)
        {
            var handler = Error;
            if (handler != null)
            {
                handler(this, new GenericEventArgs<Exception>(exception));
            }
        }

        private async void Loop()
        {
            while (_active)
            {
                try
                {
                    using (var client = await _listener.AcceptTcpClientAsync())
                    {
                        using (var stream = client.GetStream())
                        {
                            var zabbixProtocol = new ZabbixProtocol(stream);
                            var request = await zabbixProtocol.ReceiveAsync();

                            ZabbixMessage response;
                            try
                            {
                                response = _messageHandler.Handle(request);
                            }
                            catch
                            {
                                response = ZabbixMessage.ZbxError();
                            }                            

                            await zabbixProtocol.SendAsync(response);
                        }
                    }
                }
                catch (Exception exception)
                {
                    OnError(exception);
                }
            }

            _listener.Stop();
        }
    }
}