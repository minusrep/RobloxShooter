using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageUI : MonoBehaviour
{
    public CanvasGroup group;
    private bool isDamaged;
    
    public void SetDamage()
    {
        if (!isDamaged)
        {
            group.DOFade(1f, 0.5f).OnComplete(ResetDamage);
            isDamaged = true;
        }
    }

    void ResetDamage()
    {
        group.DOFade(0f, 0.5f).OnComplete(() =>
        {
            isDamaged = false;
        });
    }
}
