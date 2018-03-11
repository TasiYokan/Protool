using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TasiYokan.Audio;

public class AudioLayer : MonoBehaviour
{
    private List<AudioPlayer> m_players;

    public List<AudioPlayer> Players
    {
        get
        {
            if (m_players == null)
                m_players = new List<AudioPlayer>();
            return m_players;
        }

        set
        {
            m_players = value;
        }
    }

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
}
