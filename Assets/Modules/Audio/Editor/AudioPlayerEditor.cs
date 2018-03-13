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
        private SerializedObject m_serializedTarget;
        private SerializedProperty m_mainSource;
        private SerializedProperty m_secondSource;

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
            if (Target.MainSource != null)
                Target.MainSource.hideFlags = HideFlags.HideInInspector;
            if (Target.SecondSource != null)
                Target.SecondSource.hideFlags = HideFlags.HideInInspector;
        }

        void GetVariableProperties()
        {
            m_serializedTarget = new SerializedObject(Target);
            m_mainSource = m_serializedTarget.FindProperty("m_mainSource");
            m_secondSource = m_serializedTarget.FindProperty("m_secondSource");
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Self update");
        }

        public override void OnInspectorGUI()
        {
            m_serializedTarget.Update();
            GUILayout.Space(5);
            GUILayout.Box("Test Text", GUILayout.Width(Screen.width - 20), GUILayout.Height(20));
            DrawBasicSettings();
            GUILayout.Space(5);
            m_serializedTarget.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
        }

        void DrawBasicSettings()
        {
            GUILayout.BeginHorizontal();
            if (m_mainSource.objectReferenceValue != null)
            {
                EditorGUILayout.ObjectField(
                    ((AudioSource)m_mainSource.objectReferenceValue).clip, 
                    typeof(AudioClip));
                EditorGUILayout.ObjectField(
                    ((AudioSource)m_secondSource.objectReferenceValue).clip,
                    typeof(AudioClip));
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();
        }
    }
}
