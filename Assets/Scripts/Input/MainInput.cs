using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInput : InputProfile
{
    public EventWrapper<Vector2> onInputStarted = new EventWrapper<Vector2>();

    public EventWrapper<Vector2> onInputEnded = new EventWrapper<Vector2>();

    public EventWrapper<Vector2> onInput = new EventWrapper<Vector2>();

    private bool m_inputStarted;

    private Vector2 m_lastPosition;

    private float m_maxSwipeDistance = Screen.height * 0.2f;

    private Swipe m_swipe = new Swipe();

    public EventWrapper<Swipe, float> oncontinuedSwipe = new EventWrapper<Swipe, float>();

    public EventWrapper<Swipe, float> onSwipeEnded = new EventWrapper<Swipe, float>();

    public const string profileName = "Main Input";

    public MainInput()
    {
        name = "Main Input";

        RegisterEvents();
    }

    public MainInput(string _name)
    {
        name = _name;

        RegisterEvents();
    }
    public void RegisterEvents()
    {


        onInputStarted.AddListener(SwipeStarted);

        onInputEnded.AddListener(SwipeEnded);

        onInput.AddListener(SwipeContinued);
    }

    ~MainInput()
    {
        onInputStarted.RemoveAllListeners();

        onInputEnded.RemoveAllListeners();

        onInput.RemoveAllListeners();
    }

    public override void Update()
    {
#if !UNITY_EDITOR
        if (Input.touchCount > 0)
        {
            if (!m_inputStarted)
            {
                m_inputStarted = true;

                onInputStarted.InvokeEvent(Input.touches[0].position);
            }

            m_lastPosition = Input.touches[0].position;

            onInput.InvokeEvent(Input.touches[0].position);
        }
        else
        {
            if (m_inputStarted)
            {
                m_inputStarted = false;

                onInputEnded.InvokeEvent(m_lastPosition);
            }
        }
#endif
        //Editor Debugging controls
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_inputStarted)
            {
                m_inputStarted = true;

                onInputStarted.InvokeEvent(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_inputStarted)
            {
                m_inputStarted = false;

                onInputEnded.InvokeEvent(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            onInput.InvokeEvent(Input.mousePosition);
        }
#endif
    }

    private void SwipeContinued(Vector2 _pos)
    {
        if (_pos.y + 10 > m_swipe.startPosition.y)
            return;

        m_swipe.CalculateDistance(_pos);

        oncontinuedSwipe.InvokeEvent(m_swipe, GetSwipePercentage(m_swipe.distance));
    }

    public float GetSwipePercentage(float _distance)
    {

        var dis = Mathf.Clamp(m_swipe.distance, m_maxSwipeDistance * 0.1f, m_maxSwipeDistance);

        return dis / m_maxSwipeDistance;
    }



    private void SwipeEnded(Vector2 _pos)
    {
        m_swipe.SwipeEnded(_pos);

        onSwipeEnded.InvokeEvent(m_swipe, GetSwipePercentage(m_swipe.distance));
    }


    private void SwipeStarted(Vector2 _pos)
    {
        m_swipe = new Swipe(_pos);
    }

}
