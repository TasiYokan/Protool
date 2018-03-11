using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TasiYokan.Audio
{
    public class SingleAudio : BaseAudio
    {
        internal String m_clipName;
        private float m_lastTimeStamp;

        public SingleAudio(string _clipName, AudioLayerType _layer = AudioLayerType.Undefined)
        {
            m_clipName = _clipName;
            m_currentClip = AudioManager.Instance.GetAudioClip(m_clipName);
            m_layer = _layer;
            m_audioPlayer = AudioManager.Instance.GetLayer(_layer).AddAudio(this);
        }

        public SingleAudio(string _clipName, string _layerName)
            : this(_clipName, (AudioLayerType)Enum.Parse(typeof(AudioLayerType), _layerName)) { }
        
        public override bool WaitToComplete()
        {
            // TODO: Move it to base class
            if (m_audioPlayer.IsPaused())
                return true;

            if (m_loopTimes == 1)
            {
                m_lastTimeStamp = m_audioPlayer.MainSource.time;
                return m_audioPlayer.IsClipPlaying(m_currentClip);
            }
            else if (m_loopTimes < 1)
            {
                m_lastTimeStamp = m_audioPlayer.MainSource.time;
                if(m_audioPlayer.IsBeforeEnd())
                {
                    return true;
                }
                else
                {
                    onEveryComplete();
                    
                    return true;
                }
            }
            else
            {
                if (m_audioPlayer.MainSource.time < m_lastTimeStamp)
                {
                    m_loopedTimes++;
                    onEveryComplete();
                }

                m_lastTimeStamp = m_audioPlayer.MainSource.time;
                if (m_loopedTimes >= m_loopTimes)
                {
                    m_audioPlayer.MainSource.Stop();
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected override void FeedAudioPlayer()
        {
            // Its m_currentClip has already been initialized when constructed.

            m_audioPlayer.MainSource.clip = m_currentClip;
        }
    }
}
