using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManagerLevel : MonoBehaviour
{
    public Canvas startCanvas; 
    public Canvas optionCanvas; 
    public AudioSource audioSource; 

    public void StartGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("GameSetup");
    }

    public void MainMenu()
    {
        PlayClickSound();
        SceneManager.LoadScene("Main Menu");
    }

    public void BackToStart()
    {
        PlayClickSound();
        startCanvas.gameObject.SetActive(true);
        optionCanvas.gameObject.SetActive(false);
    }

    public void OptionMenu()
    {
        PlayClickSound();
        startCanvas.gameObject.SetActive(false);
        optionCanvas.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        PlayClickSound();
        Application.Quit();
    }

    public void GetBackStart()
    {
        PlayClickSound();
        SceneManager.LoadScene("Main Menu");
    }

    private void PlayClickSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
