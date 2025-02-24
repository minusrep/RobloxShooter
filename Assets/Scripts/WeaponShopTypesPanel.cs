using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopTypesPanel : MonoBehaviour
{
    public string wType;
    public int offsetID;
    [HideInInspector]
    public int wTypeID =-1;
    public Transform content;
    public Sprite choosedimage, noChoosedImage;
    private List<WeaponButton> _weaponButtons = new List<WeaponButton>();

    public void OpenPanel()
    {
        if (_weaponButtons.Count == 0)
        {
            foreach (Transform child in content)
            {
                _weaponButtons.Add(child.GetComponent<WeaponButton>());
                child.GetComponent<WeaponButton>()._WeaponShopTypesPanel = this;
            }
        }

        wTypeID = PlayerData.instance.GetEquippedWeapon(wType);
        CheckWeapon();
    }
    public void CheckWeapon()
    {
        foreach (WeaponButton button in _weaponButtons)
        {
            button.SetChosed(noChoosedImage);
            button.SetEquipped(false);
        }

        if (wTypeID != -1)
        {
            _weaponButtons[wTypeID].SetChosed(choosedimage);
            ShopWeapons.instance.SetChosedWeapon(wType,wTypeID,_weaponButtons[wTypeID].needMoneyCount,offsetID);
            ShopWeapons.instance.SetButtonsActiveEquipped();
            _weaponButtons[wTypeID].SetEquipped(true);
        }
        else
        {
            if (wType == "melee" || wType == "pistol")
            {
                _weaponButtons[0].SetChosed(choosedimage);
                ShopWeapons.instance.SetChosedWeapon(wType,0,_weaponButtons[0].needMoneyCount,offsetID);
                ShopWeapons.instance.SetButtonsActiveEquip();
            }
            else
            {
                ShopWeapons.instance.UnActiveButtons();
            }
        }
    }
    public void SetWeaponChosed(int weaponID)
    {
        foreach (WeaponButton button in _weaponButtons)
        {
            button.SetChosed(noChoosedImage);
            button.SetEquipped(false);
        }
        _weaponButtons[weaponID].SetChosed(choosedimage);
        ShopWeapons.instance.SetChosedWeapon(wType,weaponID,_weaponButtons[weaponID].needMoneyCount,offsetID);
        if (weaponID == wTypeID)
        {
            ShopWeapons.instance.SetButtonsActiveEquipped();
            _weaponButtons[weaponID].SetEquipped(true);
        }
        else
        {
            if (wTypeID != -1)
            {
                _weaponButtons[wTypeID].SetEquipped(true);
            }

            if (WeaponData.instance.isActiveWeapon(wType,weaponID)==false)
            {
                if (_weaponButtons[weaponID].needMoneyCount == 0)
                {
                    ShopWeapons.instance.SetButtonsActiveEquip();
                }
                else
                {
                    ShopWeapons.instance.SetButtonsActiveBuy(_weaponButtons[weaponID].needMoneyCount.ToString());
                }
            }
            else
            {
                ShopWeapons.instance.SetButtonsActiveEquip();
            }
        }
    }

    public void SetWeaponChosedEquip(int weaponID)
    {
        wTypeID = weaponID;
        foreach (WeaponButton button in _weaponButtons)
        {
            button.SetChosed(noChoosedImage);
            button.SetEquipped(false);
        }
        _weaponButtons[weaponID].SetChosed(choosedimage);
        ShopWeapons.instance.SetChosedWeapon(wType,weaponID,_weaponButtons[weaponID].needMoneyCount,offsetID);
        if (weaponID == wTypeID)
        {
            ShopWeapons.instance.SetButtonsActiveEquipped();
            _weaponButtons[weaponID].SetEquipped(true);
        }
        else
        {
            if (wTypeID != -1)
            {
                _weaponButtons[wTypeID].SetEquipped(true);
            }

            if (WeaponData.instance.isActiveWeapon(wType,weaponID)==false)
            {
                
                ShopWeapons.instance.SetButtonsActiveBuy(_weaponButtons[weaponID].needMoneyCount.ToString());
                
            }
            else
            {
                ShopWeapons.instance.SetButtonsActiveEquip();
            }
        }
    }
    public void SetBuyed(int weaponID)
    {
        _weaponButtons[weaponID].SetBuy();
        //SetWeaponChosed(weaponID);
        OpenPanel();
    }
}
