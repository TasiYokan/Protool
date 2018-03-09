using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace TasiYokan.EventsInUnity
{
    public class FlowEventDispatcherMono : MonoBehaviour,
        IFlowEventDispatcher
    {
        private FlowEventDispatcher m_flowDispatcher;
        public FlowEventDispatcher FlowDispatcher
        {
            get
            {
                if(m_flowDispatcher == null)
                    m_flowDispatcher = new FlowEventDispatcher(this);
                return m_flowDispatcher;
            }
        }

        private FlowEventDispatcherMono m_parent;
        public FlowEventDispatcherMono Parent
        {
            get
            {
                return m_parent;
            }

            set
            {
                m_parent = value;
                FlowDispatcher.Parent = value.FlowDispatcher;
            }
        }
        
        /// <typeparam name="T"></typeparam>
        /// <param name="_evtType"></param>
        /// <param name="_evt"></param>
        public void DispatchFlowEvent<T>(string _evtType, T _evt, bool _isOriginal = true)
            where T : FlowEvent
        {
            FlowDispatcher.DispatchFlowEvent<T>(_evtType, _evt, _isOriginal);
        }

        /// <summary>
        /// Dispatch a FLOW event from another FlowEventDispatcher
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_evt"></param>
        public void RedispatchFlowEvent<T>(T _evt)
            where T : FlowEvent
        {
            Assert.AreNotEqual("", _evt.eventType,
                "Redispatched event's type should initiated in dispatch from others");
            FlowDispatcher.DispatchFlowEvent<T>(_evt.eventType, _evt, false);
        }

        public void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent
        {
            FlowDispatcher.DispatchEvent<T>(_evtType, _evt);
        }

        public void AddEventListener(string _evtType, BaseEventHandler<FlowEvent> _handler)
        {
            // Inherit from EventDispatcher.
            FlowDispatcher.AddEventListener<FlowEvent>(_evtType, _handler);
        }
        public void AddEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            FlowDispatcher.AddEventListener<T>(_evtType, _handler);
        }

        public void RemoveEventListener(string _evtType, BaseEventHandler<FlowEvent> _handler)
        {
            // Inherit from EventDispatcher.
            FlowDispatcher.RemoveEventListener<FlowEvent>(_evtType, _handler);
        }
        public void RemoveEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
           where T : BaseEvent
        {
            FlowDispatcher.RemoveEventListener<T>(_evtType, _handler);
        }

        public void RemoveAllEventListeners()
        {
            FlowDispatcher.RemoveAllEventListeners();
        }
    }
}