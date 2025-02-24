using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemConsumables : MonoBehaviour
{
    public string itemName;
    public int itemCount;
    public int itemCost;

    public TMP_Text costText;
  
    void Start()
    {
        if (costText != null)
        {
            costText.text = itemCost.ToString();
        }
    }

    public void BuyItemMoney()
    {

        if (PlayerData.instance.money >= itemCost)
        {
            PlayerData.instance.SetMinusMoney(itemCost);
            PlayerData.instance.SetConsumable(itemName,itemCount);
            MainMenuUIController.instance.SetTextMoney();
            MainMenuUIController.instance._ShopConsumables.AddConsumables(itemName,transform);
        }
        else
        {
            MainMenuUIController.instance.OpenPanelNoMoney();
        }
    }

    public void BuyItemAds()
    {
        PlayerData.instance.SetConsumable(itemName,itemCount);
    }
}
