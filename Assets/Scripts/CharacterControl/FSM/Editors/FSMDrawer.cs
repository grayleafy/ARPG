//using System;
//using UnityEditor;
//using UnityEditor.UIElements;
//using UnityEngine;
//using UnityEngine.UIElements;

//[CustomPropertyDrawer(typeof(FSM))]
//public class FSMDrawer : PropertyDrawer
//{
//    public override VisualElement CreatePropertyGUI(SerializedProperty property)
//    {
//        // Create property container element.
//        var container = new VisualElement();

//        // Create property fields.
//        var amountField = new PropertyField(property);


//        // Add fields to the container.
//        container.Add(amountField);


//        return container;
//    }
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        //EditorGUI.BeginProperty(position, label, property);
//        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
//        //var rect = new Rect(position.x + 30, position.y + 30, 30, 140);
//        // 绘制默认的StateMachine属性
//        //EditorGUI.PropertyField(position, property, label, true);
//        //EditorGUILayout.PropertyField(property, true);

//        // 绘制"Add State"按钮
//        //Rect buttonRect = new Rect(position.x + position.width - 100, position.y, 100, EditorGUIUtility.singleLineHeight);
//        //if (GUI.Button(buttonRect, "Add State"))
//        //{
//        //    GenericMenu menu = new GenericMenu();
//        //    menu.AddItem(new GUIContent("StateA"), false, AddState, new AddStateContext { property = property, stateType = typeof(NormalControlState) });
//        //    menu.AddItem(new GUIContent("StateB"), false, AddState, new AddStateContext { property = property, stateType = typeof(NormalControlState) });
//        //    // 添加其他子状态类型
//        //    menu.ShowAsContext();
//        //}

//        //GUILayout.Label("MyClass Properties:");
//        SerializedProperty intProp = property.FindPropertyRelative("name");
//        EditorGUILayout.PropertyField(intProp);
//        //SerializedProperty floatProp = property.FindPropertyRelative("myFloat");
//        //EditorGUILayout.PropertyField(floatProp);

//        // 结束GUILayout
//        //GUILayout.EndVertical();


//        //EditorGUI.EndProperty();
//    }

//    //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    //{
//    //    return 100;
//    //    return base.GetPropertyHeight(property, label);
//    //}

//    private void AddState(object context)
//    {
//        AddStateContext addStateContext = (AddStateContext)context;

//        // 获取states属性
//        SerializedProperty statesProperty = addStateContext.property.FindPropertyRelative("states");

//        // 添加新的状态到StateMachine
//        int newIndex = statesProperty.arraySize;
//        statesProperty.InsertArrayElementAtIndex(newIndex);
//        statesProperty.GetArrayElementAtIndex(newIndex).managedReferenceValue = Activator.CreateInstance(addStateContext.stateType);

//        // 应用更改
//        addStateContext.property.serializedObject.ApplyModifiedProperties();
//    }

//    private class AddStateContext
//    {
//        public SerializedProperty property;
//        public Type stateType;
//    }
//}
