﻿using System;
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
        private AudioLayer m_layer;
        [SerializeField]
        private AudioSource m_mainSource;
        /// <summary>
        /// A temp place to store extra track
        /// </summary>
        [SerializeField]
        private AudioSource m_secondSource;
        private static int m_bufferLength;

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

        public AudioSource SecondSource
        {
            get
            {
                return m_secondSource;
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

        public bool IsPlaying
        {
            get
            {
                return MainSource.isPlaying;
            }
        }

        public static int BufferLength
        {
            get
            {
                if (m_bufferLength == 0)
                {
                    // We need to know the dsp buffer size so we can schedule playback correctly
                    // AudioSettings.GetDSPBufferSize is effectively a forwarded call 
                    // to FMOD::System::getDSPBufferSize. It defaults to 1024 sample frames on PC.
                    int numBuffers;
                    AudioSettings.GetDSPBufferSize(out m_bufferLength, out numBuffers);
                }
                return m_bufferLength;
            }
        }

        public void Init(AudioLayer _layer)
        {
            m_mainSource = gameObject.AddComponent<AudioSource>();
            m_secondSource = gameObject.AddComponent<AudioSource>();
            m_layer = _layer;
        }

        public void SetSettings(bool _isLoop)
        {
            MainSource.playOnAwake = false;
            MainSource.loop = _isLoop;
        }

        public void ClearAudioClip()
        {
            MainSource.clip = null;
            
            if (m_layer.CheckFreeAudioPlayerEnough())
            {
                print("Remove audio player");
                m_layer.RemoveAudioPlayer(this);
                Destroy(m_mainSource);
                Destroy(m_secondSource);
                Destroy(this);
            }
        }

        public void SetSecondSource(AudioSource _source)
        {
            m_secondSource.clip = _source.clip;
            m_secondSource.playOnAwake = _source.playOnAwake;
            m_secondSource.loop = _source.loop;
            m_secondSource.timeSamples = _source.timeSamples;
            m_secondSource.Play();
        }

        public void ClearSecondAudioClip()
        {
            m_secondSource.clip = null;                      
        }

        public void Play(bool _isForced)
        {
            if (IsPlaying && _isForced == false)
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
            if (IsPlaying && _isForced == false)
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

        public void SetVolume(float _volume, int _sourceId = 0)
        {
            if (_sourceId == 0)
                m_mainSource.volume = _volume;
            else
                m_secondSource.volume = _volume;
        }

        public float GetVolume(int _sourceId = 0)
        {
            if (_sourceId == 0)
                return m_mainSource.volume;
            else
                return m_secondSource.volume;
        }

        public bool IsPaused()
        {
            return IsPlaying == false && !IsAtEnd();
        }

        public bool IsClipPlaying(AudioClip _clip)
        {
            return m_mainSource.clip == _clip && IsPlaying; // FIXME: Pause will also be considered as not isPlaying
        }

        public bool IsBeforeEnd()
        {
            //Debug.Log("buffer length " + bufferLength + " num " + numBuffers);
            //Debug.Log("main sample " + m_main.timeSamples + " clip samples " + _clip.samples);

            //return m_main.time < _clip.length - 0.01f;
            return m_mainSource.timeSamples + BufferLength < m_mainSource.clip.samples;
        }

        public bool IsAtEnd()
        {
            return m_mainSource.timeSamples == m_mainSource.clip.samples;
        }

        public bool IsOtherSoundPlaying(AudioClip _clip)
        {
            return IsPlaying && m_mainSource.clip != _clip;
        }
    }
}
