using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectSpawner))]
public class ObjectSpawnerDrawer : PropertyDrawer
{
    //The rect postion used for displaying the properties
    private Rect currentRect;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = 2 * EditorGUIUtility.singleLineHeight;

        var prop = property.FindPropertyRelative(nameof(ObjectSpawner.cycleObjects));

        if (prop != null)
        {
            if (prop.boolValue == true)
            {
                height += 3 * EditorGUIUtility.singleLineHeight;
            }
        }

        return height;
    }

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, _label, _property);

        currentRect = new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);

        var prop = _property.FindPropertyRelative(nameof(ObjectSpawner.maxObjects));

        EditorGUI.PropertyField(GetRect(), prop);

        if (prop.intValue < 0)
        {
            prop.intValue = 0;
            Debug.Log($"{prop.displayName} cannot be less than 0");
        }
            

        EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(ObjectSpawner.cycleObjects)));

        prop = _property.FindPropertyRelative(nameof(ObjectSpawner.cycleObjects));

        if (prop.boolValue == true)
        {
            EditorGUI.indentLevel++;

            prop = _property.FindPropertyRelative(nameof(ObjectSpawner.lifeTime));

            EditorGUI.PropertyField(GetRect(), prop);

            if (prop.intValue < 0)
            {
                prop.intValue = 0;
                Debug.Log($"{prop.displayName} cannot be less than 0");
            }

            prop = _property.FindPropertyRelative(nameof(ObjectSpawner.respawnTime));

            EditorGUI.PropertyField(GetRect(), prop);

            if (prop.intValue < 0)
            {
                prop.intValue = 0;
                Debug.Log($"{prop.displayName} cannot be less than 0");
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
        
    }

    private Rect GetRect()
    {
        var value = currentRect;

        currentRect = new Rect(currentRect.x, currentRect.y + 20, currentRect.width, 16);

        return value;
    }
}
