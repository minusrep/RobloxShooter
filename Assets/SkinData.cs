using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class SkinData : MonoBehaviour
{
    public static SkinData instance;

    public string jsonString;
    void Awake()
    {
        instance = this;
        string filePath = Path.Combine(Application.persistentDataPath, "SkinData.json");
        if (File.Exists(filePath))
        {
            jsonString = File.ReadAllText(filePath);
            Debug.Log("SKIN FILE EXIST--------------" + jsonString);
            //LoadInfo();
        }
        else
        {
            Debug.Log("SKIN FILE NO EXIST--------------");
            SeveFirst();
        }
    }
    void SeveFirst()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        StartCoroutine(SaveFirstAndroid());
#else
        string filePath = "";
        filePath = Path.Combine(Application.streamingAssetsPath , "SkinDataStart.json");
        jsonString = File.ReadAllText(filePath);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "SkinData.json"), jsonString);
        Debug.Log("SKIN FILE Writed--------------");
        //LoadInfo();
#endif
    }
    
    IEnumerator SaveFirstAndroid()
    {
        UnityWebRequest www = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/SkinDataStart.json");
        yield return www.SendWebRequest();
        jsonString = www.downloadHandler.text;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "SkinData.json"), jsonString);
        //LoadInfo();
        if (www.isNetworkError)
        {
            Debug.Log("SKIN Error: " + www.error);
        }
        else
        {
            Debug.Log("SKIN Received: " + www.downloadHandler.text);
        }
    }

    public bool isActiveSkin(int id)
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        return info["Skins"][id]["isActive"];
    }

    public void SetSkinActive(int id)
    {
        JSONObject info = (JSONObject)JSON.Parse(jsonString);
        info["Skins"][id]["isActive"] = true;
        jsonString = info.ToString();
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "SkinData.json"), jsonString);

        Debug.Log("------------SKIN INFO SAVED--------------");
    }
}
