using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInput : InputProfile
{
    public const string profileName = "UI Input";

    public UIInput()
    {
        name = "UI Input";

        RegisterEvents();
    }

    public UIInput(string _name)
    {
        name = _name;

        RegisterEvents();
    }
    public void RegisterEvents()
    {
    }



    public override void Update()
    {

    }
}
