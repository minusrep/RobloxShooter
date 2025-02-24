using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyRewardItem : MonoBehaviour
{
    public GameObject cleared, focus;

    public enum rewardType
    {
        money,
        dimond,
        firstaid,
        grenade,
        weapon,
        skin
    }

    public rewardType RewardType;

    public int rewardCount;
    public string weaponType;
    public void SetCleared()
    {
        cleared.SetActive(true);
    }
    
    public void SetFocus()
    {
        if (focus != null)
        {
            focus.SetActive(true);
        }
    }

    public void SetClaim()
    {
        if (RewardType == rewardType.money)
        {
            PlayerData.instance.SetPlusMoney(rewardCount);
        }
        if (RewardType == rewardType.dimond)
        {
            PlayerData.instance.SetPlusDimond(rewardCount);
        }
        if (RewardType == rewardType.firstaid)
        {
            PlayerData.instance.SetConsumable("firstaid",rewardCount);
        }
        if (RewardType == rewardType.grenade)
        {
            PlayerData.instance.SetConsumable("grenade",rewardCount);
        }
        if (RewardType == rewardType.weapon)
        {
            WeaponData.instance.SetWeaponActive(weaponType,rewardCount);
        }
        if (RewardType == rewardType.skin)
        {
            SkinData.instance.SetSkinActive(rewardCount);
        }
    }
}
