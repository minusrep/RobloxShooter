using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinButton : MonoBehaviour
{
    public int skinId;
    public int needMoneyCount;

    public GameObject equipped, loock, costGO;

    public TMP_Text costText;
    void Start()
    {
        if (SkinData.instance.isActiveSkin(skinId))
        {
            loock.SetActive(false);
            costGO.SetActive(false);
        }
        else
        {
            loock.SetActive(true);
            costGO.SetActive(true);
            if (costText != null)
            {
                costText.text = needMoneyCount.ToString();
            }
        }
    }
    public void SetBuy()
    {
        if (skinId > 0)
        {
            if (SkinData.instance.isActiveSkin(skinId))
            {
                loock.SetActive(false);
                costGO.SetActive(false);
            }
            else
            {
                loock.SetActive(true);
                costGO.SetActive(true);
                if (costText != null)
                {
                    costText.text = needMoneyCount.ToString();
                }
            }
        }
    }
    public void SetEquipped(bool isEquip)
    {
        equipped.SetActive(isEquip);
    }
    
    public void SetSkinID()
    {
        MainMenuUIController.instance._ShopSkin.SetScinChosed(skinId);
    }
    
}
