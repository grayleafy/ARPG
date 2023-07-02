using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(HFSM))]
public class HFSMEditor : Editor
{
    SerializedProperty stateMachinesProperty;

    private void Awake()
    {
        stateMachinesProperty = serializedObject.FindProperty("stateMachines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 获取对象的所有属性
        SerializedProperty iterator = serializedObject.GetIterator();
        iterator.NextVisible(true);

        // 显示所有属性
        while (iterator.NextVisible(false))
        {
            EditorGUILayout.PropertyField(iterator);
        }


        //EditorGUILayout.PropertyField(stateMachinesProperty, true);

        if (GUILayout.Button("为指定索引的状态机添加状态"))
        {
            GenericMenu menu = new GenericMenu();
            foreach (var enumType in Enum.GetNames(typeof(FSMStateType)))
            {
                menu.AddItem(new GUIContent(enumType), false, AddState, enumType);
            }
            //menu.AddItem(new GUIContent("NormalControlState"), false, AddState, typeof(NormalControlState));
            //menu.AddItem(new GUIContent("StateB"), false, AddState, typeof(NormalControlState));
            // 添加其他子状态类型
            menu.ShowAsContext();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void AddState(object enumType)
    {
        string stateTypeName = enumType + "State";

        // 获取当前选中的StateMachine数组元素的states属性
        SerializedProperty selectedStatesProperty = GetSelectedStateMachineStatesProperty();
        if (selectedStatesProperty == null)
        {
            Debug.LogError("No StateMachine selected");
            return;
        }

        // 添加新的状态到选中的StateMachine
        {
            int newIndex = selectedStatesProperty.arraySize;
            selectedStatesProperty.InsertArrayElementAtIndex(newIndex);
            selectedStatesProperty.GetArrayElementAtIndex(newIndex).managedReferenceValue = Activator.CreateInstance(Type.GetType(stateTypeName));
            selectedStatesProperty.GetArrayElementAtIndex(newIndex).FindPropertyRelative("name").stringValue = (string)enumType;
        }


        //添加状态转换信息
        if (stateMachinesProperty.arraySize > 0)
        {
            int index = serializedObject.FindProperty("indexToModify").intValue;

            var transitionConfig = stateMachinesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("transitionConfigure");

            int newIndex = transitionConfig.arraySize;
            transitionConfig.InsertArrayElementAtIndex(newIndex);
            transitionConfig.GetArrayElementAtIndex(newIndex).managedReferenceValue = Activator.CreateInstance(typeof(TransitionInfo));
            transitionConfig.GetArrayElementAtIndex(newIndex).FindPropertyRelative("state").SetEnumValue((FSMStateType)Enum.Parse(typeof(FSMStateType), (string)enumType));
            transitionConfig.GetArrayElementAtIndex(newIndex).FindPropertyRelative("name").stringValue = (string)enumType;
        }

        // 应用更改
        serializedObject.ApplyModifiedProperties();
    }

    // 获取当前选中的StateMachine数组元素的states属性
    private SerializedProperty GetSelectedStateMachineStatesProperty()
    {
        // 这里可以根据您的需求实现选择StateMachine的逻辑
        // 例如，您可以在HFSM类中添加一个表示当前选中的StateMachine索引的变量
        // 然后在这里使用该变量获取选中的StateMachine的states属性
        // 这里仅为演示目的，返回数组中的第一个StateMachine的states属性
        if (stateMachinesProperty.arraySize > 0)
        {
            int index = serializedObject.FindProperty("indexToModify").intValue;

            return stateMachinesProperty.GetArrayElementAtIndex(index).FindPropertyRelative("states");
        }

        return null;
    }
}
#endif