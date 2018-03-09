using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public class EventRegister
    {
        #region Fields

        private readonly IDispatcher m_dispatcher;
        private Dictionary<string, List<Delegate>> m_allHandlers;

        #endregion Fields

        #region Properties

        public Dictionary<string, List<Delegate>> AllHandlers
        {
            get
            {
                return m_allHandlers;
            }

            set
            {
                m_allHandlers = value;
            }
        }

        #endregion Properties

        #region Constructors

        public EventRegister(IDispatcher _dispatcher)
        {
            m_dispatcher = _dispatcher;
            m_allHandlers = new Dictionary<string, List<Delegate>>();
        }

        #endregion Constructors

        #region Methods

        public void AddEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            // TODO: Maybe we need to find a way to deal with the situation that subscriber continuously add listen
            //even though we already have one dispatching.
            //if(m_dispatchingEvent == _evtType)
            //{
            //    // Add it later
            //    return;
            //}

            // Make a temporary copy of the event to avoid possibility of a race condition
            // if the last subscriber unsubscribes immediately after the null check and
            // before the event is raised.
            List<Delegate> thisEventHandlers;
            // First find or create the handlers corresponded to this event. 
            if(m_allHandlers.ContainsKey(_evtType))
            {
                thisEventHandlers = m_allHandlers[_evtType];
            }
            else
            {
                m_allHandlers[_evtType] = thisEventHandlers = new List<Delegate>();
            }

            // Append one if it's not included in the existed handlers. 
            if(thisEventHandlers.IndexOf(_handler as Delegate) == -1)
                m_allHandlers[_evtType].Add(_handler);
        }

        public void RemoveEventListener<T>(string _evtType, BaseEventHandler<T> _handler)
            where T : BaseEvent
        {
            List<Delegate> thisEventHandlers;
            if(m_allHandlers != null && m_allHandlers.ContainsKey(_evtType))
            {
                thisEventHandlers = m_allHandlers[_evtType];

                // Remove only when existed. 
                if(thisEventHandlers.IndexOf(_handler as Delegate) != -1)
                {
                    thisEventHandlers.Remove(_handler as Delegate);
                }
            }
        }

        public void RemoveAllListeners()
        {
            if (m_allHandlers != null)
                m_allHandlers.Clear();
        }

        /// <summary>
        /// Automatically select the handler with correct signature.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_evtType"></param>
        /// <param name="_evt"></param>
        public void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent
        {
            if(m_allHandlers != null && m_allHandlers.ContainsKey(_evtType))
            {
                // Copy the handlers in case we add/remove elements when iterating,
                // though Dynamic modifying is not suggested.
                // TODO: May find another way to prevent this
                List<Delegate> thisEventHandlers = new List<Delegate>(
                    m_allHandlers[_evtType]);

                for (int i = 0; i < thisEventHandlers.Count; ++i)
                {
                    //thisEventHandlers[i].DynamicInvoke(this, _evt);
                    if(thisEventHandlers[i] is BaseEventHandler<T>)
                        (thisEventHandlers[i] as BaseEventHandler<T>)(m_dispatcher, _evt);
                }
            }
        }

        #endregion Methods
    }
}