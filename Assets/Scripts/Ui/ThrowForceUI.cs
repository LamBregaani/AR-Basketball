using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used to update the UI element 
/// </summary>
public class ThrowForceUI : MonoBehaviour
{
    //The UI image
    [SerializeField] private Image m_image;

    //A gradient to color the image
    [SerializeField] private Gradient m_gradient;

    //A refernce to the main input profile
    private MainInput m_input;

    private void Start()
    {
        m_input = InputController.instance.inputProfiles[MainInput.profileName] as MainInput;

        m_input.oncontinuedSwipe.AddListener(UpdateArrow);

        m_input.onSwipeEnded.AddListener(ClearImage);
    }

    private void OnEnable()
    {
        m_input?.oncontinuedSwipe.AddListener(UpdateArrow);

        m_input?.onSwipeEnded.AddListener(ClearImage);
    }

    /// <summary>
    /// Rotate the image based on the swipe direction and fill the image based on a percentage 
    /// </summary>
    /// <param name="_swipeData"></param>
    /// <param name="_percentage"></param>
    public void UpdateArrow(Swipe _swipeData, float _percentage)
    {
        
        Vector2 dir = _swipeData.direction2D;

        //Get the angle of the swipe
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;

        //Set the image's rotation based on the angle
        transform.rotation = Quaternion.AngleAxis(angle, (Vector3.forward));

        //Set the image's fill amount based on the percentage
        m_image.fillAmount = _percentage;

        //Set the image's color based on the percentage and gradiant
        m_image.color = m_gradient.Evaluate(_percentage);

    }

    private void ClearImage()
    {
        m_image.fillAmount = 0;
    }
}
