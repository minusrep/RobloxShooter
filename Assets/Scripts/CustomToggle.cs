using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomToggle : MonoBehaviour
{
    public bool isToggle;
    public GameObject toggleOn, toggleOff;

    public UnityEvent _action;

    
    public void SetToggle(int isVibro)
    {
        if (isVibro==1)
        {
            toggleOn.SetActive(true);
            toggleOff.SetActive(false);
            isToggle = true;
        }
        else
        {
            toggleOff.SetActive(true);
            toggleOn.SetActive(false);
            isToggle = false;
        }
    }

    public void Toggle()
    {
        if (isToggle)
        {
            toggleOn.SetActive(false);
            toggleOff.SetActive(true);
            isToggle = false;
        }
        else
        {
            toggleOff.SetActive(false);
            toggleOn.SetActive(true);
            isToggle = true;
        }
        _action.Invoke();
    }
}
