using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public interface IFlowEventDispatcher : IDispatcher
    {
        void DispatchFlowEvent<T>(string _evtType, T _evt, bool _isOriginal)
            where T : FlowEvent;
        void RedispatchFlowEvent<T>(T _evt)
            where T : FlowEvent;
        void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent;
    }
}
