using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    public static MainMenuUIController instance;
    
    public TMP_Text moneyText, dimondText;

    public CanvasGroup internetFailed, setName, bottomButtons, shop,
        freeCoins, noMoney, noWeaponEquipped;
    public TMP_InputField nickNameInput;
    public TMP_Text playerNameAndLevel;
    
    public CameraMainMenuPos _CameraMainMenuPos;
    
    public ShopSkin _ShopSkin;
    public ShopWeapons _ShopWeapons;
    public ShopConsumables _ShopConsumables;

    [HideInInspector]
    public bool isUIOpen;
    private void Awake()
    {
        instance = this;
    }
    private IEnumerator GetCCode()
    {
        string ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
        string uri = $"https://ipapi.co/{ip}/json/";
           

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            JSONObject info = (JSONObject)JSON.Parse(webRequest.downloadHandler.text);
            PlayerPrefs.SetString("Country",info[7]);
        }
    }

    void Start()
    {
        moneyText.text = PlayerData.instance.money.ToString();
        dimondText.text = PlayerData.instance.dimond.ToString();
        StartCoroutine(GetCCode());
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.FadeOff();
        }

        isUIOpen = false;
    }
    public void SetTextMoney()
    {
        moneyText.text = PlayerData.instance.money.ToString();
        dimondText.text = PlayerData.instance.dimond.ToString();
    }
    public void SetPlayerNameTextAndLevel(string name, int level)
    {
        playerNameAndLevel.text = name + "  Lvl." + level.ToString();
    }
    
    public void BTN_PLAY()
    {
        if (isCheckInternetState())
        {
            StartGame();
        }
        else
        {
            //panelInternetConnectionFailed.SetActive(true);
            internetFailed.DOFade(1f, 0.5f);
            internetFailed.blocksRaycasts = true;
            isUIOpen = true;
        }
    }

    public void OpenPanelNoMoney()
    {
        noMoney.DOFade(1f, 0.5f);
        noMoney.blocksRaycasts = true;
        isUIOpen = true;
    }
    
    public void ClosePanelNoMoney()
    {
        noMoney.DOFade(0f, 0.5f);
        noMoney.blocksRaycasts = false;
        isUIOpen = false;
    }
    public void OpenPanelShop()
    {
        //panelBotttomButtons.SetActive(false);
        bottomButtons.DOFade(0f, 0.5f);
        bottomButtons.blocksRaycasts = false;
        shop.DOFade(1f, 0.5f);
        shop.blocksRaycasts = true;
        //panelShop.SetActive(true);
        _CameraMainMenuPos.SetCameraPosShop();
        isUIOpen = true;
    }
    public void ClosePanelShop()
    {
        //panelBotttomButtons.SetActive(true);
        //panelShop.SetActive(false);
        bottomButtons.DOFade(1f, 0.5f);
        bottomButtons.blocksRaycasts = true;
        shop.DOFade(0f, 0.5f);
        shop.blocksRaycasts = false;
        _ShopSkin.ClosePanelSkin();
        _CameraMainMenuPos.SetCameraPosMenu();
        isUIOpen = false;
    }

    public void OpenPanelFreeCoins()
    {
        //panelFreeCoins.SetActive(true);
        freeCoins.DOFade(1f, 0.5f);
        freeCoins.blocksRaycasts = true;
        isUIOpen = true;
    }
    
    public void ClosePanelFreeCoins()
    {
        //panelFreeCoins.SetActive(false);
        freeCoins.DOFade(0f, 0.5f);
        freeCoins.blocksRaycasts = false;
        isUIOpen = false;
    }

    public void AddFreeCoins()
    {
        PlayerData.instance.money+= 100000;
        SetTextMoney();
    }

    public void CloseInternetFailedPanel()
    {
        //panelInternetConnectionFailed.SetActive(false);
        internetFailed.DOFade(0f, 0.5f);
        internetFailed.blocksRaycasts = false;
        isUIOpen = false;
    }


    public void OpenChangeNickName(string playerName)
    {
        //panelSetName.SetActive(true);
        setName.DOFade(1f, 0.5f);
        setName.blocksRaycasts = true;
        nickNameInput.text = playerName;
        isUIOpen = true;
    }

    public void OpenChangeNickNameBtn()
    {
        //panelSetName.SetActive(true);
        setName.DOFade(1f, 0.5f);
        setName.blocksRaycasts = true;
        nickNameInput.text = UserInfo.instance._PlayerName;
        isUIOpen = true;
    }
    public void SaveNickName()
    {
        UserInfo.instance.SaveName(nickNameInput.text);
        ClosePanelNickName();
    }

    public void ClosePanelNickName()
    {
        //panelSetName.SetActive(false);
        setName.DOFade(0f, 0.5f);
        setName.blocksRaycasts = false;
        isUIOpen = false;
    }


    bool isCheckInternetState()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void OpenPanelShopFirst()
    {
        //panelBotttomButtons.SetActive(false);
        bottomButtons.DOFade(0f, 0.5f);
        bottomButtons.blocksRaycasts = false;
        noWeaponEquipped.DOFade(0f, 0.5f);
        noWeaponEquipped.blocksRaycasts = false;
        shop.DOFade(1f, 0.5f);
        shop.blocksRaycasts = true;
        //panelShop.SetActive(true);
        _CameraMainMenuPos.SetCameraPosShop();
        isUIOpen = true;
    }

    public void CheckPanelToClose()
    {
        if (internetFailed.alpha != 0)
        {
            isUIOpen = true;
            return;
        }
        if (setName.alpha != 0)
        {
            isUIOpen = true;
            return;
        }
        if (shop.alpha != 0)
        {
            isUIOpen = true;
            return;
        }
        if (freeCoins.alpha != 0)
        {
            isUIOpen = true;
            return;
        }
        if (noMoney.alpha != 0)
        {
            isUIOpen = true;
            return;
        }
        if (noWeaponEquipped.alpha != 0)
        {
            isUIOpen = true;
            return;
        }

        isUIOpen = false;
    }
    
    public void StartGame()
    {
        if (PlayerData.instance.isNotEquippedWeapon())
        {
            noWeaponEquipped.DOFade(1f, 0.5f);
            noWeaponEquipped.blocksRaycasts = true;
            isUIOpen = true;
        }
        else
        {
            if (SceneLoader.instance != null)
            {
                SceneLoader.instance.LoadScene("GameMode1");
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
    }
}
