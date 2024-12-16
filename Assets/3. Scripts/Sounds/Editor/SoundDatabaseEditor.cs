using _3._Scripts.Sounds.Scriptables;
using UnityEditor;
using UnityEngine;

namespace _3._Scripts.Sounds.Editor
{
    [CustomEditor(typeof(SoundDatabase))]
    public class SoundDatabaseEditor : UnityEditor.Editor
    {
        private SerializedProperty soundsProperty;

        private void OnEnable()
        {
            soundsProperty = serializedObject.FindProperty("_sounds");
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
// Add new sound button
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Sound"))
            {
                soundsProperty.arraySize++;
            }
            EditorGUILayout.EndHorizontal();
            for (int i = 0; i < soundsProperty.arraySize; i++)
            {
                var sound = soundsProperty.GetArrayElementAtIndex(i);
                var idProperty = sound.FindPropertyRelative("id");
                var clipsProperty = sound.FindPropertyRelative("audioClips");
                var volumeProperty = sound.FindPropertyRelative("volume");

                // Start a vertical group for each sound
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(idProperty, new GUIContent("Sound ID"));
                EditorGUILayout.PropertyField(volumeProperty, new GUIContent("Volume"));
                
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Audio Clips", EditorStyles.boldLabel);
                if (GUILayout.Button("Add Clip"))
                {
                    clipsProperty.arraySize++;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                if (clipsProperty.arraySize == 0)
                {
                    EditorGUILayout.HelpBox("No clips added. Click 'Add Clip' to add a new clip.", MessageType.Info);
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
                        break; // Exit the loop early to prevent issues with index shifting
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.BeginHorizontal();
               
                if (GUILayout.Button("Remove Sound"))
                {
                    soundsProperty.DeleteArrayElementAtIndex(i);
                    break; // Exit early to avoid changing index during iteration
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            

            serializedObject.ApplyModifiedProperties();
        }
    }
}
