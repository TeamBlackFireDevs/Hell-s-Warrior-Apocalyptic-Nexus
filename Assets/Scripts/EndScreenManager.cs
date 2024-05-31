using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    public GameObject endScreen;
	public TMPro.TMP_Text timeSurvivedText;

    public static EndScreenManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void ShowEndScreen(float timeSurvived)
    {
        endScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        int mins = (int)(timeSurvived / 60);
        int seconds = (int)(timeSurvived % 60);

        timeSurvivedText.text = mins + "m " + seconds + "s";

    }

    public void PlayAgain()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0,LoadSceneMode.Single);
    }
}
