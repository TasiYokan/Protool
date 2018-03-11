using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace TasiYokan.Audio
{
    /// <summary>
    /// A component with one AudioSource Component attached.
    /// <para/>
    /// Provide operations directly on AudioSource While the actual
    /// AudioClip will be decided by certain type of <see cref="BaseAudio"/>.
    /// Other checks on playing status and time elapsed also supported.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        private AudioSource m_mainSource;
        private int m_bufferLength;

        public AudioSource MainSource
        {
            get
            {
                return m_mainSource;
            }

            set
            {
                m_mainSource = value;
            }
        }

        /// <summary>
        /// Whenever AudioSource's clip is not null, 
        /// we assume it is goint to play or under looping
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return MainSource.clip != null;
            }
        }

        public void Init()
        {
            m_mainSource = gameObject.AddComponent<AudioSource>();

            // We need to know the dsp buffer size so we can schedule playback correctly
            // AudioSettings.GetDSPBufferSize is effectively a forwarded call 
            // to FMOD::System::getDSPBufferSize. It defaults to 1024 sample frames on PC.
            int numBuffers;
            AudioSettings.GetDSPBufferSize(out m_bufferLength, out numBuffers);
        }

        public void SetSettings(bool _isLoop)
        {
            MainSource.playOnAwake = false;
            MainSource.loop = _isLoop;
        }

        public void ClearAudioClip()
        {
            MainSource.clip = null;
        }

        public void Play(bool _isForced)
        {
            if (m_mainSource.isPlaying && _isForced == false)
            {
                Debug.Log("Already one in play, skip it");
                return;
            }
            else
            {
                m_mainSource.Play();
            }
        }

        public void PlayDelayed(float _delay, bool _isForced)
        {
            if (m_mainSource.isPlaying && _isForced == false)
            {
                Debug.Log("Already one in play, skip it");
                return;
            }
            else
            {
                m_mainSource.PlayDelayed(_delay);
            }
        }

        public void Stop()
        {
            m_mainSource.Stop();

            // Free player for fututre reuse
            ClearAudioClip();
        }

        public void Pause()
        {
            m_mainSource.Pause();
        }

        public void UnPause()
        {
            m_mainSource.UnPause();
        }

        public bool IsPaused()
        {
            return m_mainSource.isPlaying == false && !IsAtEnd();
        }

        public bool IsClipPlaying(AudioClip _clip)
        {
            return m_mainSource.clip == _clip && m_mainSource.isPlaying; // FIXME: Pause will also be considered as not isPlaying
        }

        public bool IsBeforeEnd()
        {
            //Debug.Log("buffer length " + bufferLength + " num " + numBuffers);
            //Debug.Log("main sample " + m_main.timeSamples + " clip samples " + _clip.samples);

            //return m_main.time < _clip.length - 0.01f;
            return m_mainSource.timeSamples + m_bufferLength < m_mainSource.clip.samples;
        }

        public bool IsAtEnd()
        {
            return m_mainSource.timeSamples == m_mainSource.clip.samples;
        }

        public bool IsOtherSoundPlaying(AudioClip _clip)
        {
            return m_mainSource.isPlaying && m_mainSource.clip != _clip;
        }
    }
}
