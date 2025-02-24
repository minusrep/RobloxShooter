using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponConteiner : MonoBehaviour
{

    public static WeaponConteiner instance;
    
    public List<GameObject> _melee = new List<GameObject>();
    public List<GameObject> _pistol = new List<GameObject>();
    public List<GameObject> _pistolAuto = new List<GameObject>();
    public List<GameObject> _shutGun = new List<GameObject>();
    public List<GameObject> _aRifle = new List<GameObject>();
    public List<GameObject> _rifle = new List<GameObject>();
    public List<GameObject> _grenadeLauncher = new List<GameObject>();

    public List<string> wTypes = new List<string>{"melee","pistol","pPistol","shotgun","aRifle","rifle","grenadeLauncher"};
    
    public List<Sprite> weaponsIcon = new List<Sprite>();
    void Awake()
    {
        instance = this;
    }

    public Sprite GetIcon(string type, int id)
    {
        if (type == wTypes[0])
        {
            return weaponsIcon[id];
        }
        else if (type == wTypes[1])
        {
            int newId = _melee.Count + id;
            return weaponsIcon[newId];
        }
        else if (type == wTypes[2])
        {
            int newId = _melee.Count +_pistol.Count + id;
            return weaponsIcon[newId];
        }
        else if (type == wTypes[3])
        {
            int newId = _melee.Count+_pistol.Count+_pistolAuto.Count + id;
            return weaponsIcon[newId];
        }
        else if (type == wTypes[4])
        {
            int newId = _melee.Count+_pistol.Count +_pistolAuto.Count+_shutGun.Count+ id;
            return weaponsIcon[newId];
        }
        else if (type == wTypes[5])
        {
            int newId = _melee.Count+_pistol.Count +_pistolAuto.Count+_shutGun.Count+_aRifle.Count+ id;
            return weaponsIcon[newId];
        }
        else
        {
            int newId = _melee.Count+_pistol.Count +_pistolAuto.Count+_shutGun.Count+_aRifle.Count+_rifle.Count+ id;
            return weaponsIcon[newId];
        }
    }

    public int GetCount(string type)
    {
        if (type == wTypes[0])
        {
            return _melee.Count;
        }else if (type == wTypes[1])
        {
            return _pistol.Count;
        }else if (type == wTypes[2])
        {
            return _pistolAuto.Count;
        }else if (type == wTypes[3])
        {
            return _shutGun.Count;
        }else if (type == wTypes[4])
        {
            return _aRifle.Count;
        }else if (type == wTypes[5])
        {
            return _rifle.Count;
        }else
        {
            return _grenadeLauncher.Count;
        }
    }
    public GameObject GetWeapon(string type, int id)
    {
        if (type == wTypes[0])
        {
            return _melee[id];
        }else if (type == wTypes[1])
        {
            return _pistol[id];
        }else if (type == wTypes[2])
        {
            return _pistolAuto[id];
        }else if (type == wTypes[3])
        {
            return _shutGun[id];
        }else if (type == wTypes[4])
        {
            return _aRifle[id];
        }else if (type == wTypes[5])
        {
            return _rifle[id];
        }else
        {
            return _grenadeLauncher[id];
        }
    }
    
}
