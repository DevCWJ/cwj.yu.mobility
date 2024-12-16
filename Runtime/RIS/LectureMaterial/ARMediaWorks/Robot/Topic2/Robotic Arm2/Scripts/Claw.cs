using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class Claw : MonoBehaviour
{
    [SerializeField] Animator ClawAnimator;
    [SerializeField] Slider[] Robotic_slider;
    [SerializeField] GameObject[] Robotic_obj;
    [SerializeField] TextMeshProUGUI[] RotationText;
    bool IsPressed=false;
    [SerializeField] Button ClawButton;
    void Start()
    {
        ClawAnimator.enabled = false;
        for (int i = 0; i < Robotic_slider.Length; i++)
        {
            int index = i;
            Robotic_slider[i].onValueChanged.AddListener(v => OnSliderChanged(v, index));
        }
    }

    public void OpenClose()
    {
        ClawAnimator.enabled = true;

        if (!IsPressed)
        {
            ClawButton.GetComponent<Image>().color = Color.green;
            ClawAnimator.SetBool("IsOpen", true);
            IsPressed = true;
        }
        else
        {
            ClawButton.GetComponent<Image>().color = Color.red;
            ClawAnimator.SetBool("IsOpen", false);
            IsPressed = false;
        }

    }
    void OnSliderChanged(float value, int i)
    {
        RotationText[i].SetText(value.ToString("F1"));
        Robotic_obj[i].transform.localRotation = (i == 4 || i == 0) ? Quaternion.Euler(0, value, 0)
                                                                    : Quaternion.Euler(0, 0, value);
    }
}
