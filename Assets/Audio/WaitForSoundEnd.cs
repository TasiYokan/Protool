using System;
using System.Collections.Generic;
using UnityEngine;

namespace TasiYokan.Audio
{
    public class WaitForAudioEnd : CustomYieldInstruction
    {
        private BaseAudio m_container;

        public WaitForAudioEnd(BaseAudio _container)
        {
            m_container = _container;
        }

        public override bool keepWaiting
        {
            get
            {
                //Debug.Log("is complete " + m_container.IsCompleted());
                //Debug.Log("time " + m_audioSource.time);
                //Debug.Log("sample " + m_audioSource.timeSamples);
                return m_container.WaitToComplete();
            }
        }
    }
}
