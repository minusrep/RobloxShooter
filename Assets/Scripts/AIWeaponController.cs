using System;
using System.Collections;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class AIWeaponController : MonoBehaviour
{
    public bool isShoot;
    
    public Transform _weaponPose;
    public LayerMask enemyLayer;
    public FastIKFabric lHand, rHand;
    public Transform enemy;
    public List<GameObject> weapons = new List<GameObject>();
    public SFXPlayer _SfxPlayer;
    public int currentWeaponID;
    private List<string> wTypes = new List<string>();
    
    
    public  void GetWeapon(int lvl)
    {
        if (wTypes.Count == 0)
        {
            wTypes = WeaponConteiner.instance.wTypes;
        }
        if (lvl >= 0 && lvl <= 3)
        {
            for (int i = 0; i < 2; i++)
            {
                int rId = Random.Range(0, 3);
                SpawnWeapon(wTypes[i],rId);
            }
        }
        if (lvl >= 4 && lvl <= 7)
        {
            for (int i = 0; i < 3; i++)
            {
                int rId = Random.Range(0, WeaponConteiner.instance.GetCount(wTypes[i]));
                SpawnWeapon(wTypes[i],rId);
            }
        }
        if (lvl >= 8 && lvl <= 12)
        {
            for (int i = 0; i < 4; i++)
            {
                int rId = Random.Range(0, WeaponConteiner.instance.GetCount(wTypes[i]));
                SpawnWeapon(wTypes[i],rId);
            }
        }
        if (lvl >= 13 && lvl <= 18)
        {
            for (int i = 0; i < 5; i++)
            {
                int rId = Random.Range(0, WeaponConteiner.instance.GetCount(wTypes[i]));
                SpawnWeapon(wTypes[i],rId);
            }
        }
        if (lvl >= 19 && lvl <= 30)
        {
            for (int i = 0; i < 6; i++)
            {
                int rId = Random.Range(0, WeaponConteiner.instance.GetCount(wTypes[i]));
                SpawnWeapon(wTypes[i],rId);
            }
        }
        if (lvl >= 31 && lvl <= 50)
        {
            for (int i = 0; i < 7; i++)
            {
                int rId = Random.Range(0, WeaponConteiner.instance.GetCount(wTypes[i]));
                SpawnWeapon(wTypes[i],rId);
            }
        }

        currentWeaponID = GetActiveVeaponID();
        SetActiveWeapon();
    }

    int GetActiveVeaponID()
    {
        int id = 0;
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
            {
                id = i;
            }
        }

        return id;
    }

    public void ChangeWeaponNoAmmo()
    {
        for (int i = weapons.Count-1; i > -1; i--)
        {
            if (weapons[i] != null)
            {
                if (!weapons[i].GetComponent<Weapon>().isAmmoEmpty())
                {
                    currentWeaponID = i;
                    SetActiveWeapon();
                    break;
                }
            }

            if (i == 0)
            {
                currentWeaponID = i;
                SetActiveWeapon();
            }
        }
    }
    public void SpawnWeapon(string type, int id)
    {
        GameObject weapon = Instantiate(WeaponConteiner.instance.GetWeapon(type, id), _weaponPose);
        for (int i = 0; i < wTypes.Count; i++)
        {
            if (type == wTypes[i])
            {
                weapons[i] = weapon;
            }
        }

        weapon.GetComponent<Weapon>().isAI = true;
        weapon.GetComponent<Weapon>()._soundFX = _SfxPlayer;
        weapon.GetComponent<Weapon>()._AIWeaponController = this;
    }
    public void SetRig()
    {
        if (weapons[currentWeaponID].GetComponent<Weapon>().lTarget != null)
        {
            lHand.Target = weapons[currentWeaponID].GetComponent<Weapon>().lTarget;
            lHand.enabled = true;
        }
        else
        {
            lHand.enabled = false;
        }
        
        if (weapons[currentWeaponID].GetComponent<Weapon>().rTarget != null)
        {
            rHand.Target = weapons[currentWeaponID].GetComponent<Weapon>().rTarget;
            rHand.enabled = true;
        }
        else
        {
            rHand.enabled = false;
        }
    }
    public void SetActiveWeapon()
    {
        weapons[currentWeaponID].SetActive(true);
        SetRig();
    }
    
    public void SetUnActiveWeapon()
    {
        weapons[currentWeaponID].SetActive(false);
        rHand.enabled = false;
        lHand.enabled = false;
    }

    public void StartShoot(GameObject enemy)
    {
        isShoot = true;
        weapons[currentWeaponID].GetComponent<Weapon>().enemy = enemy;
    }
    public void StopShoot()
    {
        isShoot = false;
    }
}
