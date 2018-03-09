using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TasiYokan.EventsInUnity;

namespace TasiYokan.Fsm
{
    public class BasicFsmEvent : BaseEvent
    {
        public string strArg;
        public BasicFsmEvent(string _str = "") : base()
        {
            strArg = _str;
        }
    }

    public class FsmHost : SimpleEventDispatcherMono
    {
        private Dictionary<Enum, BaseFsmState> m_states;
        private IEnumerator m_currentCoroutine;
        private BaseFsmState m_state;
        private Enum m_stateEnum;

        public BaseFsmState State
        {
            get
            {
                return m_state;
            }
        }

        public Enum StateEnum
        {
            get
            {
                return m_stateEnum;
            }
        }

        protected void AddState(Enum _stateEnum, BaseFsmState _state)
        {
            if(m_states == null)
                m_states = new Dictionary<Enum, BaseFsmState>();

            m_states.Add(_stateEnum, _state);
        }

        public void EnterState(Enum _stateEnum)
        {
            if(m_currentCoroutine != null)
                this.StopCoroutine(m_currentCoroutine);// All maybe we can terminate all?
            if(m_state != null)
                m_state.OnExit();

            m_stateEnum     = _stateEnum;
            m_state         = m_states[_stateEnum];
            m_currentCoroutine = m_state.OnEnter();
            this.StartCoroutine(m_currentCoroutine);
        }

        public void SignalBasicEvents(string _eventName, string _args = "")
        {
            DispatchEvent<BasicFsmEvent>(_eventName, new BasicFsmEvent());
        }
    }
}
