using Attributes.Input;
using Source.Editor.EditorSettings.Input;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.Input
{
    [CustomPropertyDrawer(typeof(ActionInputDropdownAttribute))]
    public class ActionInputDropdownAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                string[] options = EditorInputActionsDataSettings.instance.GetActionNames();
                int selectedIndex = Mathf.Max(0, System.Array.IndexOf(options, property.stringValue));

                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, options);
                property.stringValue = options[selectedIndex];
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}