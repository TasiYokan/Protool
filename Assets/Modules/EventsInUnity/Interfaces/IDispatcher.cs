using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public interface IDispatcher
    {
        void AddEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent;
        void RemoveEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent;
        void RemoveAllEventListeners();
    }
}
