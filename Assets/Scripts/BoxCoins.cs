using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxCoins : MonoBehaviour
{
    public UnityEvent _action;
    void OnMouseDown () {
        if (MainMenuUIController.instance.isUIOpen == false)
        {
            _action.Invoke();
        }
    }
}
