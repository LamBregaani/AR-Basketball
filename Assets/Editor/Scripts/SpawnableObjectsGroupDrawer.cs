using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnableObjectsGroup<>))]
public class SpawnableObjectsGroupDrawer : PropertyDrawer
{
    //The rect postion used for displaying the properties
    private Rect currentRect;

    bool showSpawnRates = true;

    public bool[] lockedSpawnRates = new bool[0];

    const string lockTexture = "Assets/Editor/Textures/InspectorLock.png";

    const string unlockTexture = "Assets/Editor/Textures/InspectorUnLock.png";

    private int oldCount;

    private float[] oldSpawnRates;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = 2 * EditorGUIUtility.singleLineHeight;

        var prop = property.FindPropertyRelative(nameof(SpawnableObjectsGroup<object>.objects));

        if (prop != null)
        {
            if(prop.isExpanded)
            {
                height += Mathf.Clamp((prop.arraySize - 1) * 20, 0, Mathf.Infinity);

                height += 3 * EditorGUIUtility.singleLineHeight;
            }

            if (showSpawnRates)
                height += prop.arraySize * 20;

            while (prop.Next(true))
            {
                height += EditorGUI.GetPropertyHeight(prop, label) - EditorGUIUtility.singleLineHeight;
            }
        }

        return height;
    }

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        EditorGUI.BeginProperty(_position, _label, _property);

        currentRect = new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);

        bool valuesChanged = false;

        float changedValue = 0f;

        int changedValueIndex = 0;

        var count = 0;

        var spawnRates = new float[0];

        var prop = _property.FindPropertyRelative(nameof(SpawnableObjectsGroup<object>.objects));

        if (prop != null)
        {
            count = prop.arraySize;

            spawnRates = new float[count];

            var spawnRateProp = _property.FindPropertyRelative(nameof(SpawnableObjectsGroup<object>.objectSpawnRates));

            if (spawnRateProp != null)
            {
                var iterations = spawnRateProp.arraySize;

                if(iterations > count)
                {
                    iterations = count;
                }

                for (int i = 0; i < iterations; i++)
                {
                    spawnRates[i] = spawnRateProp.GetArrayElementAtIndex(i).floatValue;
                }
            }

        }

        var newSpawnRates = new float[count];

        var newLockedSpawnRates = new bool[count];

        int length = count;

        if(count > oldCount)
        {
            length = lockedSpawnRates.Length;
        }
        else if (count < oldCount)
        {
            ItemRemoved(spawnRates, oldCount - count);
        }

        Array.Copy(lockedSpawnRates, newLockedSpawnRates, length);

        if (count > 0)
        {
            if (count <= spawnRates.Length)
            {
                Array.Copy(spawnRates, newSpawnRates, count);
            }
            else
            {
                Array.Copy(spawnRates, newSpawnRates, spawnRates.Length);
            }
        }


        if(count > 0)
        {
            showSpawnRates = EditorGUI.BeginFoldoutHeaderGroup(GetRect(), showSpawnRates, "Spawn Rates");

            EditorGUI.indentLevel++;

            if (showSpawnRates)
            {
                if (count == 1)
                {
                    var locked = true;

                    var value = 100f;

                    var image = AssetDatabase.LoadAssetAtPath<Texture>(lockTexture);

                    var button = new GUIContent(image, "Lock this Spawnrate value");

                    if (GUI.Button(new Rect(23, 49, 20, 20), button, GUIStyle.none))
                    {
                        locked = !locked;
                    }

                    EditorGUI.BeginDisabledGroup(locked);

                    value = EditorGUI.Slider(GetRect(), new GUIContent("Item #1 Rate"), value, 0.1f, 100f);

                    EditorGUI.EndDisabledGroup();

                    newSpawnRates[0] = value;
                }
                else
                {

                    for (int i = 0; i < count; i++)
                    {
                        var value = spawnRates[i];

                        if (value == 0)
                            value = 1f;

                        var locked = newLockedSpawnRates[i];

                        Texture image = null;

                        if (locked == true)
                        {
                            image = AssetDatabase.LoadAssetAtPath<Texture>(lockTexture);
                        }
                        else
                        {
                            image = AssetDatabase.LoadAssetAtPath<Texture>(unlockTexture);
                        }

                        var button = new GUIContent(image, "Lock this Spawnrate value");

                        if (GUI.Button(new Rect(23, 49 + (20 * i), 20, 20), button, GUIStyle.none))
                        {
                            locked = !locked;
                        }

                        EditorGUI.BeginDisabledGroup(locked);

                        value = EditorGUI.Slider(GetRect(), new GUIContent($"Item #{i + 1} Rate"), value, 0.1f, 100);

                        EditorGUI.EndDisabledGroup();




                        newSpawnRates[i] = value;

                        if (spawnRates[i] != value)
                        {
                            valuesChanged = true;

                            changedValue = value;

                            changedValueIndex = i;
                        }

                        newLockedSpawnRates[i] = locked;
                    }
                }

                if (valuesChanged)
                    DistributeValues(newLockedSpawnRates, newSpawnRates, changedValue, spawnRates[changedValueIndex], changedValueIndex);

                prop = _property.FindPropertyRelative("objectSpawnRates");

                if (prop != null && newSpawnRates.Length > 0)
                {
                    for (int i = 0; i < newSpawnRates.Length; i++)
                    {
                        prop.arraySize = newSpawnRates.Length;

                        var valueProp = prop.GetArrayElementAtIndex(i);

                        valueProp.floatValue = newSpawnRates[i];
                    }
                }

                lockedSpawnRates = newLockedSpawnRates;
            }

            EditorGUI.indentLevel--;

            EditorGUI.EndFoldoutHeaderGroup();
        }

        EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(SpawnableObjectsGroup<object>.objects)));

        EditorGUI.EndProperty();

        oldSpawnRates = newSpawnRates;

        oldCount = count;
    }

    private void ItemRemoved(float[] spawnrates, int change)
    {
        

        var value = 0f;

        var iter = 0;
        
        for (int i = oldSpawnRates.Length - 1; i > oldSpawnRates.Length - change - 1; i--)
        {
            value += oldSpawnRates[i];

            iter++;
        }

        var unlockedCount = 0;

        for (int i = 0; i < oldSpawnRates.Length - 1; i++)
        {
            if (lockedSpawnRates[i] == false)
            {
                unlockedCount++;
            }
        }

        
        var addition = value / (unlockedCount - change);

        for (int i = 0; i < oldSpawnRates.Length - change; i++)
        {
            if (lockedSpawnRates[i] == false)
            {
                spawnrates[i] += addition;
            }
        }
    }

    private Rect GetRect()
    {
        var value = currentRect;

        currentRect = new Rect(currentRect.x, currentRect.y + 20, currentRect.width, 16);

        return value;
    }

    /// <summary>
    /// Used to get the rect that should be used and iterate the postion down the passed number of times
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private Rect GetRect(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }

        var value = currentRect;

        for (int i = 0; i < amount; i++)
        {
            currentRect = new Rect(currentRect.x, currentRect.y + 20, currentRect.width, 16);
        }

        return value;
    }

    public static float[] DistributeValues(bool[] lockedValues, float[] values, float newValue, float oldValue, int index)
    {

        var unlockedCount = 0;

        for (int i = 0; i < lockedValues.Length; i++)
        {
            if(lockedValues[i] == false)
            {
                unlockedCount++;
            }
        }

        if(unlockedCount == 1)
        {
            
            values[index] = oldValue;

            return values;
        }

        var maxValue = 100f;

        for (int i = 0; i < values.Length; i++)
        {
            if (lockedValues[i] == true)
            {
                maxValue -= values[i];
            }
        }

        maxValue -= 0.1f * (unlockedCount - 1);

        for (int i = 0; i < values.Length; i++)
        {
            var amount = (oldValue - newValue) / (unlockedCount - 1);
                

            if (lockedValues[i] == false)
            {
                if(i != index)
                values[i] += amount;

                values[i] = Mathf.Clamp(values[i], 0.1f, maxValue);
            }
        }

        return values;
    }
}
