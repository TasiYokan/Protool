using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public delegate void BaseEventHandler<TEvent>(IDispatcher sender, TEvent e)
        where TEvent : BaseEvent;

    public class BaseEvent
    {
        public static readonly BaseEvent Empty = new BaseEvent();

        public string eventType = "";

        public BaseEvent()
        {
        }
    }
}