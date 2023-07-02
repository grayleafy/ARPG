

using UnityEditor;
using UnityEngine;
using System;

#if UNITY_EDITOR

[CustomEditor(typeof(FSM))]
public class StateMachineEditor : Editor
{
    SerializedProperty statesProperty;

    private void OnEnable()
    {
        statesProperty = serializedObject.FindProperty("states");
    }

    public override void OnInspectorGUI()
    {
        Debug.Log("run");
        serializedObject.Update();
        EditorGUILayout.PropertyField(statesProperty, true);

        if (GUILayout.Button("Add State"))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("NormalControl"), false, AddState, typeof(NormalControlState));
            menu.AddItem(new GUIContent("StateB"), false, AddState, typeof(NormalControlState));
            // 添加其他子状态类型
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddState(object stateType)
    {
        int newIndex = statesProperty.arraySize;
        statesProperty.InsertArrayElementAtIndex(newIndex);
        statesProperty.GetArrayElementAtIndex(newIndex).managedReferenceValue = Activator.CreateInstance((Type)stateType);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
