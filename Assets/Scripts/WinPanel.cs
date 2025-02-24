using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [Serializable]
    public class WinItemInfo
    {
        public string name;
        public int kills;
    }
    [SerializeField]
    public List<WinItemInfo> playersinfo = new List<WinItemInfo>();

    public List<WinItem> _winItems = new List<WinItem>();

    public void SetWinRate()
    {
        WinItemInfo info = new WinItemInfo();
        info.name = PlayerData.instance.playerName;
        info.kills = PlayerController.instance.playerKills;
        playersinfo.Add(info);
        foreach (var controller in LevelController.instance._AIControllers)
        {
            WinItemInfo info2 = new WinItemInfo();
            info2.name = controller.AIName;
            info2.kills = controller.playerKills;
            playersinfo.Add(info2);
        }

        playersinfo = playersinfo.OrderByDescending(x => x.kills).ToList();
        for (int i = 0; i < playersinfo.Count; i++)
        {
            _winItems[i].SetText(playersinfo[i].name,playersinfo[i].kills);
        }
    }
}
