using System.Collections;
using System.Collections.Generic;
using HQFPSWeapons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject levelPanel;
    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    public GameObject backBtn;

    public Slider soundSlider;
    public Slider musicSlider;

    public TMPro.TMP_Dropdown graphicsDropdown;

    void Start()
    {
        soundSlider.SetValueWithoutNotify(GlobalVolumeManager.Instance.GetSoundVol());
        musicSlider.SetValueWithoutNotify(GlobalVolumeManager.Instance.GetMusicVol());

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel",0));
        graphicsDropdown.SetValueWithoutNotify(PlayerPrefs.GetInt("QualityLevel",0));
    }
    

    public void ShowLevelPanel()
    {
        levelPanel.SetActive(true);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        backBtn.SetActive(true);
    }

    public void ShowCreditsPanel()
    {
        levelPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
        controlsPanel.SetActive(false);
        backBtn.SetActive(true);
    }

    public void ShowOptionsPanel()
    {
        levelPanel.SetActive(false);
        optionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        backBtn.SetActive(true);
    }

    public void ShowControlsPanel()
    {
        levelPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(true);
        backBtn.SetActive(true);
    }

    public void Back()
    {
        levelPanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        backBtn.SetActive(false);
    }

    public void SetSoundVolume()
    {
        GlobalVolumeManager.Instance.SetSoundVol(soundSlider.value);
    }

    public void SetMusicVolume()
    {
        GlobalVolumeManager.Instance.SetMusicVol(musicSlider.value);
    }

    public void UpdateGraphics()
    {
        PlayerPrefs.SetInt("QualityLevel",graphicsDropdown.value);
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenLevel(int no)
    {
        SceneManager.LoadSceneAsync(no,LoadSceneMode.Single);
    }
}
