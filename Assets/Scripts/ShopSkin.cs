using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkin : MonoBehaviour
{
    
    public GameObject btnMoney, btnEquipped, btnEquip;
    public TMP_Text needMoneytext;

    public List<Image> _ButtonsImages = new List<Image>();
    public List<SkinButton> _SkinButtons = new List<SkinButton>();
    public Sprite choosedimage, noChoosedImage;

    public int skinPlayerID;

    public int BuySkinID;

    public PlayerScinController _ScinController;
    void Start()
    {
        skinPlayerID = PlayerData.instance.playerSkinID;
        SetScinChosed(skinPlayerID);
    }

    
    public void SetScinChosed(int skinID)
    {
        _ScinController.SetScinAtID(skinID);
        foreach (Image button in _ButtonsImages)
        {
            button.sprite = noChoosedImage;
        }
        foreach (SkinButton button in _SkinButtons)
        {
            button.SetEquipped(false);
        }
        _ButtonsImages[skinID].sprite = choosedimage;
        BuySkinID = skinID;
        if (skinID == skinPlayerID)
        {
            btnEquipped.SetActive(true);
            btnMoney.SetActive(false);
            btnEquip.SetActive(false);
            _SkinButtons[skinID].SetEquipped(true);
        }
        else
        {
            _SkinButtons[skinPlayerID].SetEquipped(true);
            if (SkinData.instance.isActiveSkin(skinID)==false)
            {

                btnEquipped.SetActive(false);
                btnMoney.SetActive(true);
                btnEquip.SetActive(false);
                needMoneytext.text = _SkinButtons[skinID].needMoneyCount.ToString();
            }
            else
            {
                btnEquipped.SetActive(false);
                btnMoney.SetActive(false);
                btnEquip.SetActive(true);
            }
        }
    }
    
    public void Equip()
    {
        PlayerData.instance.playerSkinID = BuySkinID;
        skinPlayerID = BuySkinID;
        SetScinChosed(BuySkinID);
    }

    public void ClosePanelSkin()
    {
        skinPlayerID = PlayerData.instance.playerSkinID;
        SetScinChosed(skinPlayerID);
    }

    public void BuyMoney()
    {
        if (PlayerData.instance.money >= _SkinButtons[BuySkinID].needMoneyCount)
        {
            PlayerData.instance.SetMinusMoney(_SkinButtons[BuySkinID].needMoneyCount);
            SkinData.instance.SetSkinActive(BuySkinID);
            PlayerData.instance.SetEquppedSkin(BuySkinID);
            skinPlayerID = BuySkinID;
            SetScinChosed(BuySkinID);
            _SkinButtons[BuySkinID].SetBuy();
            MainMenuUIController.instance.SetTextMoney();
        }else
        {
            MainMenuUIController.instance.OpenPanelNoMoney();
        }
    }
}
