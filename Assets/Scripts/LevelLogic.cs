using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{
    private GameObject missionComplete;
    private GameObject pressSpace;
    private bool levelComplete;

    void Start()
    {
        Time.timeScale = 1;
    }

    private void Awake()
    {
        levelComplete = false;
        missionComplete = GameObject.Find("MissionComplete");
        missionComplete.SetActive(false);
        pressSpace = GameObject.Find("PressSpace");
        pressSpace.SetActive(false);

        StartCoroutine("enemyCountRoutine");
    }

    private void Update()
    {
        if (levelComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (SceneManager.GetActiveScene().buildIndex < 3)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load next level
                }
                else
                {
                    SceneManager.LoadScene(0); //completed last level, go back to main menu
                }
            }
        }
    }

    //check how many enemies are still alive
    private void enemyCountCheck()
    {
        var numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        var enemyCount = GameObject.Find("EnemyCount").GetComponent<TMPro.TextMeshProUGUI>();
        enemyCount.text = numEnemies + " Enemies";

        //no more enemies, level completed
        if (numEnemies == 0)
        {
            StopCoroutine("enemyCountRoutine");
            StartCoroutine("fadeOutRoutine");
            missionComplete.SetActive(true);
        }
    }

    //routine to check number of enemies
    private IEnumerator enemyCountRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            enemyCountCheck();
        }
    }

    //routine to run the level fade out sequence
    private IEnumerator fadeOutRoutine()
    {
        float opacity = 0.0f;
        var fade = GameObject.Find("Fade").GetComponent<Image>();

        levelComplete = true;

        float delay = 0.05f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (opacity < 1)
        {
            fade.color = new Color(0, 0, 0, opacity);
            opacity += 0.05f;
            yield return wait;
        } 
        pressSpace.SetActive(true);
    }
}
