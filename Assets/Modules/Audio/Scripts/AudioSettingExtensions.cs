using System;
using System.Collections.Generic;
using UnityEngine;

namespace TasiYokan.Audio
{
    public static class AudioSettingExtensions
    {
        public static T OnStart<T>(this T _container, AudioCallback _onStart) where T : BaseAudio
        {
            if (_container == null)
                return _container;

            _container.onStart = _onStart;

            return _container;
        }

        public static T OnComplete<T>(this T _container, AudioCallback _onComplete) where T : BaseAudio
        {
            if (_container == null)
                return _container;

            _container.onComplete = _onComplete;

            return _container;
        }

        public static T OnEveryComplete<T>(this T _container, AudioCallback _onEveryComplete) where T : BaseAudio
        {
            if (_container == null)
                return _container;

            _container.onEveryComplete = _onEveryComplete;

            return _container;
        }

        public static T SetDelay<T>(this T _container, float _delay) where T : BaseAudio
        {
            _container.m_delay = _delay;

            return _container;
        }

        public static T SetAudioSource<T>(this T _container, AudioPlayer _audioSource) where T : BaseAudio
        {
            _container.m_audioPlayer = _audioSource;

            return _container;
        }

        public static T SetLoop<T>(this T _container, int _loopTimes) where T : BaseAudio
        {
            _container.m_loopTimes = _loopTimes;

            return _container;
        }

        public static T SetForce<T>(this T _container, bool _isForce) where T : BaseAudio
        {
            _container.m_isForced = _isForce;
            return _container;
        }

        public static T SetVolume<T>(this T _container, float _volume) where T : BaseAudio
        {
            _container.m_volume = _volume;
            _container.AudioPlayer.MainSource.volume = _volume;
            return _container;
        }

        /// <summary>
        /// Store current audioSource to <see cref="AudioPlayer.SecondSource"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_container"></param>
        /// <param name="_isForce"></param>
        /// <returns></returns>
        public static T SetCrossFade<T>(this T _container, bool _isCorssFade = true) where T : BaseAudio
        {
            if (_isCorssFade)
                _container.AudioPlayer.SetSecondSource(_container.AudioPlayer.MainSource);
            return _container;
        }
    }
}
