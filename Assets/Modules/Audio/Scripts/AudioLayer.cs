using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TasiYokan.Audio;

namespace TasiYokan.Audio
{
    public class AudioLayer : MonoBehaviour
    {
        private List<AudioPlayer> m_players;
        private const int maxPlayerInPool = 4;
        //private List<BaseAudio> m_audios;

        public List<AudioPlayer> Players
        {
            get
            {
                if (m_players == null)
                    m_players = new List<AudioPlayer>();
                return m_players;
            }
        }

        //public List<BaseAudio> Audios
        //{
        //    get
        //    {
        //        if (m_audios == null)
        //            m_audios = new List<BaseAudio>();
        //        return m_audios;
        //    }
        //}

        public void Init()
        {
            Players.Add(CreateAudioPlayer());
        }

        private AudioPlayer CreateAudioPlayer()
        {
            AudioPlayer player = gameObject.AddComponent<AudioPlayer>();
            player.Init(this);
            return player;
        }

        public void RemoveAudioPlayer(AudioPlayer _player)
        {
            Players.Remove(_player);
        }

        private AudioPlayer SearchFreeAudioPlayer()
        {
            // Return the first found free audioplayer
            foreach (var player in Players)
            {
                if (player.IsBusy == false)
                {
                    return player;
                    // TODO: Set it as busy?
                }
            }

            AudioPlayer newPlayer = CreateAudioPlayer();
            Players.Add(newPlayer);
            return newPlayer;
        }

        public AudioPlayer SerachPlayerWith(string _clipName)
        {
            foreach (AudioPlayer player in Players)
            {
                if(player.MainSource.clip != null
                    && player.MainSource.clip.name == _clipName)
                {
                    return player;
                }
            }

            return null;
        }

        /// <summary>
        /// Return the audioplayer to play what <see cref="BaseAudio"/> wants to play
        /// </summary>
        /// <param name="_audio"></param>
        /// <returns></returns>
        public AudioPlayer AddAudio(BaseAudio _audio)
        {
            return SearchFreeAudioPlayer();
        }

        public bool CheckFreeAudioPlayerEnough()
        {
            int totalFreeCount = 0;
            foreach (AudioPlayer player in Players)
            {
                if (player.IsBusy == false)
                    totalFreeCount++;

                if (totalFreeCount > maxPlayerInPool)
                    return true;
            }

            return false;
        }
    }
}