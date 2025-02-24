using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopChother : MonoBehaviour
{
    public List<GameObject> _panels = new List<GameObject>();
    public List<Image> _btnImages = new List<Image>();

    public Sprite focusSprite, noFocusSprite;
    void Start()
    {
        SetChosePanel(0);
    }


    public void SetChosePanel(int id)
    {
        foreach (var panel in _panels)
        {
            panel.SetActive(false);
        }

        foreach (var btnImage in _btnImages)
        {
            btnImage.sprite = noFocusSprite;
        }
        _panels[id].SetActive(true);
        _btnImages[id].sprite = focusSprite;
        MainMenuUIController.instance._ShopSkin.ClosePanelSkin();
        MainMenuUIController.instance._ShopWeapons.ClosePanel();
        MainMenuUIController.instance._ShopConsumables.SetActiveInfo(false);
        if (id == 0)
        {
            MainMenuUIController.instance._ShopWeapons.OpenPanelWeaponInfo();
        }

        if (id == 1)
        {
            MainMenuUIController.instance._ShopConsumables.SetActiveInfo(true);
        }
    }
}
