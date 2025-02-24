using System;
using System.Collections;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;
    public bool isShoot;
    public FastIKFabric lHand, rHand;
    public RigBuilder weaponsRig;
    public Transform _weaponPose;
    public List<GameObject> weapons = new List<GameObject>();
    public SFXPlayer _SfxPlayer;
    public int currentWeaponID;


    private List<string> wTypes = new List<string>();
    private void Awake()
    {
        instance = this;

        StartCoroutine(WaitTimeCheckAmmo());
        currentWeaponID = -1;
    }

    IEnumerator WaitTimeCheckAmmo()
    {
        yield return new WaitForSeconds(1f);
        CheckWeaponAmmo();
    }
    public void SpawnWeapon(string type, int id,ArsenalUISlot uiSlot)
    {
        if (wTypes.Count == 0)
        {
            wTypes = WeaponConteiner.instance.wTypes;
        }
        GameObject weapon = Instantiate(WeaponConteiner.instance.GetWeapon(type, id), _weaponPose);
        for (int i = 0; i < wTypes.Count; i++)
        {
            if (type == wTypes[i])
            {
                weapons[i] = weapon;
            }
        }
        weapon.GetComponent<Weapon>()._soundFX = _SfxPlayer;
        weapon.GetComponent<Weapon>()._PlayerUISlot=uiSlot;
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

    public void AddAmmo()
    {
        for (int i = 1; i < weapons.Count; i++)
        {
            if (weapons[i]!=null)
            {
                weapons[i].GetComponent<Weapon>().AddAmmoCount();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon();
        }
    }

    void ChangeWeapon()
    {
        if (currentWeaponID < 40)
        {
            weapons[currentWeaponID].SetActive(false);
            currentWeaponID++;
            SetActiveWeapon();
            SetRig();
        }
        else
        {
            weapons[currentWeaponID].SetActive(false);
            currentWeaponID = 0;
            SetActiveWeapon();
            SetRig();
        }
    }

    public void SetActiveWeapon()
    {
        weaponsRig.layers[0].active = true;
        //weaponsRig.layers[currentWeaponID+1].active = true;
        weapons[currentWeaponID].SetActive(true);
        SetRig();
    }

    public void CheckWeaponAmmo()
    {
        for (int i = 1; i < weapons.Count; i++)
        {
            if (weapons[i]!=null)
            {
                weapons[i].GetComponent<Weapon>().CheckAmmoText();
            }
        }
    }
    public void SetActiveWeapon(int id)
    {
        if (currentWeaponID != -1)
        {
            if (weapons[currentWeaponID].activeSelf)
                weapons[currentWeaponID].SetActive(false);
        }

        currentWeaponID = id;
        weaponsRig.layers[0].active = true;
        //weaponsRig.layers[currentWeaponID+1].active = true;
        weapons[currentWeaponID].SetActive(true);
        SetRig();
    }

    public void SetDead()
    {
        StopShoot();
        weaponsRig.layers[0].active = false;
        //weaponsRig.layers[currentWeaponID+1].active = false;
        weapons[currentWeaponID].SetActive(false);
        rHand.enabled = false;
        lHand.enabled = false;
    }
    
    public void StartShoot(GameObject enemy)
    {
        isShoot = true;
        weapons[currentWeaponID].GetComponent<Weapon>().enemy = enemy;
        weapons[currentWeaponID].GetComponent<Weapon>().SetShootInController(isShoot);
    }
    public void StopShoot()
    {
        isShoot = false;
        if(currentWeaponID>-1)
            weapons[currentWeaponID].GetComponent<Weapon>().SetShootInController(isShoot);
    }
}
