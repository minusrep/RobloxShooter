using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    public CanvasGroup _visualCanvas;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void FadeOff()
    {
        _visualCanvas.DOFade(0f, 1f).OnComplete(() =>
        {
            _visualCanvas.blocksRaycasts = false;
            _visualCanvas.interactable = false;
        });
    }

    public void LoadScene(string sceneName)
    {
        _visualCanvas.blocksRaycasts = true;
        _visualCanvas.interactable = true;
        _visualCanvas.DOFade(1f, 1f).OnComplete(() =>
        {
            Load(sceneName);
        });
    }

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
   
}
