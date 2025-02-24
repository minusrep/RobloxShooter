using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public string jsonString;

    public int money, dimond;
    public string playerName;
    public int playerIconID;
    public int playerLevel, playerLevelValue;
    public int allMatchCount, winMatchCount, defeatMatchCount, killsCount, deathCount;
    public List<int> weaponsEquipped = new List<int>();
    public int playerSkinID, playerHatID;
    public int firstAidCount, shieldCount, grenadeCount;

    private Coroutine waitCoroutine;
    private bool isCoroutineStarted;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        string filePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        Debug.Log(filePath);
        if (File.Exists(filePath))
        {
            jsonString = File.ReadAllText(filePath);
            Debug.Log("FILE EXIST--------------" + jsonString);
            LoadInfo();
        }
        else
        {
            Debug.Log("FILE NO EXIST--------------");
            SeveFirst();
        }
    }

    public void SeveFirst()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(SaveFirstAndroid());
#else
        string filePath = "";
        filePath = Path.Combine(Application.streamingAssetsPath , "PlayerDataStart.json");
        jsonString = File.ReadAllText(filePath);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerData.json"), jsonString);
        Debug.Log("FILE Writed--------------");
        LoadInfo();
#endif
    }
    
    IEnumerator SaveFirstAndroid()
    {
        UnityWebRequest www = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/PlayerDataStart.json");
        yield return www.SendWebRequest();
        jsonString = www.downloadHandler.text;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerData.json"), jsonString);
        LoadInfo();
        if (www.isNetworkError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Received: " + www.downloadHandler.text);
        }
    }
    
    void LoadInfo()
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        
        money = info["PlayerData"]["MainInfo"]["Money"];
        dimond = info["PlayerData"]["MainInfo"]["Diamond"];
        playerName = info["PlayerData"]["MainInfo"]["PlayerName"];
        playerIconID = info["PlayerData"]["MainInfo"]["Player_Icon_ID"];
        playerLevel = info["PlayerData"]["MainInfo"]["PlayerLevel"];
        playerLevelValue = info["PlayerData"]["MainInfo"]["PlayerLevelValue"];
        
        allMatchCount = info["PlayerData"]["Statistic"]["All_Match_Count"];
        winMatchCount = info["PlayerData"]["Statistic"]["Win_Match_Count"];
        defeatMatchCount = info["PlayerData"]["Statistic"]["Defeat_Match_Count"];
        killsCount = info["PlayerData"]["Statistic"]["Kills_Count"];
        deathCount = info["PlayerData"]["Statistic"]["Death_Count"];

        for (int i = 0; i < info["PlayerData"]["PlayerWeapon"].Count; i++)
        {
            if (info["PlayerData"]["PlayerWeapon"][i]["status_equipped"] == true)
            {
                weaponsEquipped.Add(info["PlayerData"]["PlayerWeapon"][i]["weapon_id"]);
            }
            else
            {
                weaponsEquipped.Add(-1);
            }
        }
        
        playerSkinID = info["PlayerData"]["PlayerSkin"]["Skin_id"];
        playerHatID = info["PlayerData"]["PlayerSkin"]["Hat_id"];
        
        firstAidCount = info["PlayerData"]["PlayerConsumables"]["First_Aid"];
        shieldCount = info["PlayerData"]["PlayerConsumables"]["Shield"];
        grenadeCount = info["PlayerData"]["PlayerConsumables"]["Grenade"];
        
        Debug.Log("------------INFO LOADED--------------");
    }
    
    public void SaveData()
    {
        JSONObject info = (JSONObject) JSON.Parse(jsonString);
        info["PlayerData"]["MainInfo"]["Money"]=money;
        info["PlayerData"]["MainInfo"]["Diamond"]=dimond;
        info["PlayerData"]["MainInfo"]["PlayerName"]=playerName;
        info["PlayerData"]["MainInfo"]["Player_Icon_ID"]=playerIconID;
        info["PlayerData"]["MainInfo"]["PlayerLevel"]=playerLevel;
        info["PlayerData"]["MainInfo"]["PlayerLevelValue"]=playerLevelValue;
        
        info["PlayerData"]["Statistic"]["All_Match_Count"]=allMatchCount;
        info["PlayerData"]["Statistic"]["Win_Match_Count"]=winMatchCount;
        info["PlayerData"]["Statistic"]["Defeat_Match_Count"]=defeatMatchCount;
        info["PlayerData"]["Statistic"]["Kills_Count"]=killsCount;
        info["PlayerData"]["Statistic"]["Death_Count"]=deathCount;

        for (int i = 0; i < info["PlayerData"]["PlayerWeapon"].Count; i++)
        {
            if (weaponsEquipped[i]!=-1)
            {
                info["PlayerData"]["PlayerWeapon"][i]["status_equipped"] = true;
                info["PlayerData"]["PlayerWeapon"][i]["weapon_id"] = weaponsEquipped[i];
            }
            else
            {
                info["PlayerData"]["PlayerWeapon"][i]["status_equipped"] = false;
                info["PlayerData"]["PlayerWeapon"][i]["weapon_id"] = -1;
            }
        }
        
        info["PlayerData"]["PlayerSkin"]["Skin_id"]=playerSkinID;
        info["PlayerData"]["PlayerSkin"]["Hat_id"]=playerHatID;

        info["PlayerData"]["PlayerConsumables"]["First_Aid"] = firstAidCount;
        info["PlayerData"]["PlayerConsumables"]["Shield"]=shieldCount;
        info["PlayerData"]["PlayerConsumables"]["Grenade"]=grenadeCount;
        
        jsonString = info.ToString();
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerData.json"), jsonString);

        Debug.Log("------------INFO SAVED--------------");
    }

    IEnumerator WaitPerSave()
    {
        isCoroutineStarted = true;
        yield return new WaitForSeconds(1f);
        SaveData();
        isCoroutineStarted = false;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
    }

    public void SetMinusMoney(int count)
    {
        money -= count;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }

        if (MainMenuUIController.instance != null)
        {
            MainMenuUIController.instance.SetTextMoney();
        }
    }
    
    public void SetPlusMoney(int count)
    {
        money += count;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
        if (MainMenuUIController.instance != null)
        {
            MainMenuUIController.instance.SetTextMoney();
        }
    }

    public void SetPlusDimond(int count)
    {
        dimond += count;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
        if (MainMenuUIController.instance != null)
        {
            MainMenuUIController.instance.SetTextMoney();
        }
    }
    
    public void SetMinusDimond(int count)
    {
        dimond -= count;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
        if (MainMenuUIController.instance != null)
        {
            MainMenuUIController.instance.SetTextMoney();
        }
    }

    public void SetEquppedWeapon(string wType, int wId)
    {
        weaponsEquipped[GetWeaponTypeID(wType)] = wId;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
    }

    public bool isNotEquippedWeapon()
    {
        bool isNotEquipped = true;
        for (int i = 0; i < weaponsEquipped.Count; i++)
        {
            if (weaponsEquipped[i] != -1)
            {
                isNotEquipped = false;
            }
        }

        return isNotEquipped;
    }

    public int GetEquippedWeapon(string wType)
    {
        return weaponsEquipped[GetWeaponTypeID(wType)];
    }
    public void SetEquppedSkin(int skinId)
    {
        playerSkinID = skinId;
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
    }

    public void SetConsumable(string itemName, int count)
    {
        if (itemName == "firstaid")
        {
            firstAidCount += count;
        }else if (itemName == "armor")
        {
            shieldCount += count;
        }
        else
        {
            grenadeCount += count;
        }
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
    }

    public int GetConsumable(string itemName)
    {
        if (itemName == "firstaid")
        {
            return firstAidCount;
        }else if (itemName == "armor")
        {
            return shieldCount;
        }
        else
        {
            return grenadeCount;
        }
    }
    public void SetConsumableMinus(string itemName, int count)
    {
        if (itemName == "firstaid")
        {
            firstAidCount -= count;
        }else if (itemName == "armor")
        {
            shieldCount -= count;
        }
        else
        {
            grenadeCount -= count;
        }
        if (!isCoroutineStarted)
        {
            waitCoroutine = StartCoroutine(WaitPerSave());
        }
    }
    int GetWeaponTypeID(string wType)
    {
        if (wType == "melee")
        {
            return 0;
        }else if (wType == "pistol")
        {
            return 1;
        }else if (wType == "pPistol")
        {
            return 2;
        }else if (wType == "shotgun")
        {
            return 3;
        }else if (wType == "aRifle")
        {
            return 4;
        }else if (wType == "rifle")
        {
            return 5;
        }
        else
        {
            return 6;
        }
    }
   
}
