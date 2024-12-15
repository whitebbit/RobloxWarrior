using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloatingJoystick))]
public class FloatingJoystickEditor : JoystickEditor
{
    private SerializedProperty leftUp;
    private SerializedProperty leftDown;
    private SerializedProperty rightUp;
    private SerializedProperty rightDown;
    protected override void OnEnable()
    {
        base.OnEnable();
        
        leftUp = serializedObject.FindProperty("leftUp");
        leftDown = serializedObject.FindProperty("leftDown");
        rightUp = serializedObject.FindProperty("rightUp");
        rightDown = serializedObject.FindProperty("rightDown");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.ObjectField(leftUp, new GUIContent("Left Up"));
        EditorGUILayout.ObjectField(leftDown, new GUIContent("Left Down"));
        EditorGUILayout.ObjectField(rightUp, new GUIContent("Right Up"));
        EditorGUILayout.ObjectField(rightDown, new GUIContent("Right Down"));
        
        serializedObject.ApplyModifiedProperties();

        if (background != null)
        {
            RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
            backgroundRect.anchorMax = Vector2.zero;
            backgroundRect.anchorMin = Vector2.zero;
            backgroundRect.pivot = center;
        }
    }
}