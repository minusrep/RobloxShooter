using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource _AudioSource;
    public void SetValue(float value)
    {
        _AudioSource.volume = value;
    }
}
