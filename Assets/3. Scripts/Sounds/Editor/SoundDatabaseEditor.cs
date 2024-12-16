using System;
using _3._Scripts.Sounds.Scriptables;
using UnityEditor;
using UnityEngine;

namespace _3._Scripts.Sounds.Editor
{
    [CustomEditor(typeof(SoundDatabase))]
    public class SoundDatabaseEditor : UnityEditor.Editor
    {
        private SerializedProperty soundsProperty;
        private bool[] foldouts; // Массив для хранения состояния сворачивания для каждого элемента

        private void OnEnable()
        {
            soundsProperty = serializedObject.FindProperty("sounds");
            foldouts = new bool[soundsProperty.arraySize]; // Инициализация массива сворачивания
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sound Database", EditorStyles.boldLabel);

            if (soundsProperty.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No sounds in the database. Add some sounds.", MessageType.Info);
            }

            // Add new sound button (new element is added at the start)
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Sound"))
            {
                soundsProperty.InsertArrayElementAtIndex(0); // Add new element at the start
                Array.Resize(ref foldouts, soundsProperty.arraySize); // Resize foldouts array
                foldouts[0] = true; // Set the new element to be expanded by default
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical("box");
            for (int i = 0; i < soundsProperty.arraySize; i++)
            {
                var sound = soundsProperty.GetArrayElementAtIndex(i);
                var idProperty = sound.FindPropertyRelative("id");
                var clipsProperty = sound.FindPropertyRelative("audioClips");
                var volumeProperty = sound.FindPropertyRelative("volume");

                // Add Foldout for each item
                EditorGUILayout.BeginHorizontal();
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], $"Sound: {idProperty.stringValue}");
                if (GUILayout.Button("Remove"))
                {
                    soundsProperty.DeleteArrayElementAtIndex(i);
                    soundsProperty.DeleteArrayElementAtIndex(i); // Ensure that the element is fully removed
                    Array.Resize(ref foldouts, soundsProperty.arraySize); // Re-resize foldouts array after deletion
                    break; // Break out of the loop to avoid errors in array resizing
                }

                EditorGUILayout.EndHorizontal();

                if (foldouts[i])
                {
                    // Show sound properties
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    EditorGUILayout.PropertyField(idProperty, new GUIContent("Sound ID"));
                    EditorGUILayout.PropertyField(volumeProperty, new GUIContent("Volume"));

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Clips", EditorStyles.boldLabel);
                    if (GUILayout.Button("Add Clip"))
                    {
                        clipsProperty.arraySize++;
                    }

                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;

                    if (clipsProperty.arraySize == 0)
                    {
                        EditorGUILayout.HelpBox("No clips added. Click 'Add Clip' to add a new clip.",
                            MessageType.Info);
                    }

                    for (int j = 0; j < clipsProperty.arraySize; j++)
                    {
                        var clipElement = clipsProperty.GetArrayElementAtIndex(j);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(clipElement, GUIContent.none);

                        // Remove clip button
                        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(18)))
                        {
                            clipsProperty.DeleteArrayElementAtIndex(j);
                            break; // Break out of the loop to avoid errors due to index change
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel--;

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.Space();
            }

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}