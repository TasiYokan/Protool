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
            // Only update when in play mode
            //if (EditorApplication.isPlaying)
                this.Repaint();
        }

        public override void OnInspectorGUI()
        {
            m_serializedTarget.Update();
            GUILayout.Space(5);
            //GUILayout.Box("Test Text", GUILayout.Width(Screen.width - 20), GUILayout.Height(20));
            DrawBasicSettings();
            GUILayout.Space(5);
            m_serializedTarget.ApplyModifiedProperties();
        }

        void OnSceneGUI()
        {
        }

        void DrawBasicSettings()
        {
            GUILayout.BeginVertical();
            if (m_mainSource.objectReferenceValue != null)
            {
                GUIStyle style = new GUIStyle(EditorStyles.helpBox);
                style.fixedWidth = 90;

                AudioSource mainSource = ((AudioSource)m_mainSource.objectReferenceValue);
                //EditorGUILayout.ObjectField(mainSource.clip, typeof(AudioClip));
                if (mainSource.clip)
                {
                    ProgressBar(mainSource.time / mainSource.clip.length, mainSource.clip.name);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("Volume: {0: 00%}", mainSource.volume), style);
                    GUILayout.HorizontalSlider(mainSource.volume, 0, 1);
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);

                AudioSource secondSource = ((AudioSource)m_secondSource.objectReferenceValue);
                //EditorGUILayout.ObjectField(secondSource.clip, typeof(AudioClip));
                if (secondSource.clip)
                {
                    ProgressBar(secondSource.time / secondSource.clip.length, secondSource.clip.name);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label(string.Format("Volume: {0: 00%}", secondSource.volume), style);
                    GUILayout.HorizontalSlider(secondSource.volume, 0, 1);
                    GUILayout.EndHorizontal();
                }
                GUI.enabled = true;
            }
            GUILayout.EndVertical();
        }

        void ProgressBar(float value, string label)
        {
            // Get a rect for the progress bar using the same margins as a textfield:
            Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rect, value, 
                string.Format("{0}: {1: 00%}",label, value));
            EditorGUILayout.Space();
        }
    }
}
