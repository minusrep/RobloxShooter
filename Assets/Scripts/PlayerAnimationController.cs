using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;

public class PlayerAnimationController : MonoBehaviour
{
    public AnimancerComponent _animancer;

    [SerializeField] private AnimationClip _Idle, _walkForward, _walkBackward, _dead;


    public void PlayIdle()
    {
        _animancer.Play(_Idle,0.25f);
    }
    public void PlayWalkF()
    {
        _animancer.Play(_walkForward, 0.25f);
    }
    public void PlayWalkB()
    {
        _animancer.Play(_walkBackward, 0.25f);
    }

    public void PlayDead()
    {
        _animancer.Play(_dead, 0.25f);
    }
}
