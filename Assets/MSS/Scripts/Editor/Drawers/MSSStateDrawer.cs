﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Obel.MSS;

namespace Obel.MSS.Editor
{
    /*
    [CustomPropertyDrawer(typeof(MSSState))]
    public class MSSStateDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.LabelField(position, (property.FindPropertyRelative("tweens") == null).ToString());
            //EditorGUI.PropertyField(position, property.FindPropertyRelative("name"), GUIContent.none);
            EditorGUI.EndProperty();
        }
    }
    */
}
