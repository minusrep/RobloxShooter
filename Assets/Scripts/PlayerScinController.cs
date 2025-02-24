using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerScinController : MonoBehaviour
{
    [Serializable]
    public class Skin
    {
        public int id;
        public Texture scinImage;
        public bool isHairActive;
    }
    [SerializeField]
    public List<Skin> Scins = new List<Skin>();

    public GameObject Hair;
    public List<SkinnedMeshRenderer> _parts = new List<SkinnedMeshRenderer>();
    public Material _Material;
    public bool isPlayer;
   
    private void Start()
    {
        if (isPlayer)
        {
            SetScinAtID();
        }
        else
        {
            SetScinAI();
        }
    }

  
    public void SetScinAtID()
    {
        int scinID = PlayerPrefs.GetInt("ScinID", 0);
        if (Scins[scinID].isHairActive)
        {
            Hair.SetActive(true);
        }
        else
        {
            Hair.SetActive(false);
        }
        
        _Material.mainTexture = Scins[scinID].scinImage;
    }

    public void SetScinAI()
    {
        Material newMaterial = new Material(_Material);
        int scinID = Random.Range(0, Scins.Count);
        if (Scins[scinID].isHairActive)
        {
            Hair.SetActive(true);
        }
        else
        {
            Hair.SetActive(false);
        }
        newMaterial.mainTexture = Scins[scinID].scinImage;
        foreach (SkinnedMeshRenderer renderer in _parts)
        {
            renderer.material = newMaterial;
        }
    }
    
    public void SetScinAtID(int skinId)
    {
        if (Hair != null)
        {
            if (Scins[skinId].isHairActive)
            {
                Hair.SetActive(true);
            }
            else
            {
                Hair.SetActive(false);
            }
        }
        _Material.mainTexture = Scins[skinId].scinImage;
    }

}
