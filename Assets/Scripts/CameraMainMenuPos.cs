using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMainMenuPos : MonoBehaviour
{
    public Transform camTransform, posMenu, posShop;    
    void Start()
    {
        camTransform = transform;
    }

    public void SetCameraPosShop()
    {
        camTransform.DOMove(posShop.position, 1f);
        camTransform.DORotate(posShop.eulerAngles, 1f);
    }
    public void SetCameraPosMenu()
    {
        camTransform.DOMove(posMenu.position, 1f);
        camTransform.DORotate(posMenu.eulerAngles, 1f);
    }
}
