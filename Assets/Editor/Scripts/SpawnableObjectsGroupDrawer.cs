using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnableObjectsGroup<>))]
public class SpawnableObjectsGroupDrawer : PropertyDrawer
{
    //The rect postion used for displaying the properties
    private Rect currentRect;

    //Bool for the spawnrates drop down
    bool showSpawnRates = true;

    //Files for the lock textures
    const string lockTexture = "Assets/Editor/Textures/InspectorLock.png";

    const string unlockTexture = "Assets/Editor/Textures/InspectorUnLock.png";

    const string lockTexttureRuntime = "Assets/Editor/Textures/InspectorLockRuntime.png";


    //
    private SpawnableObjectsGroup<object>.valueLockType[] lockedSpawnRates = new SpawnableObjectsGroup<object>.valueLockType[0];

    private float[] previousSpawnRates = new float[0];

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

        float changedValue = 0f;

        int changedValueIndex = -1;

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

        var newLockedSpawnRates = new SpawnableObjectsGroup<object>.valueLockType[count];

        int length = count;

        if(count > previousSpawnRates.Length)
        {
            length = lockedSpawnRates.Length;
        }
        else if (count < previousSpawnRates.Length)
        {
            ItemRemoved(spawnRates, previousSpawnRates.Length - count);
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

                    value = EditorGUI.Slider(GetRect(), new GUIContent("Item #1 Rate"), value, 0f, 100f);

                    EditorGUI.EndDisabledGroup();

                    newSpawnRates[0] = value;
                }
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        var value = spawnRates[i];

                        var locked = newLockedSpawnRates[i];

                        var index = i;

                        Texture image = null;

                        var isUnlocked = false;

                        var isLocked = false;

                        var isLockedRuntime = false;

                        switch (((int)locked))
                        {
                            case 0:
                                image = AssetDatabase.LoadAssetAtPath<Texture>(unlockTexture);
                                isUnlocked = true;
                                break;

                            case 1:
                                image = AssetDatabase.LoadAssetAtPath<Texture>(lockTexture);
                                isLocked = true;
                                break;

                            case 2:
                                image = AssetDatabase.LoadAssetAtPath<Texture>(lockTexttureRuntime);
                                isLockedRuntime = true;
                                break;

                            default:
                                image = AssetDatabase.LoadAssetAtPath<Texture>(unlockTexture);
                                isUnlocked = true;
                                break;
                        }

                        if (EditorGUI.DropdownButton(new Rect(23, 49 + (20 * i), 20, 20), new GUIContent(image), FocusType.Keyboard, new GUIStyle()
                        {
                            fixedWidth = 150f,
                            border = new RectOffset(1, 1, 1, 1)
                        }))
                        {
                            GenericMenu menu = new GenericMenu();


                            menu.AddDisabledItem(new GUIContent($"Item {index + 1}"));

                            menu.AddSeparator("");

                            menu.AddItem(new GUIContent($"Unlocked"), isUnlocked, () => newLockedSpawnRates[index] = SpawnableObjectsGroup<object>.valueLockType.unlocked);

                            menu.AddItem(new GUIContent("Locked"), isLocked, () => newLockedSpawnRates[index] = SpawnableObjectsGroup<object>.valueLockType.locked);

                            menu.AddItem(new GUIContent("Locked Runtime"), isLockedRuntime, () => newLockedSpawnRates[index] = SpawnableObjectsGroup<object>.valueLockType.lockedRuntime);

                            menu.ShowAsContext();

                            
                        }

                        EditorGUI.BeginDisabledGroup(!isUnlocked);

                        value = EditorGUI.Slider(GetRect(), new GUIContent($"Item #{i + 1} Rate"), value, 0f, 100);

                        EditorGUI.EndDisabledGroup();

                        newSpawnRates[i] = value;

                        if (spawnRates[i] != value)
                        {
                            changedValue = value;

                            changedValueIndex = i;
                        }
                    }
                }

                if (changedValueIndex != -1)
                    SpawnableObjectsGroup<object>.DistributeValuesEditor(newLockedSpawnRates, newSpawnRates, changedValue, spawnRates[changedValueIndex], changedValueIndex);

                prop = _property.FindPropertyRelative("objectSpawnRates");

                var lockedSpawnRatesProp = _property.FindPropertyRelative("lockedSpawnRates");

                if (prop != null && newSpawnRates.Length > 0)
                {
                    for (int i = 0; i < newSpawnRates.Length; i++)
                    {
                        prop.arraySize = newSpawnRates.Length;

                        var valueProp = prop.GetArrayElementAtIndex(i);

                        valueProp.floatValue = newSpawnRates[i];

                        lockedSpawnRatesProp.arraySize = newLockedSpawnRates.Length;

                        valueProp = lockedSpawnRatesProp.GetArrayElementAtIndex(i);

                        var boolVal = false;

                        if (newLockedSpawnRates[i] == SpawnableObjectsGroup<object>.valueLockType.lockedRuntime)
                            boolVal = true;

                        valueProp.boolValue = boolVal;
                    }
                }

                lockedSpawnRates = newLockedSpawnRates;
            }

            EditorGUI.indentLevel--;

            EditorGUI.EndFoldoutHeaderGroup();
        }

        EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(SpawnableObjectsGroup<object>.objects)));

        EditorGUI.EndProperty();

        previousSpawnRates = newSpawnRates;
    }

    private void ItemRemoved(float[] spawnrates, int change)
    {
        var unlockedCount = 0;


        for (int i = 0; i < previousSpawnRates.Length - change; i++)
        {

            if (lockedSpawnRates[i] == SpawnableObjectsGroup<object>.valueLockType.unlocked)
            {
                unlockedCount++;
            }
        }

        var remainder = 100f;


        for (int i = 0; i < previousSpawnRates.Length - change; i++)
        {
            remainder -= previousSpawnRates[i];
        }
        

        var addition = remainder / unlockedCount;

        for (int i = 0; i < previousSpawnRates.Length - change; i++)
        {

            if (lockedSpawnRates[i] == SpawnableObjectsGroup<object>.valueLockType.unlocked)
            {
                spawnrates[i] += addition;
            }
        }

        var debug = 0f;

        for (int i = 0; i < spawnrates.Length; i++)
        {
            debug += spawnrates[i];
        }

        Debug.Log($"Item removed: Total = {debug}");
    }

    private Rect GetRect()
    {
        var value = currentRect;

        currentRect = new Rect(currentRect.x, currentRect.y + 20, currentRect.width, 16);

        return value;
    }
}
