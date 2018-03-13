using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace TasiYokan.Audio
{
    public class RandomAudio : BaseAudio
    {
        internal List<string> m_clipNames;
        internal List<AudioClip> m_clips;
        //private int m_lastSampleStamp;

        public RandomAudio(string _prefixName, AudioLayerType _layer = AudioLayerType.Undefined)
        {
            m_clipNames = GetAllAvailableClipNames(_prefixName);
            Assert.IsTrue(m_clipNames.Count > 0, "Could find any sounds with prefixName: " + _prefixName);
            m_clips = AudioManager.Instance.GetAudioClips(m_clipNames);
            m_layer = _layer;
            m_audioPlayer = AudioManager.Instance.GetLayer(_layer).AddAudio(this);
        }

        private List<string> GetAllAvailableClipNames(string _prefixName)
        {
            return AudioManager.Instance.AllAudioNames.FindAll(
                (string s) =>
                {
                    string[] names = s.Split('\\');
                    return names[names.Length - 1].StartsWith(_prefixName);
                });
        }

        public override bool WaitToComplete()
        {
            if (m_audioPlayer.IsPaused())
                return true;

            //m_lastSampleStamp = m_audioPlayer.MainSource.timeSamples;

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
                    //Debug.Log("Finish one loop from " + m_currentClip.name);

                    // If we want let it call the onComplete() only once which will nullify AudioClip on player after continuing coroutine
                    //return false;

                    if (onEveryComplete != null)
                        onEveryComplete();

                    //TODO: Find a way to move it into BaseAudio
                    FeedAudioPlayer();
                    Debug.Log("Randomly pick another one " + m_currentClip.name);
                    m_audioPlayer.MainSource.Play();
                    return true;
                }
            }
            else
            {
                // We assume when timesample Has back from n to 0 denotes it rewinds to the head
                //if (m_audioPlayer.MainSource.timeSamples < m_lastSampleStamp)

                // Will exceed the end after this sample
                if (m_audioPlayer.IsBeforeEnd() == false)
                {
                    m_loopedTimes++;
                    if (onEveryComplete != null)
                        onEveryComplete();

                    if (m_loopedTimes < m_loopTimes)
                    {
                        FeedAudioPlayer();
                        Debug.Log("Randomly pick another one " + m_currentClip.name);
                        m_audioPlayer.MainSource.Play();
                    }
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
            m_currentClip = m_clips[UnityEngine.Random.Range(0, m_clips.Count)];
            m_audioPlayer.MainSource.clip = m_currentClip;
        }
    }
}