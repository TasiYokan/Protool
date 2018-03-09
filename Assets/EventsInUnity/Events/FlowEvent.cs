using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TasiYokan.EventsInUnity
{
    public enum EventPhase
    {
        UNDEFINED = 0,
        CAPTURING = 1,
        TARGETED = 2,
        BUBBLING = 3,
    }

    public class FlowEvent : BaseEvent
    {
        public new static readonly FlowEvent Empty = new FlowEvent();

        private readonly bool m_forCapture;
        private readonly bool m_forBubble;

        // We will leave it to be null and init them in the DispatchEvent.
        private IFlowEventDispatcher m_target = null;
        // Seems it can be get from sender in each dispatch function.
        private IFlowEventDispatcher m_currentTarget = null;
        private EventPhase m_eventPhase;

        public bool ForCapture
        {
            get
            {
                return m_forCapture;
            }
        }

        public bool ForBubble
        {
            get
            {
                return m_forBubble;
            }
        }

        public EventPhase EventPhase
        {
            get
            {
                return m_eventPhase;
            }

            set
            {
                m_eventPhase = value;
            }
        }

        /// <summary>
        /// Actually target is the midpoint of the whole flow.
        /// It's the source of an event.
        /// </summary>
        public IFlowEventDispatcher Target
        {
            get
            {
                return m_target;
            }

            set
            {
                m_target = value;
            }
        }

        public IFlowEventDispatcher CurrentTarget
        {
            get
            {
                return m_currentTarget;
            }

            set
            {
                m_currentTarget = value;
            }
        }

        public FlowEvent(bool _capture = false, bool _bubble = true)
        {
            m_forCapture = _capture;
            m_forBubble = _bubble;
            EventPhase = EventPhase.UNDEFINED;
        }
    }
}
