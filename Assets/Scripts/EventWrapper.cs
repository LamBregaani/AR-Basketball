using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class used to create a joint C# and Unity event
/// </summary>
[System.Serializable]
public class EventWrapper : UnityEvent
{
    public delegate void OnEventInvoked();

    public event OnEventInvoked onEventInvoked;

    public EventWrapper onEventInvokedUnity;

    /// <summary>
    /// Invoke both the Unity event and regular C# event
    /// </summary>
    public void InvokeEvent()
    {
        onEventInvoked?.Invoke();

        Invoke();
    }

    /// <summary>
    /// Add a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void AddListener(OnEventInvoked _listener)
    {
        onEventInvoked += _listener;
    }

    /// <summary>
    /// Remove a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void RemoveListener(OnEventInvoked _listener)
    {
        onEventInvoked -= _listener;
    }


}

/// <summary>
/// Create an event that takes parameter T
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class EventWrapper<T> : UnityEvent
{
    public delegate void OnEventInvoked(T val);

    public event OnEventInvoked onEventInvoked;

    public EventWrapper<T> onEventInvokedUnity;

    /// <summary>
    /// Invoke both the Unity event and the C# event
    /// </summary>
    public void InvokeEvent(T val)
    { 
        Invoke();
        onEventInvoked.Invoke(val);
    }

    /// <summary>
    /// Add a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void AddListener(OnEventInvoked _listener)
    {
        onEventInvoked += _listener;
    }

    /// <summary>
    /// Remove a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void RemoveListener(OnEventInvoked _listener)
    {
        onEventInvoked -= _listener;
    }

}

/// <summary>
/// Used to make an event wrapper with two type paramaters
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class EventWrapper<T1, T2> : UnityEvent
{
    public delegate void OnEventInvoked(T1 val, T2 val2);

    public event OnEventInvoked onEventInvoked;

    public EventWrapper<T1> onEventInvokedUnity;

    /// <summary>
    /// Invoke both the Unity event and the C# event
    /// </summary>
    public void InvokeEvent(T1 val, T2 val2)
    {
        Invoke();
        onEventInvoked.Invoke(val, val2);
    }

    /// <summary>
    /// Add a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void AddListener(OnEventInvoked _listener)
    {
        onEventInvoked += _listener;
    }

    /// <summary>
    /// Remove a listener to the C# event
    /// </summary>
    /// <param name="_listener"></param>
    public void RemoveListener(OnEventInvoked _listener)
    {
        onEventInvoked -= _listener;
    }

}


