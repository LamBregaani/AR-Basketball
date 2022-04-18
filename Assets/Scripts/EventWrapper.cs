using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EventWrapper : UnityEvent
{
    public delegate void OnEventInvoked();

    public event OnEventInvoked onEventInvoked;

    public delegate void OnEventInvokedParam(object val);

    public event OnEventInvokedParam onEventInvokedParam;

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
    /// Invoke both the Unity event and the C# event with a parameter
    /// </summary>
    public void InvokeEvent(object val)
    {
        onEventInvokedParam?.Invoke(val);

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
    /// Add a listener to the C# event with a parameter
    /// </summary>
    /// <param name="_listener"></param>
    public void AddListener(OnEventInvokedParam _listener)
    {
        onEventInvokedParam += _listener;
    }


}


