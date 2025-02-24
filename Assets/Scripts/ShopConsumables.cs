using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopConsumables : MonoBehaviour
{
    public Scrollbar _Scrollbar;

    public Transform firstaidImage, armorImage, grenadeImage;
    public Transform firstAidEndPoint, armorEndPoint, grenadeEndPoint;

    public GameObject consumableInfo;
    public TMP_Text firstaidCountText, armorCountText, grenadeCountText;
    public void SetActiveInfo(bool isActive)
    {
        consumableInfo.SetActive(isActive);
        if (isActive)
        {
            SetTextConsumables();
        }
    }
    public void DoValue(float endValue)
    {
        float value = _Scrollbar.value;
        DOTween.To(() => value, x => value = x, endValue, 0.2f)
            .OnUpdate(() =>
            {
                _Scrollbar.value = value;
            });
    }

    public void SetTextConsumables()
    {
        firstaidCountText.text = PlayerData.instance.firstAidCount.ToString();
        armorCountText.text = PlayerData.instance.shieldCount.ToString();
        grenadeCountText.text = PlayerData.instance.grenadeCount.ToString();
    }

    public void AddConsumables(string type, Transform SpawnPoint)
    {
        if (type == "firstaid")
        {
            Transform image = Instantiate(firstaidImage, SpawnPoint.position, Quaternion.identity,transform);
            image.GetComponent<ConsumableVisualImage>().MoveTo(firstAidEndPoint);
        }
        if (type == "armor")
        {
            Transform image = Instantiate(armorImage, SpawnPoint.position, Quaternion.identity,transform);
            image.GetComponent<ConsumableVisualImage>().MoveTo(armorEndPoint);
        }
        if (type == "grenade")
        {
            Transform image = Instantiate(grenadeImage, SpawnPoint.position, Quaternion.identity,transform);
            image.GetComponent<ConsumableVisualImage>().MoveTo(grenadeEndPoint);
        }
    }
    
    
}
