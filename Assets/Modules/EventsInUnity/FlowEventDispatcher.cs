using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace TasiYokan.EventsInUnity
{
    public class FlowEventDispatcher : BaseEventDispatcher,
        IFlowEventDispatcher
    {
        #region Fields

        private FlowEventDispatcher m_parent;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// When created by Mono, _host should be the mono itself,
        /// otherwise the _host is the dispatcher itself.
        /// </summary>
        /// <param name="_host"></param>
        public FlowEventDispatcher(FlowEventDispatcherMono _host = null) : base(_host)
        {
        }

        #endregion Constructors

        #region Properties

        // If no mono hosts it then it hosts itself.
        // Thus we need the interface as its type.
        private IFlowEventDispatcher Host
        {
            get
            {
                return base.m_host as IFlowEventDispatcher;
            }
        }

        public FlowEventDispatcher Parent
        {
            get
            {
                return m_parent;
            }

            set
            {
                m_parent = value;
            }
        }

        #endregion Properties

        #region Methods


        /// <summary>
        /// Event's properties like Target, CurrentTarget are all IFlowEventDispatcher(it may be a mono or not)
        /// however the sender is of EventDispatcher, you may use the .Host to find its real host.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_evtType"></param>
        /// <param name="_evt"></param>
        /// <param name="_isOriginal">Indicates if the target is itself</param>
        // TODO: While the modification on input param is not a good practice,
        // it does save some overhead. Later I'll try to find a better way to do that.
        public void DispatchFlowEvent<T>(string _evtType, T _evt, bool _isOriginal = true)
            where T : FlowEvent
        {
            if (_evt.EventPhase == EventPhase.UNDEFINED)
            {
                // Everytime we dispatch an new flow event,
                // its type ought to be the same as what we specified in the signature
                _evt.eventType = _evtType;

                if (_isOriginal || _evt.Target == null)
                    _evt.Target = this.Host;

                // When dispatch begins, try catpture first.
                if (_evt.ForCapture && (Parent != null))
                {
                    _evt.EventPhase = EventPhase.CAPTURING;
                    // Put the capturing before eventHandler so that we can start from the top(root)
                    Parent.DispatchFlowEvent(_evtType, _evt);
                }

                _evt.EventPhase = EventPhase.TARGETED;
            }

            if (_evt.EventPhase == EventPhase.CAPTURING)
            {
                // Capture from top to bottom.
                if (Parent != null)
                    Parent.DispatchFlowEvent(_evtType, _evt);
                // Else has already reached the topest. Go down now.
            }

            _evt.CurrentTarget = this.Host;
            // Call the handler to deal with event.
            EventRegister.DispatchEvent<T>(_evtType, _evt);

            if (_evt.EventPhase == EventPhase.TARGETED)
            {
                if (_evt.ForBubble)
                    _evt.EventPhase = EventPhase.BUBBLING;
                else
                    return;
            }

            if (_evt.EventPhase == EventPhase.BUBBLING)
            {
                // Put the bubbling after eventHandler so that we can start from the bottom(leaf)
                if (Parent != null)
                    Parent.DispatchFlowEvent(_evtType, _evt);
            }
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
            DispatchFlowEvent<T>(_evt.eventType, _evt, false);
        }

        /// <summary>
        /// Enable downward compatibility with SimpleEventDispatcher
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_evtType"></param>
        /// <param name="_evt"></param>
        public void DispatchEvent<T>(string _evtType, T _evt)
            where T : BaseEvent
        {
            // With the help of this line, we can redispatch event
            // without setting the target again in the next dispatcher.
            // See also: _isOriginal in "DispatchFlowEvent"
            if (_evt is FlowEvent)
                (_evt as FlowEvent).Target = Host;

            // Everytime we dispatch an event,
            // its type ought to be the same as what we specified in the signature
            _evt.eventType = _evtType;

            EventRegister.DispatchEvent<T>(_evtType, _evt);
        }

        #endregion Methods
    }
}
