using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ConsumableVisualImage : MonoBehaviour
{

    public float scaleTo;
    
    public void MoveTo(Transform endPoint)
    {
        transform.DOMove(endPoint.position, 0.9f).OnComplete(EndMove);
        transform.DOScale(scaleTo, 0.8f);
    }

    void EndMove()
    {
        MainMenuUIController.instance._ShopConsumables.SetTextConsumables();
        Destroy(gameObject);
    }
}
