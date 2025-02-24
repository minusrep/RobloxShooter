using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCount : MonoBehaviour
{
    public Color _whiteColor, _grayColor;
    public Transform container;
    private List<Image> _AmmoImages = new List<Image>();
    public int ammoCount;
    private int startAmmoCount;
    void Start()
    {
        //ammoCount = startAmmoCount = WeaponController.instance.GetAmmoCount();

        foreach (Transform child in container)
        {
            _AmmoImages.Add(child.GetComponent<Image>());
        }
        Debug.Log(startAmmoCount);
        for (int i = 0; i < _AmmoImages.Count; i++)
        {
            if (startAmmoCount - 1 < i)
            {
                _AmmoImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetShootedAmmo()
    {
        if (ammoCount > 0)
        {
            ammoCount -= 1;
            _AmmoImages[ammoCount].color = _grayColor;
        }
    }

    public void SetAllAmmo()
    {
        for (int i = 0; i < startAmmoCount; i++)
        {
            _AmmoImages[i].color = _whiteColor;
        }

        ammoCount = startAmmoCount;
    }
}
