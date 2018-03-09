using System;
using UnityEngine;

namespace TasiYokan.EventsInUnity
{
    public class BaseEventDispatcher : IDispatcher
    {
        #region Fields

        private readonly EventRegister m_eventRegister = null;
        private string m_dispatchingEvent = "";
        protected readonly IDispatcher m_host = null;

        protected EventRegister EventRegister
        {
            get
            {
                return m_eventRegister;
            }
        }

        #endregion Fields

        #region Constructors

        public BaseEventDispatcher(IDispatcher _host)
        {
            if(_host != null)
                m_host = _host;
            else
                m_host = this;
            m_eventRegister = new EventRegister(m_host);
        }

        #endregion Constructors

        #region Methods

        public void AddEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            if(m_dispatchingEvent == _evtType)
            {
                // Add it later
                return;
            }
            m_eventRegister.AddEventListener<T>(_evtType, _handler);
        }

        public void RemoveEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            if(m_dispatchingEvent == _evtType)
            {
                // Remove it later
                return;
            }
            m_eventRegister.RemoveEventListener<T>(_evtType, _handler);
        }

        public void RemoveAllEventListeners()
        {
            m_eventRegister.RemoveAllListeners();
        }

        #endregion Methods
    }
}