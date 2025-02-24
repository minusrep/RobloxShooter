using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RandomNameAndCountry.Scripts;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Canvas mainCanvas, mapCanvas;
    public CanvasGroup PanelMenu;
    public GameObject PanelConnction, PanelWin, PanelDead, SecondFireButton;
    public Transform ConnectionParent;
    public GameObject ConnectionPrefab;
    public ProgressBar _playerHealthBar,_PlayerArmorBar;
    public GameObject _waitWaveTextGO;
    public TMP_Text _playerLevelText, _waveTimerText, _waveCountText;
    public TMP_Text _playersCountText, _enemysCountText;
    public TMP_Text _moneyWin, _expirientsWin;
    public bool isConectionSuccess;

    private int conectionCount;
    private bool isMapOpened;
    [HideInInspector]
    public int coinsCollectedInMap;

    public WinPanel winPanel;
    public PlayerDamageUI playerVisualDamage;
    public SettingsManager SettingsManager;
    private void Awake()
    {
        instance = this;
    }

    public void SetActiveSecondFireButton(int isActive)
    {
        if (isActive == 0)
        {
            SecondFireButton.SetActive(false);
        }
        else
        {
            SecondFireButton.SetActive(true);
        }
    }
    public void OpenBigMap()
    {
        if (isMapOpened)
        {
            mainCanvas.sortingOrder = 1;
            mapCanvas.sortingOrder = 0;
            isMapOpened = false;
        }
        else
        {
            mainCanvas.sortingOrder = 0;
            mapCanvas.sortingOrder = 1;
            isMapOpened = true;
        }
    }

    void Start()
    {
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.FadeOff();
        }
        if (!isConectionSuccess)
        {
            if (LevelController.instance != null)
            {
                PanelConnction.SetActive(true);
                conectionCount = 1;
                GameObject panel = Instantiate(ConnectionPrefab, ConnectionParent);
                panel.GetComponent<PlayersConnectionField>()
                    .SetName("#" + conectionCount, PlayerData.instance.playerName,
                        "lvl:" + PlayerData.instance.playerLevel,
                        RandomNameAndCountryPicker.Instance.GetCountrySprite(PlayerPrefs.GetString("Country", "none")));
                conectionCount++;
                float randT = Random.Range(1f, 2.5f);
                StartCoroutine(SetConnectionPlayer(randT));
            }
        }
    }

    public void OpenSettings()
    {
        SettingsManager.OpenSettings();
    }
    
    public void OpenPanelMenu()
    {
        PanelMenu.DOFade(1f, 0.5f);
        PanelMenu.blocksRaycasts = true;
    }
    
    public void ClosePanelMenu()
    {
        PanelMenu.DOFade(0f, 0.5f);
        PanelMenu.blocksRaycasts = false;
    }
    public void OpenPanelWin()
    {
        if (PanelWin.activeSelf == false)
        {
            PanelWin.SetActive(true);
            winPanel.SetWinRate();

            int winMoney = (int) (PlayerController.instance.playerKills * 25f) + coinsCollectedInMap;
            int winExpirience = (int) (PlayerController.instance.playerKills * 1.5f);
            PlayerData.instance.playerLevelValue+= winExpirience;
            _moneyWin.text = "+\n" + winMoney.ToString("F0");
            _expirientsWin.text = "+" + winExpirience.ToString("F0") + "xp";
            PlayerData.instance.SetPlusMoney(winMoney);
        }
    }
    public void OpenPanelDead()
    {
        PanelDead.SetActive(true);
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ButtonPlayerShoot(bool isShot)
    {
        PlayerController.instance.SetButtonShoot(isShot);
    }

    IEnumerator SetConnectionPlayer(float randTime)
    {
        if (conectionCount < 5)
        {
            yield return new WaitForSeconds(randTime);
            GameObject panel = Instantiate(ConnectionPrefab, ConnectionParent);
            RandomPlayerInfo info = RandomNameAndCountryPicker.Instance.GetRandomPlayerInfo();
            
            LevelController.instance._AIControllers[conectionCount-2].SetName(info.playerName);
            panel.GetComponent<PlayersConnectionField>().SetName("#" + conectionCount,
                info.playerName,"lvl:"+LevelController.instance._AIControllers[conectionCount-2].enemyLevel,info.countrySprite);
            conectionCount++;
            float randT = Random.Range(1f, 2.5f);
            StartCoroutine(SetConnectionPlayer(randT));
        }
        else
        {
            yield return new WaitForSeconds(randTime);
            isConectionSuccess = true;
            PanelConnction.SetActive(false);
            LevelController.instance.isWaveStart = true;
        }
    }

    public void SetPlayerLevelText(int level)
    {
        _playerLevelText.text = "Level " + level.ToString();
    }

    public void SetWaveTimerText(string text)
    {
        _waveTimerText.text = text;
    }
    public void SetHealth(float health)
    {
        _playerHealthBar.SetProgress(health);
    }
    public void SetArmor(float armor)
    {
        _PlayerArmorBar.SetProgress(armor);
    }

    public void SetEnemysCountInWave(int count)
    {
        _enemysCountText.text = count.ToString();
    }
    public void SetPlayersCountInWave(int count)
    {
        _playersCountText.text = count.ToString();
    }

    public void SetWaveCountText(int waveCount, int maxWaveCount)
    {
        _waveCountText.text ="Wawe Count \n"+ waveCount.ToString() + "/" + maxWaveCount.ToString();
    }

    public void SetWaveTimerState(bool isTimer, bool isKillAll = true)
    {
        if (isKillAll)
        {
            if (isTimer)
            {
                _waveCountText.gameObject.SetActive(false);
                _waveTimerText.gameObject.SetActive(true);
                _waitWaveTextGO.SetActive(true);
            }
            else
            {
                _waveCountText.gameObject.SetActive(true);
                _waveTimerText.gameObject.SetActive(false);
                _waitWaveTextGO.SetActive(false);
            }
        }
    }

    public void PlayerRevive()
    {
        PlayerController.instance.PlayerRevive();
        PanelDead.SetActive(false);
    }

    public void SetVisualDamage()
    {
        playerVisualDamage.SetDamage();
    }
}
