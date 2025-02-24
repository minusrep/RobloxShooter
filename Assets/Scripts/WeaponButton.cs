using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public string weaponType;
    public int weaponId;
    public int needMoneyCount;

    public GameObject equipped, loock, costGO;

    public TMP_Text nameText, costText;
    private Image chosedImage;
    [HideInInspector]
    public WeaponShopTypesPanel _WeaponShopTypesPanel;
    
    public void SetNameText(string name)
    {
        nameText.text = name;
    }

    void Start()
    {
        SetNameText(WeaponData.instance.GetNameWeapon(weaponType,weaponId));
        if (WeaponData.instance.isActiveWeapon(weaponType,weaponId)==false)
        {
            loock.SetActive(true);
            costGO.SetActive(true);
            
            if (costText != null)
            {
                costText.text = needMoneyCount.ToString();
            }
        }
        else
        {
            if (needMoneyCount == 0)
            {
                costGO.SetActive(false);
                loock.SetActive(false);
            }
            else
            {
                loock.SetActive(false);
                costGO.SetActive(false);
            }
        }
    }

    public void SetBuy()
    {
        if (needMoneyCount > 0)
        {
            if (WeaponData.instance.isActiveWeapon(weaponType,weaponId)==false)
            {
                loock.SetActive(true);
                costGO.SetActive(true);
                if (costText != null)
                {
                    costText.text = needMoneyCount.ToString();
                }
            }
            else
            {
                loock.SetActive(false);
                costGO.SetActive(false);
            }
        }
    }

    public void SetChosed(Sprite isChose)
    {
        if (chosedImage == null)
        {
            chosedImage = GetComponent<Image>();
        }

        chosedImage.sprite = isChose;
    }
    public void SetEquipped(bool isEquip)
    {
        equipped.SetActive(isEquip);
    }
    
    public void SetWeaponID()
    {
        _WeaponShopTypesPanel.SetWeaponChosed(weaponId);
    }
}
