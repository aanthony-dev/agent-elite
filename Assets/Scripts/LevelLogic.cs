using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{
    private bool levelComplete;
    private GameObject missionComplete;
    private GameObject pressSpace;
    private GameObject missionBriefing;
    private GameObject description;


    void Start()
    {
        Time.timeScale = 0;
    }

    private void Awake()
    {
        levelComplete = false;

        missionComplete = GameObject.Find("MissionComplete");
        missionComplete.SetActive(false);
        pressSpace = GameObject.Find("PressSpace");
        pressSpace.SetActive(true);

        missionBriefing = GameObject.Find("MissionBriefing");
        missionBriefing.SetActive(true);
        description = GameObject.Find("Description");
        description.SetActive(true);

        var fade = GameObject.Find("Fade").GetComponent<Image>();
        fade.color = new Color(0, 0, 0, 0.9f);

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
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                missionBriefing.SetActive(false);
                description.SetActive(false);
                pressSpace.SetActive(false);
                var fade = GameObject.Find("Fade").GetComponent<Image>();
                fade.color = new Color(0, 0, 0, 0);

                Time.timeScale = 1;
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
