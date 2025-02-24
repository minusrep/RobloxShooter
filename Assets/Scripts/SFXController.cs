using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXController : MonoBehaviour
{
    public List<GameObject> _sfxGO = new List<GameObject>();
    void Awake()
    {
        _sfxGO = GameObject.FindGameObjectsWithTag("SFX").ToList();
    }

    public void SetValue(float value)
    {
        foreach (GameObject sfx in _sfxGO)
        {
            sfx.GetComponent<AudioSource>().volume = value;
        }
    }
    
}
