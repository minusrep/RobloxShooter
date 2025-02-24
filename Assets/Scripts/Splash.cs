using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public GameObject isOk, panelGdpr, panelLoading;
    public bool isChek;
    void Start()
    {
        if (PlayerPrefs.GetInt("GDPR", 0) == 0)
        {
            panelLoading.SetActive(false);
            panelGdpr.SetActive(true);
            StartCoroutine(WaitPerStart());
        }
        else
        {
            panelLoading.SetActive(true);
            panelGdpr.SetActive(false);
            isChek = true;
            StartCoroutine(WaitPerStart());
        }
        SceneLoader.instance.FadeOff();
    }

    public void SetCheck()
    {
        isOk.SetActive(true);
        PlayerPrefs.SetInt("GDPR", 1);
        panelLoading.SetActive(true);
        panelGdpr.SetActive(false);
        isChek = true;
    }

    IEnumerator WaitPerStart()
    {
        yield return new WaitWhile(() => isChek == false);

        yield return new WaitForSeconds(2f);

        SceneLoader.instance.LoadScene("MainMenu");
    }
}
