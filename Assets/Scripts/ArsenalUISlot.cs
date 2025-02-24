using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArsenalUISlot : MonoBehaviour
{
    public int idBtn;
    public Image iconWeapon;
    public GameObject selected;
    public TMP_Text _ammoCountText;
    private ArsenalUI _arsenalUI;

    private void Start()
    {
        _arsenalUI = GetComponentInParent<ArsenalUI>();
    }

    public void SetAmmoCountText(string text)
    {
        _ammoCountText.text = text;
    }

    public void SetActiveWeapon()
    {
        _arsenalUI.SetActiveWeapon(idBtn);
    }

    public void SetIcon(Sprite sprite)
    {
        iconWeapon.sprite = sprite;
    }
    public void SetSelectedActive(bool isActive)
    {
        selected.SetActive(isActive);
    }
}
