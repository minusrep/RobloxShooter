using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public CanvasGroup panelSettings;
    public GameObject panelSound, panelControll, panelLanguage;
    public Image btnSound, btnControll, btnLanguage;
    public Sprite btnOn, btnOff;
    public Slider sfxSlider, musicSlider, sensivitySlider;
    public CustomToggle vibrationToggle, autoShoot, secondFireButton;
    void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("BGM", 1f);
        sensivitySlider.value = PlayerPrefs.GetFloat("Sensivity", 0.001f);
        int isVibro = PlayerPrefs.GetInt("isVibro", 1);
        int isAutoShoot = PlayerPrefs.GetInt("isAutoShoot", 1);
        int isSecondFireBtn = PlayerPrefs.GetInt("isSecondFireBtn", 1);
        vibrationToggle.SetToggle(isVibro);
        autoShoot.SetToggle(isAutoShoot);
        secondFireButton.SetToggle(isSecondFireBtn);
        if (GetComponent<SFXController>() != null)
        {
            GetComponent<SFXController>().SetValue(sfxSlider.value);
        }

        if (GetComponent<BGMController>() != null)
        {
            GetComponent<BGMController>().SetValue(musicSlider.value);
        }

        if (FindObjectOfType<CMF.CameraMouseInput>() != null)
        {
            CMF.CameraMouseInput input = FindObjectOfType<CMF.CameraMouseInput>();
            input.mouseInputMultiplier = sensivitySlider.value;
        }

        if (PlayerController.instance != null)
        {
            if (isAutoShoot == 0)
            {
                PlayerController.instance.isAIMMode = false;
            }
            else
            {
                PlayerController.instance.isAIMMode = true;
            }
        }
        
        if (UIController.instance != null)
        {
            UIController.instance.SetActiveSecondFireButton(isSecondFireBtn);
        }
        ActivePanel(0);
    }

    public void ActivePanel(int id)
    {
        if (id == 0)
        {
            panelSound.SetActive(true);
            panelControll.SetActive(false);
            panelLanguage.SetActive(false);

            btnSound.sprite = btnOn;
            btnControll.sprite = btnOff;
            btnLanguage.sprite = btnOff;
        }
        if (id == 1)
        {
            panelSound.SetActive(false);
            panelControll.SetActive(true);
            panelLanguage.SetActive(false);

            btnSound.sprite = btnOff;
            btnControll.sprite = btnOn;
            btnLanguage.sprite = btnOff;
        }
        if (id == 2)
        {
            panelSound.SetActive(false);
            panelControll.SetActive(false);
            panelLanguage.SetActive(true);

            btnSound.sprite = btnOff;
            btnControll.sprite = btnOff;
            btnLanguage.sprite = btnOn;
        }
    }

    public void SetSfxValue()
    {
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        if (GetComponent<SFXController>() != null)
        {
            GetComponent<SFXController>().SetValue(sfxSlider.value);
        }
    }
    
    public void SetBGMValue()
    {
        PlayerPrefs.SetFloat("BGM", musicSlider.value);
        if (GetComponent<BGMController>() != null)
        {
            GetComponent<BGMController>().SetValue(musicSlider.value);
        }
    }
    
    public void SetSensivityValue()
    {
        PlayerPrefs.SetFloat("Sensivity", sensivitySlider.value);
        if (FindObjectOfType<CMF.CameraMouseInput>() != null)
        {
            CMF.CameraMouseInput input = FindObjectOfType<CMF.CameraMouseInput>();
            input.mouseInputMultiplier = sensivitySlider.value;
        }
    }
    
    public void SetVibroValue()
    {
        if (vibrationToggle.isToggle)
        {
            PlayerPrefs.SetInt("isVibro", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isVibro", 0);
        }
    }

    public void SetAutoShoot()
    {
        if (autoShoot.isToggle)
        {
            PlayerPrefs.SetInt("isAutoShoot", 1);
            if (PlayerController.instance != null)
            {
                PlayerController.instance.isAIMMode = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt("isAutoShoot", 0);
            if (PlayerController.instance != null)
            {
                PlayerController.instance.isAIMMode = false;
            }
        }

        
    }
    
    public void SetSecondFireButton()
    {
        if (secondFireButton.isToggle)
        {
            PlayerPrefs.SetInt("isSecondFireBtn", 1);
            if (UIController.instance != null)
            {
                UIController.instance.SetActiveSecondFireButton(1);
            }
        }
        else
        {
            PlayerPrefs.SetInt("isSecondFireBtn", 0);
            if (UIController.instance != null)
            {
                UIController.instance.SetActiveSecondFireButton(0);
            }
        }
    }
    public void CloseSettings()
    {
        panelSettings.DOFade(0f, 0.5f);
        panelSettings.blocksRaycasts = false;
        if (MainMenuUIController.instance != null)
        {
            MainMenuUIController.instance.isUIOpen = false;
        }
    }
    
    public void OpenSettings()
    {
        if (MainMenuUIController.instance != null)
        {
            if (MainMenuUIController.instance.isUIOpen == false)
            {
                MainMenuUIController.instance.isUIOpen = true;
                panelSettings.DOFade(1f, 0.5f);
                panelSettings.blocksRaycasts = true;
            }
        }
        else
        {
            panelSettings.DOFade(1f, 0.5f);
            panelSettings.blocksRaycasts = true;
        }
    }
}
