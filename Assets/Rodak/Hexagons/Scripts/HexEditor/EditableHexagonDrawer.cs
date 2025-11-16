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
        // If true, Q and R auto corrects S, S auto corrects Q
        // If fale, Q auto corrects R, R auto corrects S, S auto corrects Q
        public static bool TwoComponentAutoCorrect = false;

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
            float spacing = 5f;

            float totalFieldWidth = position.width - (spacing * (propertyNames.Length - 1));
            float fieldWidth = totalFieldWidth / propertyNames.Length;
            float labelWidth = 13f;

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = labelWidth;

            Rect fieldRect = new Rect(position.x, position.y, fieldWidth, position.height);

            for (int i = 0; i < propertyNames.Length; i++)
            {
                if (i > 0)
                {
                    fieldRect.x += fieldWidth + spacing;
                }

                string propertyName = propertyNames[i];
                SerializedProperty subProperty = property.FindPropertyRelative(propertyName);

                EditorGUI.BeginChangeCheck();

                EditorGUI.PropertyField(fieldRect, subProperty, new GUIContent(propertyName));

                if (EditorGUI.EndChangeCheck())
                {
                    UpdateValue(propertyName, property);
                }
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndProperty();
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
                    if (TwoComponentAutoCorrect)
                    {
                        sProperty.intValue = -(q + r);
                    }
                    else
                    {
                        rProperty.intValue = -(q + s);
                    }
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