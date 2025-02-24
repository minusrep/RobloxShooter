using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinItem : MonoBehaviour
{
    public TMP_Text nameText, killsText;

    public void SetText(string name, int kills)
    {
        nameText.text = name;
        killsText.text = kills.ToString();
    }
}
