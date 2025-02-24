using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
    public static DailyReward instance;

    public CanvasGroup dailyRewardGroup, claimPanel;
    public List<GameObject> daysCleared = new List<GameObject>();

    public List<DailyRewardItem> _Items = new List<DailyRewardItem>();
    
    public int currendDay;

    private int currentReward;
    private bool isClaim;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DateTime theDate = DateTime.Now;
        if (PlayerPrefs.GetInt("CurrentDay", -1) == -1)
        {
            PlayerPrefs.SetInt("CurrentDay", theDate.Day);
            currendDay = -1;
        }
        else
        {
            currendDay = PlayerPrefs.GetInt("CurrentDay", 1);
        }
        isClaim = false;
        currentReward = PlayerPrefs.GetInt("CurrentDayReward", 0);

        if (currendDay != theDate.Day)
        {
            if (currentReward <= 6)
            {
                SetActiveReward();
            }
        }
    }

    void SetActiveReward()
    {
        MainMenuUIController.instance.isUIOpen = true;
        dailyRewardGroup.DOFade(1f, 0.5f);
        dailyRewardGroup.blocksRaycasts = true;
        claimPanel.DOFade(1f, 0.5f);
        claimPanel.blocksRaycasts = true;
        daysCleared[currentReward].SetActive(true);
        for (int i = 0; i < _Items.Count; i++)
        {
            if (i <= currentReward)
            {
                _Items[i].SetCleared();
            }
        }

        if (currentReward < 6)
        {
            _Items[currentReward+1].SetFocus();
        }
    }

    public void Claim()
    {
        if (!isClaim)
        {
            _Items[currentReward].SetClaim();
            claimPanel.DOFade(0f, 0.5f);
            claimPanel.blocksRaycasts = false;
            DateTime theDate = DateTime.Now;
            PlayerPrefs.SetInt("CurrentDay", theDate.Day);
            PlayerPrefs.SetInt("CurrentDayReward", PlayerPrefs.GetInt("CurrentDayReward", 0) + 1);
            isClaim = true;
        }
    }

    public void CloseDailyReward()
    {
        MainMenuUIController.instance.CheckPanelToClose();
        dailyRewardGroup.DOFade(0f, 0.5f);
        dailyRewardGroup.blocksRaycasts = false;
    }
}
