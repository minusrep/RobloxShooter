using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DamageUI : MonoBehaviour
{
    
    void Start()
    {
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), 0.5f).OnComplete(DestroyUI);
        GetComponent<Text>().DOFade(0f, 0.5f);
    }

    public void DestroyUI()
    {
        Destroy(gameObject);
    }
}
