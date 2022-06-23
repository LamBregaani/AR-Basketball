using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(Hoop))]
public class HoopDrawer : PropertyDrawer
{
    //The rect postion used for displaying the properties
    private Rect currentRect;

    /// <summary>
    /// Sets the height of the property drawer based on how many properties are being displayed
    /// </summary>
    /// <param name="property"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //A single property line size is used by default
        int propertyAmount = 1;


        if(property.isExpanded)
        {
            //If the property is expanded start with the minimum size of the property
            propertyAmount = 8;

            //If randomizeHeight is true it will need to show one more property
            if (property.FindPropertyRelative(nameof(Hoop.randomizeHeight)).boolValue == true)
                propertyAmount++;

            //Different movement states will show a different number of properties
            //Right now the defualt state "None" or "0" only shows one, while the other states can show an extra 3
            if (property.FindPropertyRelative(nameof(Hoop.movementType)).intValue != 0)
            {
                //Other states show an extra 2
                propertyAmount += 2;

                //If randomizeSpeed is true it will need to show one more property
                if (property.FindPropertyRelative(nameof(Hoop.randomizeSpeed)).boolValue == true)
                {
                    propertyAmount++;
                }
            }

            //If hasBase is true it will need to show 2 more propertyies
            if (property.FindPropertyRelative(nameof(Hoop.hasBase)).boolValue == true)
            {
                propertyAmount += 2;
            }
        }

