using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXSaw : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip sawIdle, sawAttak;

    private bool isAttak = true;
    public void PlaySawIdle()
    {
        if (isAttak)
        {
            aud.clip = sawIdle;
            aud.Play();
            isAttak = false;
        }
    }
    public void PlaySawAttak()
    {
        if (!isAttak)
        {
            aud.clip = sawAttak;
            aud.Play();
            isAttak = true;
        }
    }
}
