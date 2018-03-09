using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using TasiYokan.Utilities.Serialization;

namespace TasiYokan.Audio
{
    public delegate void AudioCallback();

    public enum AudioLayer
    {
        Undefined = -9999,
        Bgm = -1,
        Dialogue = -2
    }

    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// TODO: remove it to a more static class
        /// </summary>
        /// <returns></returns>
        public static string GetStreamingAssetsRootPath()
        {
            string path =
#if UNITY_ANDROID   // Android
        "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE  // iPhone  
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  // Windows/Web
        Application.streamingAssetsPath;//Application.dataPath;
#else
        string.Empty;  
#endif
            return path;
        }

        private static AudioManager m_instance;
        private Dictionary<string, AudioClip> m_audioDict;
        private List<string> m_allAudioNames;

        private List<AudioPlayer> m_inbuiltAudioPlayers;
        private List<AudioPlayer> m_runtimeAudioPlayers;

        public static AudioManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<AudioManager>();
                    //Assert.IsNotNull(m_instance, "Check if you hava a audio manager in your scene.");
                    if (m_instance == null)
                    {
                        m_instance = GameObject.Instantiate(
                            Resources.Load<GameObject>("AudioManager")).GetComponent<AudioManager>();
                    }
                    m_instance.Init();
                }
                return m_instance;
            }

            set
            {
                m_instance = value;
            }
        }

        public Dictionary<string, AudioClip> AudioDict
        {
            get
            {
                if (m_audioDict == null)
                    m_audioDict = new Dictionary<string, AudioClip>();
                return m_audioDict;
            }
        }

        public List<string> AllAudioNames
        {
            get
            {
                return m_allAudioNames;
            }

            set
            {
                m_allAudioNames = value;
            }
        }

        public List<AudioPlayer> InbuiltAudioPlayers
        {
            get
            {
                if (m_inbuiltAudioPlayers == null)
                    m_inbuiltAudioPlayers = new List<AudioPlayer>();
                return m_inbuiltAudioPlayers;
            }

            set
            {
                m_inbuiltAudioPlayers = value;
            }
        }

        public List<AudioPlayer> RuntimeAudioPlayers
        {
            get
            {
                if (m_runtimeAudioPlayers == null)
                    m_runtimeAudioPlayers = new List<AudioPlayer>();
                return m_runtimeAudioPlayers;
            }

            set
            {
                m_runtimeAudioPlayers = value;
            }
        }

        public String GetAudioRootPath()
        {
            return "AudioTracks/";
        }

        private void Awake()
        {
            GenerateAllAudioList();
        }

        /// <summary>
        /// TODO: We shouldn't do this kind of hard coded init.
        /// </summary>
        private void Init()
        {
            ReadAllAudioList();

            // Bgm
            InitInbuiltAudioSource(AudioLayer.Bgm);
            // Dialogue
            InitInbuiltAudioSource(AudioLayer.Dialogue);
        }

        /// <summary>
        /// Write a json to record all audio tracks we have.
        /// </summary>
        private void GenerateAllAudioList()
        {
            List<string> names = new List<string>();
            string audioRootPath = Application.dataPath + "/Audio/Resources/" + GetAudioRootPath();
            DirectoryInfo rootDirectoryPath = new DirectoryInfo(audioRootPath);
            FileInfo[] fileInfo = rootDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);
            //Debug.Log("full path " + rootDirectoryPath + _prefixName + "_*.*");

            foreach (FileInfo file in fileInfo)
            {
                // file extension check
                if (file.Extension == ".mp3" || file.Extension == ".wav")
                {
                    string nameWithFolder = file.Name.Split('.')[0];
                    string[] fullName = file.FullName.Split('\\');
                    if (fullName[fullName.Length - 2] + '/' != GetAudioRootPath())
                        nameWithFolder = fullName[fullName.Length - 2] + '\\' + nameWithFolder;

                    names.Add(nameWithFolder);
                }
            }

            Debug.Log("Update audio track list. Count: " + names.Count);
            if (Directory.Exists(GetStreamingAssetsRootPath()) == false)
                Directory.CreateDirectory(GetStreamingAssetsRootPath());
            JsonSerializationHelper.WriteJsonList<string>(GetStreamingAssetsRootPath() + "/AudioTrackList.json", names);
        }

        private void ReadAllAudioList()
        {
            //m_allAudioNames = new List<string>();
            m_allAudioNames = JsonSerializationHelper.ReadJsonList<string>(GetStreamingAssetsRootPath() + "/AudioTrackList.json");
            //foreach (var name in m_allAudioNames)
            //{
            //    print("Load audio track: " + name);
            //}
        }

        private AudioPlayer InitInbuiltAudioSource(AudioLayer _layer)
        {
            AudioPlayer player = SpawnAudioPlayer(_layer.ToString());
            InbuiltAudioPlayers.Add(player);
            return player;
        }

        public AudioClip GetAudioClip(string _name)
        {
            if (AudioDict.ContainsKey(_name) == false)
            {
                AudioClip clipToLoad = Resources.Load<AudioClip>(GetAudioRootPath() + _name);
                Assert.IsNotNull(clipToLoad, "Couldn't load " + _name + " from resource");

                AudioDict.Add(_name, clipToLoad);
            }

            return AudioDict[_name];
        }

        public List<AudioClip> GetAudioClips(List<string> _names)
        {
            List<AudioClip> clips = new List<AudioClip>();
            foreach (string name in _names)
            {
                clips.Add(GetAudioClip(name));
            }

            return clips;
        }

        public AudioPlayer GetAudioPlayer(AudioLayer _layer = AudioLayer.Undefined)
        {
            if (_layer == AudioLayer.Undefined)
            {
                return SearchAudioPlayerInPool() ?? SpawnAudioPlayer();
            }
            else if (_layer < 0)
            {
                // Should also start from 0 instead of -1
                return InbuiltAudioPlayers[-(int)_layer - 1];
            }
            else
            {
                return RuntimeAudioPlayers[(int)_layer];
            }
        }

        private AudioPlayer SearchAudioPlayerInPool()
        {
            foreach (var player in RuntimeAudioPlayers)
            {
                if (player.IsBusy == false)
                {
                    return player;
                    // Set it as busy
                }
            }

            return null;
        }

        private AudioPlayer SpawnAudioPlayer(string _playerName = "")
        {
            GameObject playerObj = GameObject.Instantiate(
                Resources.Load<GameObject>("AudioPlayer_Pref"));

            AudioPlayer player = playerObj.GetComponent<AudioPlayer>();
            player.Init();
            playerObj.transform.SetParent(transform);
            if (_playerName == "")
                RuntimeAudioPlayers.Add(player);

            playerObj.name = _playerName != "" ? _playerName : "Runtime Audio Player " + RuntimeAudioPlayers.Count;

            return player;
        }
    }
}