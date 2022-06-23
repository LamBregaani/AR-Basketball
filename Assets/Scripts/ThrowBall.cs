using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    [Tooltip("The maximum amount of force the ball can be thrown at")]
    [SerializeField] private float m_maxThrowForce;

    [Tooltip("The time bewteen being able to throw the ball")]
    [SerializeField] private float m_throwDelay;

    [Tooltip("the point where the ball is fired from")]
    [SerializeField] private Transform m_throwPosition;

    [Tooltip("The camera or object to get the forward vector from")]
    [SerializeField] private GameObject m_camera;

    public EventWrapper onBallThorwn = new EventWrapper();

    public static Action onBallthrownGlobal = delegate { };

    //Call for getting a ball from a pool
    private BallSupplier m_ballSupplier;

    //Refernce to the main input profile
    private MainInput m_input;

    //Used for the throw delay
    private bool m_canThrow = true;

    private void Awake()
    {
        m_ballSupplier = GetComponent<BallSupplier>();
    }

    private void Start()
    {
        m_input = InputController.instance.inputProfiles[MainInput.profileName] as MainInput;

        m_input.onSwipeEnded.AddListener(Throw);
    }

    private void OnEnable()
    {
        m_input?.onSwipeEnded.AddListener(Throw);
    }

    private void OnDisable()
    {
        m_input?.onSwipeEnded.RemoveListener(Throw);
    }



    public void Throw(Swipe _swipeData, float _percentage)
    {
        
        if (!m_canThrow)
            return;

        //Gets a new object from the pool
        var ball = m_ballSupplier.GetNewBall();

        //Changes the object;s postion to the throw position
        ball.transform.position = m_throwPosition.position;

        //Used to slighty angle the direction the ball is thrown based on the normalized direction of the swipe
        var angle = _swipeData.direction2D.normalized;

        //Set the direction the ball will be thrown
        Vector3 direction = new Vector3(m_camera.transform.forward.x + angle.x, m_camera.transform.forward.y, m_camera.transform.forward.z);

        m_canThrow = false;

        ball.AddForce(direction * _percentage * m_maxThrowForce, ForceMode.Impulse);

        onBallThorwn?.InvokeEvent();

        onBallthrownGlobal?.Invoke();

        StartCoroutine(ThrowDelay());
    }

    /// <summary>
    /// The delay between being able to fire a ball
    /// </summary>
    /// <returns></returns>
    private IEnumerator ThrowDelay()
    {
        yield return new WaitForSeconds(m_throwDelay);
        m_canThrow = true;
    }
}


