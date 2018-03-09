using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace TasiYokan.Utilities.Serialization
{
    public class JsonSerializationHelper
    {
        /// <summary>
        /// Using the StreamWriter write the json file within utf-8 directly. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_path"></param>
        /// <param name="_obj"> </param>
        public static void WriteJson<T>(string _path, T _obj)
        {
            // Set the 2rd params to false if you want to save the size of json file.
            string jsonContent = JsonUtility.ToJson(_obj, true);
            File.WriteAllText(_path, jsonContent, Encoding.UTF8);
        }

        /// <summary>
        /// Using memory stream to boost the loading of Xml. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static T ReadJson<T>(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.Log("No file in " + _path);
                return default(T);// <- return null if it's a reference tyep.
            }

            using (StreamReader sr = new StreamReader(_path))
            {
                string jsonContent = sr.ReadToEnd();
                if (jsonContent.Length > 0)
                    return JsonUtility.FromJson<T>(jsonContent);
                else
                    return default(T);
            }
        }

        public static void WriteJsonArray<T>(string _path, T[] _objs)
        {
            // Set the 2rd params to false if you want to save the size of json file.
            string jsonContent = ToJsonArray(_objs);
            File.WriteAllText(_path, jsonContent, Encoding.UTF8);
        }

        public static T[] ReadJsonArray<T>(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.Log("No file in " + _path);
                return default(T[]);// <- return null if it's a reference tyep.
            }

            using (StreamReader sr = new StreamReader(_path))
            {
                string jsonContent = sr.ReadToEnd();
                if (jsonContent.Length > 0)
                    return FromJsonArray<T>(jsonContent);
                else
                    return default(T[]);
            }
        }

        public static void WriteJsonList<T>(string _path, List<T> _objs)
        {
            // Set the 2rd params to false if you want to save the size of json file.
            string jsonContent = ToJsonList(_objs);
            File.WriteAllText(_path, jsonContent, Encoding.UTF8);
        }

        public static List<T> ReadJsonList<T>(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.Log("No file in " + _path);
                return default(List<T>);// <- return null if it's a reference tyep.
            }

            using (StreamReader sr = new StreamReader(_path))
            {
                string jsonContent = sr.ReadToEnd();
                if (jsonContent.Length > 0)
                    return FromJsonList<T>(jsonContent);
                else
                    return default(List<T>);
            }
        }

        [Serializable]
        private class ArrayWrapper<T>
        {
            public T[] Array;
        }

        public static T[] FromJsonArray<T>(string _rawJsonContent)
        {
            // Once you use ToJsonArray to serialize, you dont need to append the prefix yourself.
            //string jsonContent = "{ \"Array\": " + _rawJsonContent + "}";
            ArrayWrapper<T> wrapper = JsonUtility.FromJson<ArrayWrapper<T>>(_rawJsonContent);
            return wrapper.Array;
        }

        public static string ToJsonArray<T>(T[] _array)
        {
            ArrayWrapper<T> wrapper = new ArrayWrapper<T>();
            wrapper.Array = _array;
            return JsonUtility.ToJson(wrapper, true);
        }

        [Serializable]
        private class ListWrapper<T>
        {
            public List<T> List;
        }

        public static List<T> FromJsonList<T>(string _rawJsonContent)
        {
            // Once you use ToJsonArray to serialize, you dont need to append the prefix yourself.
            //string jsonContent = "{ \"List\": " + _rawJsonContent + "}";
            ListWrapper<T> wrapper = JsonUtility.FromJson<ListWrapper<T>>(_rawJsonContent);
            return wrapper.List;
        }

        public static string ToJsonList<T>(List<T> _list)
        {
            ListWrapper<T> wrapper = new ListWrapper<T>();
            wrapper.List = _list;
            return JsonUtility.ToJson(wrapper, true);
        }
    }

    public class XmlSerializationHelper
    {
        /// <summary>
        /// Using the StreamWriter write the Xml file within utf-8 directly.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_path"></param>
        /// <param name="_obj"></param>
        public static void WriteXml<T>(string _path, T _obj)
        {
            // TODO: Maybe we need check if the type is serializable?
            // Create the xml parser for specific class.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter tw = new StreamWriter(_path, false, Encoding.UTF8))
            {
                serializer.Serialize(tw, _obj);
            }
        }

        /// <summary>
        /// Using memory stream to boost the loading of Xml.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static T ReadXml<T>(string _path)
        {
            if (!File.Exists(_path))
            {
                Debug.Log("No file in " + _path);
                return default(T);// <- return null if it's a reference tyep.
            }
            // Create the xml parser for specific class.
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // All the xml nodes are mapped into string.
            string xmlContent = File.ReadAllText(_path);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent)))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}