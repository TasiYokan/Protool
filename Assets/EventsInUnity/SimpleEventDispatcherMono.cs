using UnityEngine;
using System;

namespace TasiYokan.EventsInUnity
{
    public class SimpleEventDispatcherMono : MonoBehaviour,
        ISimpleEventDispatcher
    {
        private SimpleEventDispatcher m_dispatcher;
        private SimpleEventDispatcher Dispatcher
        {
            get
            {
                if(m_dispatcher == null)
                    m_dispatcher = new SimpleEventDispatcher(this);
                return m_dispatcher;
            }
        }

        public void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent
        {
            Dispatcher.DispatchEvent<T>(_evtType, _evt);
        }

        public void AddEventListener(string _evtType, BaseEventHandler<BaseEvent> _handler)
        {
            Dispatcher.AddEventListener<BaseEvent>(_evtType, _handler);
        }
        public void AddEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            Dispatcher.AddEventListener<T>(_evtType, _handler);
        }

        public void RemoveEventListener(string _evtType, BaseEventHandler<BaseEvent> _handler)
        {
            Dispatcher.RemoveEventListener<BaseEvent>(_evtType, _handler);
        }
        public void RemoveEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            Dispatcher.RemoveEventListener<T>(_evtType, _handler);
        }
        public void RemoveAllEventListeners()
        {
            Dispatcher.RemoveAllEventListeners();
        }
    }
}