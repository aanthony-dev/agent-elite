using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    bool isPaused;
    private GameObject continueButton;
    private GameObject restartButton;
    private GameObject exitButton;

    void Start()
    {
        isPaused = false;

        continueButton = GameObject.Find("ContinueLevel");
        restartButton = GameObject.Find("RestartLevel");
        exitButton = GameObject.Find("ExitLevel");

        activateButtons(false);
    }

    void Update()
    {
        //reload
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;
                Time.timeScale = 0;
                activateButtons(true);
            }
            else
            {
                isPaused = false;
                Time.timeScale = 1;
                activateButtons(false);
            }
        }
    }

    private void activateButtons(bool status)
    {
        continueButton.SetActive(status);
        restartButton.SetActive(status);
        exitButton.SetActive(status);
    }

    //pause menu function to continue the level by unpausing
    public void continueLevel()
    {
        isPaused = false;
        Time.timeScale = 1;
        activateButtons(false);
    }

    //pause menu function to restart the level
    public void restartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.LogWarning("LEVEL RESTARTED");
        isPaused = false;
        Time.timeScale = 1;
        activateButtons(false);
    }

    //pause menu function to return to main menu
    public void exitLevel()
    {
        SceneManager.LoadScene(0); //loads main menu
    }
    
}
