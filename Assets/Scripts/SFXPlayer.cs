using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    public AudioClip pistolNoAmmoClip, changeAmmoClip, damage, walk, jump;
    public AudioSource _sfxWalk;
    public List<AudioSource> _AudioSources = new List<AudioSource>();


    public void PlayWalk(bool isWalk)
    {
        if (isWalk)
        {
            if (!_sfxWalk.isPlaying)
            {
                _sfxWalk.clip = walk;
                _sfxWalk.Play();
            }
        }
        else
        {
            if (_sfxWalk.isPlaying)
            {
                _sfxWalk.Stop();
            }
        }
    }

    public void PlayJump()
    {
        PlayCustomClip(jump);
    }
    public void PlayNoAmmoShoot()
    {
        PlayCustomClip(pistolNoAmmoClip);
    }

    public void PlayCustomClip(AudioClip clip)
    {
        bool isPlay = false;
        for (int i = 0; i < _AudioSources.Count; i++)
        {
            if (_AudioSources[i].isPlaying == false)
            {
                _AudioSources[i].clip = clip;
                _AudioSources[i].Play();
                isPlay = true;
                break;
            }
        }

        if (!isPlay)
        {
            _AudioSources[0].clip = clip;
            _AudioSources[0].Play();
        }
    }
    
    public void PlayChangeAmmo()
    {
        PlayCustomClip(changeAmmoClip);
    }
    
    public void PlayDamage()
    {
       PlayCustomClip(damage);
    }
}
