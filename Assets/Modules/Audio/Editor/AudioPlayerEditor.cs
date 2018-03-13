using System.Collections;
using System.Collections.Generic;
using TasiYokan.Audio;
using UnityEditor;
using UnityEngine;

namespace TasiYokan.Audio
{
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : Editor
    {
        private AudioPlayer m_target;
        //Serialized Properties
        private SerializedObject serializedObjectTarget;

        public AudioPlayer Target
        {
            get
            {
                if (m_target == null)
                    m_target = (AudioPlayer)target;
                return m_target;
            }
        }

        void OnEnable()
        {
            EditorApplication.update += Update;

            if (Target == null) return;

            SetupEditorVariables();
            GetVariableProperties();
        }

        void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        void SetupEditorVariables()
        {
        }

        void GetVariableProperties()
        {
            serializedObjectTarget = new SerializedObject(Target);
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Self update");
        }

        public override void OnInspectorGUI()
        {
            serializedObjectTarget.Update();
            GUILayout.Space(5);
            GUILayout.Box("Test Text", GUILayout.Width(Screen.width - 20), GUILayout.Height(20));
            GUILayout.Space(5);
            serializedObjectTarget.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
        }
    }
}
