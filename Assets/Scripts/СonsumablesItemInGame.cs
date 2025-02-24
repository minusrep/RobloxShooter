using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class СonsumablesItemInGame : MonoBehaviour
{
    public string itemName;
    public int itemCount;
    public GameObject textCountGo, adsGo;
    public TMP_Text textCount;
    void Start()
    {
        itemCount = PlayerData.instance.GetConsumable(itemName);
        Debug.Log(itemCount);
        if (itemCount > 5)
        {
            itemCount = 5;
            textCount.text = itemCount.ToString();
            textCountGo.SetActive(true);
            adsGo.SetActive(false);
        }else if (itemCount > 0)
        {
            textCount.text = itemCount.ToString();
            textCountGo.SetActive(true);
            adsGo.SetActive(false);
        }
        if (itemCount <= 0)
        {
            textCountGo.SetActive(false);
            adsGo.SetActive(true);
        }
    }


    public void GetConsumables()
    {
        if (itemCount > 0)
        {
            itemCount--;
            PlayerData.instance.SetConsumableMinus(itemName, 1);
            textCount.text = itemCount.ToString();
            SetConsumable();
            if (itemCount <= 0)
            {
                textCountGo.SetActive(false);
                adsGo.SetActive(true);
            }
        }
        else
        {
            GetAds();
        }
    }

    void GetAds()
    {
        PlayerData.instance.SetConsumable(itemName,1);
        itemCount++;
        textCount.text = itemCount.ToString();
        textCountGo.SetActive(true);
        adsGo.SetActive(false);
    }

    void SetConsumable()
    {
        PlayerController.instance.SetConsumable(itemName);
    }
}