        //Mulitple the default line height by the amount of visable properties
        return EditorGUIUtility.singleLineHeight * propertyAmount;

    }

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {

        EditorGUI.BeginProperty(_position, _label, _property);

        currentRect = new Rect(_position.x, _position.y, _position.width, EditorGUIUtility.singleLineHeight);
        
        //This is just a way to get the current index # from the label for formatting purposes
        int.TryParse(_label.ToString()[_label.ToString().Length - 1].ToString(), out int result);

        //Add one to the previous result
        var number = (result + 1).ToString();

        //Create a foldout for the property
        if (_property.isExpanded = EditorGUI.Foldout(GetRect(), _property.isExpanded, new GUIContent($"Hoop {number}")))
        {
            EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.hoopObj)));

            //dropDownBoolProp is used for various controls bools for drop down menus
            //Here the property for the randomizeHeight bool is retrieved
            var dropDownBoolProp = _property.FindPropertyRelative(nameof(Hoop.randomizeHeight));

            //Create a double drop down menu for randomizing height or constant height
            CreateDoubleFieldDropdown(_property, "Height", "Constant", "Random between two values", dropDownBoolProp.boolValue, dropDownBoolProp.name);

            EditorGUI.indentLevel++;

            //Create a slider for the randomized height, or create a slider for a constant height
            if (dropDownBoolProp.boolValue == true)
            { 
                EditorGUI.PropertyField(GetRect(2), _property.FindPropertyRelative(nameof(Hoop.randomHeightSlider)));
            }
            else
            {
                EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.height)), new GUIContent());

            }

            EditorGUI.indentLevel--;

            EditorGUI.LabelField(GetRect(), new GUIContent("Movement Type"));

            //Create a property for the meveemnt eneum
            EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.movementType)), GUIContent.none);

            
            //Check if the movment type is something besides the defualt "None" or "0"
            if (_property.FindPropertyRelative(nameof(Hoop.movementType)).intValue != 0)
            {
                //Get the radnomizeSpeed Property
                dropDownBoolProp = _property.FindPropertyRelative(nameof(Hoop.randomizeSpeed));

                //This is to add an offset to the right that doesn't show up for some reason
                var rect = GetRect();

                //Create a drop down menu for using randomized speed or constant speed
                CreateDoubleFieldDropdown(_property, "Movement Speed", "Constant", "Random between two values", dropDownBoolProp.boolValue, dropDownBoolProp.name, new Rect(rect.x + 4, rect.y, rect.width, rect.height));

                EditorGUI.indentLevel++;

                //Create a slider for the randomized speed, or create a slider for a constant speed
                if (_property.FindPropertyRelative(nameof(Hoop.randomizeSpeed)).boolValue == true)
                {

                    EditorGUI.PropertyField(GetRect(2), _property.FindPropertyRelative(nameof(Hoop.randomSpeedSlider)));
                }
                else
                {

                    EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.movementSpeed)), new GUIContent());

                }

                EditorGUI.indentLevel--;

            }

            EditorGUI.indentLevel --;

            dropDownBoolProp = _property.FindPropertyRelative(nameof(Hoop.hasBase));

            //Create a dropdown menu for using a base or no base
            CreateDoubleFieldDropdown(_property, "Base", "None", "3 Pieces", dropDownBoolProp.boolValue, dropDownBoolProp.name);

            EditorGUI.indentLevel++;

            //Create properties for the three objects to create the base of the hoop
            if (_property.FindPropertyRelative(nameof(Hoop.hasBase)).boolValue == true)
            {              
                EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.baseObj)));

                EditorGUI.PropertyField(GetRect(), _property.FindPropertyRelative(nameof(Hoop.poseObj)));

                EditorGUI.PropertyField(GetRect(2), _property.FindPropertyRelative(nameof(Hoop.postBendObj)));
            }
            else
            {
                EditorGUI.LabelField(GetRect(), "None");
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    //Creates a dropdown menu with two options
    private void CreateDoubleFieldDropdown(SerializedProperty _property, string _dropdownName, string _item1Name, string _item2Name, bool _controlBool, string _fieldToModify)
    {

        var labelIcon = EditorGUIUtility.IconContent("Icon Dropdown");

        labelIcon.text = _dropdownName;

        if (EditorGUI.DropdownButton(GetRect(), labelIcon, FocusType.Keyboard, new GUIStyle()
        {
            fixedWidth = 150f,
            border = new RectOffset(1, 1, 1, 1)
        }))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent(_item1Name), !_controlBool, () => SetBoolProperty(_property, _fieldToModify, false));

            menu.AddItem(new GUIContent(_item2Name), _controlBool, () => SetBoolProperty(_property, _fieldToModify, true));

            menu.ShowAsContext();
        }
    }


    //Creates a dropdown menu using the passed Rect
    private void CreateDoubleFieldDropdown(SerializedProperty _property, string _dropdownName, string _item1Name, string _item2Name, bool _controlBool, string _fieldToModify, Rect position)
    {

        var labelIcon = EditorGUIUtility.IconContent("Icon Dropdown");

        labelIcon.text = _dropdownName;

        if (EditorGUI.DropdownButton(position, labelIcon, FocusType.Keyboard, new GUIStyle()
        {
            fixedWidth = 150f,
            border = new RectOffset(1, 1, 1, 1)
        }))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent(_item1Name), !_controlBool, () => SetBoolProperty(_property, _fieldToModify, false));

            menu.AddItem(new GUIContent(_item2Name), _controlBool, () => SetBoolProperty(_property, _fieldToModify, true));

            menu.ShowAsContext();
        }
    }

    /// <summary>
    /// Set a bool property
    /// </summary>
    /// <param name="_property"></param>
    /// <param name="field"></param>
    /// <param name="value"></param>
    public void SetBoolProperty(SerializedProperty _property, string field, bool value)
    {
        _property.FindPropertyRelative(field).boolValue = value;

        _property.serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Used to get the rect that should be used
    /// </summary>
    /// <returns></returns>
    private Rect GetRect()
    {
        var value = currentRect;

        currentRect = new Rect(currentRect.x, currentRect.y + EditorGUIUtility.singleLineHeight, currentRect.width, 16);

        return value;
    }

    /// <summary>
    /// Used to get the rect that should be used and iterate the postion down the passed number of times
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private Rect GetRect(int amount)
    {
        if(amount < 0)
        {
            amount = 0;
        }

        var value = currentRect;

        for (int i = 0; i < amount; i++)
        {
            currentRect = new Rect(currentRect.x, currentRect.y + EditorGUIUtility.singleLineHeight, currentRect.width, 16);
        }

        return value;
    }
}


