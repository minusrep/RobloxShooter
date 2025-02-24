using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{

    public static UserInfo instance;
    public TMP_Text playerNameText, playerLevelText;
    public Slider _PlayerLevelSlider;
    [HideInInspector] public string _PlayerName;
    [HideInInspector] public int _PlayerLevel;
    [HideInInspector] public int _PlayerLevelValue;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _PlayerName = PlayerData.instance.playerName;
        _PlayerLevel = PlayerData.instance.playerLevel;
        _PlayerLevelValue = PlayerData.instance.playerLevelValue;
        _PlayerLevelSlider.value = _PlayerLevelValue;
        if ( _PlayerName == "User_0001")
        {
            MainMenuUIController.instance.OpenChangeNickName(_PlayerName);
        }

        SetNameLvl();
    }

    public void SaveName(string newPlayerName)
    {
        PlayerData.instance.SetPlayerName(newPlayerName);
        _PlayerName = newPlayerName;
        SetNameLvl();
    }

    void SetNameLvl()
    {
        MainMenuUIController.instance.SetPlayerNameTextAndLevel(_PlayerName, _PlayerLevel);
        playerNameText.text = _PlayerName;
        playerLevelText.text = "Lvl."+_PlayerLevel.ToString();
    }
}
