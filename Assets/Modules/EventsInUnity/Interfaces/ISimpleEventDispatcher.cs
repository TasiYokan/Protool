using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public interface ISimpleEventDispatcher : IDispatcher
    {
        void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent;
    }
}
