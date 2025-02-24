using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopWeapons : MonoBehaviour
{
    public static ShopWeapons instance;
    [Serializable]
    public class Weapon
    {
        public Sprite weaponImage;
        public string weaponName;
        public float fireRate;
        public float damage;
        public float reload;
        public int capacity;
    }
    [SerializeField]
    public List<Weapon> WeaponsInfo = new List<Weapon>();

    public List<GameObject> _PanelsWeapons = new List<GameObject>();
    public List<WeaponShopTypesPanel> _WeaponShopTypesPanels = new List<WeaponShopTypesPanel>();
    public GameObject PanelWeaponInfo;
    public GameObject btnMoney, btnEquipped, btnEquip;
    public TMP_Text needMoneytext;

    public Image weaponImage;
    public TMP_Text weaponName,fireRateText, damageText, capacityText, reloadText;
    
    public int currentPanelActive;

    private string selectedWType;
    private int selectedWeaponID;
    private int selectedNeedMoney;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OpenPanel(0);
    }

    public void SetChosedWeapon(string type,int id, int needMoney,int offset)
    {
        selectedWType = type;
        selectedWeaponID = id;
        selectedNeedMoney = needMoney;
        SetWeaponChosed(offset+selectedWeaponID);
    }

    public void SetButtonsActiveEquipped()
    {
        btnEquipped.SetActive(true);
        btnMoney.SetActive(false);
        btnEquip.SetActive(false);
    }

    public void SetButtonsActiveBuy(string cost)
    {
        btnEquipped.SetActive(false);
        btnMoney.SetActive(true);
        btnEquip.SetActive(false);
        needMoneytext.text = cost;
    }
    
    public void SetButtonsActiveEquip()
    {
        btnEquipped.SetActive(false);
        btnMoney.SetActive(false);
        btnEquip.SetActive(true);
    }

    public void UnActiveButtons()
    {
        btnEquipped.SetActive(false);
        btnMoney.SetActive(false);
        btnEquip.SetActive(false);
    }
    public void OpenPanel(int id)
    {
        foreach (GameObject panelsWeapon in _PanelsWeapons)
        {
            panelsWeapon.SetActive(false);
        }
        _PanelsWeapons[id].SetActive(true);
        currentPanelActive = id;
        _WeaponShopTypesPanels[currentPanelActive].OpenPanel();
    }

    public void SetWeaponChosed(int weaponID)
    {
        PanelWeaponInfo.SetActive(true);
        weaponImage.sprite = WeaponsInfo[weaponID].weaponImage;
        weaponName.text = WeaponsInfo[weaponID].weaponName;
        float fireRate = 60f / WeaponsInfo[weaponID].fireRate;
        fireRateText.text = fireRate.ToString("F0");
        damageText.text = WeaponsInfo[weaponID].damage.ToString("F0");
        capacityText.text = WeaponsInfo[weaponID].capacity.ToString();
        reloadText.text = WeaponsInfo[weaponID].reload.ToString("F0")+"s";
    }
    
    public void Equip()
    {
        PlayerData.instance.SetEquppedWeapon(selectedWType,selectedWeaponID);
        _WeaponShopTypesPanels[currentPanelActive].SetWeaponChosedEquip(selectedWeaponID);
    }

    public void ClosePanel()
    {
        PanelWeaponInfo.SetActive(false);
    }

    public void OpenPanelWeaponInfo()
    {
        PanelWeaponInfo.SetActive(true);
    }
    
    public void BuyMoney()
    {
        if (PlayerData.instance.money >= selectedNeedMoney)
        {
            PlayerData.instance.SetMinusMoney(selectedNeedMoney);
            WeaponData.instance.SetWeaponActive(selectedWType,selectedWeaponID);
            PlayerData.instance.SetEquppedWeapon(selectedWType,selectedWeaponID);
            _WeaponShopTypesPanels[currentPanelActive].SetBuyed(selectedWeaponID);
            Equip();
            MainMenuUIController.instance.SetTextMoney();
        }
        else
        {
            MainMenuUIController.instance.OpenPanelNoMoney();
        }
    }
}
