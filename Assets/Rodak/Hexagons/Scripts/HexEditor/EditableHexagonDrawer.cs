#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Unity.Collections;

namespace Rodak.Hexagons.HexEditor
{
    [CustomPropertyDrawer(typeof(EditableHexagon))]
    public class EditableHexagonDrawer : PropertyDrawer
    {
        private const string Q = "Q";
        private const string R = "R";
        private const string S = "S";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            bool isReadOnly = IsReadOnly(property);
            EditorGUI.BeginDisabledGroup(isReadOnly);

            string[] propertyNames = { Q, R, S };
            float propertyWidth = position.width / propertyNames.Length;

            for (int i = 0; i < propertyNames.Length; i++)
            {
                string propertyName = propertyNames[i];
                Rect propertyPosition = new(position.x + propertyWidth * i, position.y, propertyWidth, position.height);

                bool propertyChanged = AddSubProperty(propertyPosition, propertyName, i > 0, property);
                if (propertyChanged)
                {
                    UpdateValue(propertyName, property);
                }
            }

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndProperty();
        }

        private bool AddSubProperty(Rect position, string name, bool offsetLabel, SerializedProperty property)
        {
            SerializedProperty subProperty = property.FindPropertyRelative(name);

            float labelWidth = 13f;
            float propertyWidth = position.width - labelWidth;

            Rect labelPosition = new(position.x, position.y, labelWidth, position.height);
            Rect propertyPosition = new(position.x + labelWidth, position.y, propertyWidth, position.height);

            float spacing = 5;
            propertyPosition.xMin += spacing;
            labelPosition.xMax += spacing;

            if (offsetLabel)
                labelPosition.xMin += spacing;

            EditorGUI.LabelField(labelPosition, name);

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(propertyPosition, subProperty, GUIContent.none);
            return EditorGUI.EndChangeCheck();
        }

        private void UpdateValue(string changedProperty, SerializedProperty property)
        {
            SerializedProperty qProperty = property.FindPropertyRelative(Q);
            SerializedProperty rProperty = property.FindPropertyRelative(R);
            SerializedProperty sProperty = property.FindPropertyRelative(S);

            int q = qProperty.intValue;
            int r = rProperty.intValue;
            int s = sProperty.intValue;

            if (q + r + s == 0) return;

            switch (changedProperty)
            {
                case Q:
                    sProperty.intValue = -(q + r);
                    break;
                case R:
                    sProperty.intValue = -(q + r);
                    break;
                case S:
                    qProperty.intValue = -(r + s);
                    break;
                default:
                    throw new ArgumentException($"{nameof(changedProperty)} has unknown value: ${changedProperty}");
            }
        }

        private bool IsReadOnly(SerializedProperty property)
        {
            Type targetType = property.serializedObject.targetObject.GetType();
            FieldInfo field = targetType.GetField(property.name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            if (field == null)
                return false;

            return field.GetCustomAttribute<ReadOnlyAttribute>() != null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif