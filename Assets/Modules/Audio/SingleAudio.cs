using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TasiYokan.Audio
{
    public class SingleAudio : BaseAudio
    {
        internal String m_clipName;

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
                return m_audioPlayer.IsClipPlaying(m_currentClip);
            }
            else if (m_loopTimes <= 0)
            {
                if (m_audioPlayer.IsBeforeEnd())
                {
                    return true;
                }
                else
                {
                    if (onEveryComplete != null)
                        onEveryComplete();

                    return true;
                }
            }
            else
            {
                //if (m_audioPlayer.MainSource.time < m_lastTimeStamp)

                // Will exceed the end after this sample
                if (m_audioPlayer.IsBeforeEnd() == false)
                {
                    m_loopedTimes++;
                    if (onEveryComplete != null)
                        onEveryComplete();
                }

                if (m_loopedTimes >= m_loopTimes)
                {
                    // We will do it after onComplete in BaseAudio
                    //m_audioPlayer.MainSource.Stop();
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
