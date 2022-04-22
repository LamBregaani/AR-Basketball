using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private List<InputProfile> m_profiles = new List<InputProfile>();

    public Dictionary<string, InputProfile> inputProfiles = new Dictionary<string, InputProfile>();

    public InputProfile currentInputProfile;

    public static InputController instance;

    private const string m_starterProfile = "UI Input";

    public void Awake()
    {
        //if (instance != null)
        //    Destroy(this);

        instance = this;

        inputProfiles.Add(UIInput.profileName, new UIInput());

        inputProfiles.Add(MainInput.profileName, new MainInput());

        currentInputProfile = inputProfiles[m_starterProfile];
    }

    private void Update()
    {
        currentInputProfile.Update();
    }
    /// <summary>
    /// Set the current input profile to the passed profile
    /// </summary>
    /// <param name="_profileName"></param>

    public void ChangeProfile(string _profileName)
    {
        currentInputProfile = inputProfiles[_profileName];
    }

    /// <summary>
    /// Remove an input profile
    /// </summary>
    /// <param name="_profilename"></param>
    public void RemoveProfile(string _profilename)
    {
        inputProfiles.Remove(_profilename);
    }

    /// <summary>
    /// Add an input profile
    /// </summary>
    /// <param name="_profile"></param>
    public void AddProfile(InputProfile _profile)
    {
        inputProfiles.Add(_profile.name, _profile);
    }
}


