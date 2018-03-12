using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TasiYokan.Audio;

public class AudioLayer : MonoBehaviour
{
    private List<AudioPlayer> m_players;
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
        player.Init();
        return player;
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

        return CreateAudioPlayer();
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
}
