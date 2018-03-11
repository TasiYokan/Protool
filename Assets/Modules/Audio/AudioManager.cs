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

    public enum AudioLayerType
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
        
        private List<AudioLayer> m_inbuiltLayers;
        private List<AudioLayer> m_runtimeLayers;

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

        public List<AudioLayer> InbuiltLayers
        {
            get
            {
                if (m_inbuiltLayers == null)
                    m_inbuiltLayers = new List<AudioLayer>();
                return m_inbuiltLayers;
            }

            set
            {
                m_inbuiltLayers = value;
            }
        }

        public List<AudioLayer> RuntimeLayers
        {
            get
            {
                if (m_runtimeLayers == null)
                    m_runtimeLayers = new List<AudioLayer>();
                return m_runtimeLayers;
            }

            set
            {
                m_runtimeLayers = value;
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
            InitInbuiltLayer(AudioLayerType.Bgm);
            // Dialogue
            InitInbuiltLayer(AudioLayerType.Dialogue);
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
        
        private AudioLayer CreateLayer(string _layerName = "")
        {
            //string layerName = _layerName != "" ? _layerName : "Runtime Layer " + RuntimeLayers.Count;
            GameObject layerObj = new GameObject(_layerName);
            layerObj.transform.SetParent(transform);

            AudioLayer layer = layerObj.AddComponent<AudioLayer>();
            layer.Init();

            //if (_layerName == "")
            //    RuntimeLayers.Add(layer);

            return layer;
        }

        private AudioLayer InitInbuiltLayer(AudioLayerType _layerType)
        {
            AudioLayer layer = CreateLayer(_layerType.ToString());
            InbuiltLayers.Add(layer);

            return layer;
        }

        private AudioLayer InitRuntimeLayer()
        {
            AudioLayer layer = CreateLayer("Runtime Layer " + RuntimeLayers.Count);
            RuntimeLayers.Add(layer);

            return layer;
        }

        public AudioLayer GetLayer(AudioLayerType _layerType = AudioLayerType.Undefined)
        {
            if (_layerType == AudioLayerType.Undefined)
            {
                return InitRuntimeLayer();
            }
            else
            {
                return GetLayer((int)_layerType);
            }
        }

        public AudioLayer GetLayer(int _layerType)
        {
            if (_layerType < 0)
            {
                // Should also start from 0 instead of -1
                return InbuiltLayers[-(int)_layerType - 1];
            }
            else
            {
                return RuntimeLayers[(int)_layerType];
            }
        }
    }
}