using System;
using System.Collections;
using System.Collections.Generic;

namespace TasiYokan.Fsm
{
    public abstract class BaseFsmState
    {
        private FsmHost m_host;

        public FsmHost Host
        {
            get
            {
                return m_host;
            }
        }

        public BaseFsmState(FsmHost _FsmHost)
        {
            m_host = _FsmHost;
        }

        public abstract IEnumerator OnEnter();

        // HACK: To stop animation or reset and so on.
        public virtual void OnExit() { }
    }
}
