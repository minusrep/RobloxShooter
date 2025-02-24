using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersConnectionField : MonoBehaviour
{
    public TMP_Text id, name, lvl;
    public Image country;
    public void SetName(string _id, string _name, string _lvl, Sprite _country)
    {
        id.text = _id;
        name.text = _name;
        lvl.text = _lvl;
        country.sprite = _country;
    }
}
