using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponData : MonoBehaviour
{
    public static WeaponData instance;

    public string jsonString;
    void Awake()
    {
        instance = this;
        string filePath = Path.Combine(Application.persistentDataPath, "WeaponData.json");
        if (File.Exists(filePath))
        {
            jsonString = File.ReadAllText(filePath);
            Debug.Log("Weapon FILE EXIST--------------" + jsonString);
            //LoadInfo();
        }
        else
        {
            Debug.Log("Weapon FILE NO EXIST--------------");
            SeveFirst();
        }
    }

    void SeveFirst()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(SaveFirstAndroid());
#else
        string filePath = "";
        filePath = Path.Combine(Application.streamingAssetsPath , "WeaponDataStart.json");
        jsonString = File.ReadAllText(filePath);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "WeaponData.json"), jsonString);
        Debug.Log("Weapon FILE Writed--------------");
        //LoadInfo();
#endif
    }
    
    IEnumerator SaveFirstAndroid()
    {
        UnityWebRequest www = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/WeaponDataStart.json");
        yield return www.SendWebRequest();
        jsonString = www.downloadHandler.text;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "WeaponData.json"), jsonString);
        //LoadInfo();
        if (www.isNetworkError)
        {
            Debug.Log("Weapon Error: " + www.error);
        }
        else
        {
            Debug.Log("Weapon Received: " + www.downloadHandler.text);
        }
    }
    
    public bool isActiveWeapon(string wType,int wId)
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        return info["Weapons"][wType][wId]["isActive"];
    }

    public string GetNameWeapon(string wType,int wId)
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        return info["Weapons"][wType][wId]["name"];
    }

    public void SetWeaponActive(string wType, int wId)
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        info["Weapons"][wType][wId]["isActive"]=true;
        jsonString = info.ToString();
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "WeaponData.json"), jsonString);

        Debug.Log("------------Weapon INFO SAVED--------------");
    }
}
