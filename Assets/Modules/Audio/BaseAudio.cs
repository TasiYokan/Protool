using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TasiYokan.Audio
{
    /// <summary>
    /// Base class that consists of two parts:
    /// <para>
    /// 1) AudioPlayer which is basically a gameobject with AudioSource Component attached.
    /// We use it to directly perform operations(Play, Stop...) on <see cref="AudioClip"/>.
    /// </para>
    /// <para>
    /// 2) Clips feed audioplayer. Especially useful to <see cref="RandomAudio"/>
    /// </para>
    /// </summary>
    public abstract class BaseAudio : IContainer
    {
        public AudioCallback onStart;
        public AudioCallback onComplete;
        /// <summary>
        /// Called everytime finished a loop
        /// </summary>
        public AudioCallback onEveryComplete;
        protected internal AudioPlayer m_audioPlayer;
        protected internal AudioClip m_currentClip;
        protected internal float m_delay = 0;
        protected internal AudioLayerType m_layer = AudioLayerType.Bgm;
        protected internal int m_loopTimes = 1;
        protected internal bool m_isForced = true;
        protected internal int m_loopedTimes = 0;
        private Coroutine m_currentFading;

        public AudioPlayer AudioPlayer
        {
            get
            {
                return m_audioPlayer;
            }
        }

        public abstract bool WaitToComplete();
        protected abstract void FeedAudioPlayer();
        // TODO: Not tidy!
        public void FeedNewAudioPlayer(AudioClip _newClip)
        {
            m_currentClip = _newClip;
            m_audioPlayer.MainSource.clip = _newClip;
            // We shouldn't play immediately
            m_audioPlayer.MainSource.Play();
        }

        /// <summary>
        /// Set up audioplayer settings based on initialization.
        /// Then start PlaySound coroutine to execute <see cref="onStart"/>/<see cref="onComplete"/>
        /// </summary>
        public virtual void Play()
        {
            FeedAudioPlayer();

            if (m_audioPlayer.IsOtherSoundPlaying(m_currentClip) && m_isForced == false)
            {
                Debug.Log("Play " + m_currentClip.name + " has been rejected");
                return;
            }

            AudioManager.Instance.StartCoroutine(
                Playing(m_loopTimes < 0 || m_loopTimes > 1));
        }

        public virtual void Stop()
        {
            if (m_audioPlayer.MainSource.clip == m_currentClip)
                m_audioPlayer.Stop();
        }

        // TODO: Check if it's still under delay?
        public virtual void Pause()
        {
            if (m_audioPlayer.MainSource.clip == m_currentClip)
                m_audioPlayer.Pause();
        }

        public virtual void Unpause()
        {
            if (m_audioPlayer.MainSource.clip == m_currentClip)
                m_audioPlayer.UnPause();
        }

        protected virtual IEnumerator Playing(bool _isLoop)
        {
            m_audioPlayer.SetSettings(_isLoop);

            // Debug.Log("PlaySound " + m_clipName);
            m_loopedTimes = 0;

            if (m_delay > 0)
                m_audioPlayer.PlayDelayed(m_delay, m_isForced);
            else
                m_audioPlayer.Play(m_isForced);

            yield return new WaitForAudioStart(this);
            // Invoke onStart only when it actually start playing
            if (onStart != null)
                onStart();

            yield return new WaitForAudioEnd(this);

            if (onComplete != null)
                onComplete();

            // If not loop, clear the audio track
            if (m_loopTimes >= 0)
                m_audioPlayer.ClearAudioClip();
        }

        public void Fade(float _startVol, float _endVol, float _duration)
        {
            // Can only have one fading at one time
            if (m_currentFading != null)
                AudioManager.Instance.StopCoroutine(m_currentFading);

            m_currentFading =
                AudioManager.Instance.StartCoroutine(Fading(_startVol, _endVol, _duration));
        }

        private IEnumerator Fading(float _startVol, float _endVol, float _duration)
        {
            if (m_audioPlayer.MainSource.clip == null)
            {
                yield break;
            }

            _startVol = Mathf.Clamp01(_startVol);
            _endVol = Mathf.Clamp01(_endVol);
            float startTime = Time.time;

            m_audioPlayer.MainSource.volume = _startVol;

            while (m_audioPlayer.MainSource.clip != null
                && Time.time < startTime + _duration)
            {
                float t = (Time.time - startTime) / _duration;
                m_audioPlayer.MainSource.volume = Mathf.Lerp(_startVol, _endVol, t);

                yield return null;
            }

            // Incase it has been stopped before end of fading
            if (m_audioPlayer.MainSource.clip != null)
                m_audioPlayer.MainSource.volume = _endVol;
        }

        public void CrossFade(float _startVol, float _endVol, float _duration)
        {
            // Can only have one fading at one time
            if (m_currentFading != null)
                AudioManager.Instance.StopCoroutine(m_currentFading);

            m_currentFading =
                AudioManager.Instance.StartCoroutine(CrossFading(_startVol, _endVol, _duration));
        }

        private IEnumerator CrossFading(float _startVol, float _endVol, float _duration)
        {
            if (m_audioPlayer.MainSource.clip == null)
            {
                yield break;
            }

            _startVol = Mathf.Clamp01(_startVol);
            _endVol = Mathf.Clamp01(_endVol);
            float startTime = Time.time;

            m_audioPlayer.MainSource.volume = _startVol;
            m_audioPlayer.SecondSource.volume = 1 - m_audioPlayer.MainSource.volume;

            while (m_audioPlayer.MainSource.clip != null
                && Time.time < startTime + _duration)
            {
                float t = (Time.time - startTime) / _duration;
                m_audioPlayer.MainSource.volume = Mathf.Lerp(_startVol, _endVol, t);
                m_audioPlayer.SecondSource.volume = 1 - m_audioPlayer.MainSource.volume;

               yield return null;
            }

            // Incase it has been stopped before end of fading
            if (m_audioPlayer.MainSource.clip != null)
            {
                m_audioPlayer.MainSource.volume = _endVol;
                m_audioPlayer.SecondSource.volume = 1 - m_audioPlayer.MainSource.volume;
            }

            m_audioPlayer.ClearSecondAudioClip();
        }
    }
}
