using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArsenalUI : MonoBehaviour
{
    public List<GameObject> _buttons = new List<GameObject>();
    void Start()
    {
        int lastid = -1;
        for (int i = 0; i < _buttons.Count; i++)
        {
            string currentType = WeaponConteiner.instance.wTypes[i];
            int wTypeID = PlayerData.instance.weaponsEquipped[i];
            if (wTypeID == -1)
            {
                _buttons[i].SetActive(false);
            }
            else
            {
                if (currentType != "melee")
                {
                    WeaponController.instance.SpawnWeapon(currentType, wTypeID,
                        _buttons[i].GetComponent<ArsenalUISlot>());
                }
                else
                {
                    WeaponController.instance.SpawnWeapon(currentType, wTypeID,
                        null);
                }

                lastid = i;
                _buttons[i].SetActive(true);
                _buttons[i].GetComponent<ArsenalUISlot>().SetIcon(WeaponConteiner.instance.GetIcon(currentType,wTypeID));
            }
        }

        if (lastid != -1)
        {
            SetActiveWeapon(lastid);
        }
    }

    public void SetActiveWeapon(int buttonID)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            if(i!=buttonID)
                _buttons[i].GetComponent<ArsenalUISlot>().SetSelectedActive(false);
            else
                _buttons[i].GetComponent<ArsenalUISlot>().SetSelectedActive(true);
        }
        WeaponController.instance.SetActiveWeapon(buttonID);
    }
}
