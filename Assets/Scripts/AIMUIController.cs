using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIMUIController : MonoBehaviour
{
    public Image _aimImage;

    public Color red, green;
    

    public void SetAIMColor(int colorID)
    {
        if(colorID==0)
        {
            _aimImage.color = green;
        }else
        {
            _aimImage.color = red;
        }
    }

    public void SetAIM()
    {
        PlayerController.instance.SetAimCamera();
    }
}
