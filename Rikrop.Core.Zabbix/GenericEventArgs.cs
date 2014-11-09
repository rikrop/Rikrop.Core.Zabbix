using System;
namespace Rikrop.Core.Zabbix
{
    public class GenericEventArgs<T> : EventArgs
    {
        private readonly T _eventData;

        public GenericEventArgs(T data)
        {
            _eventData = data;
        }
        public T Data
        {
            get { return _eventData; }
        }
    }
}
