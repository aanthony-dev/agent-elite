using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLogic : MonoBehaviour
{
    GameObject temp;
    // Start is called before the first frame update
    void Start()
    {
        temp = GameObject.Find("MissionComplete");
        temp.SetActive(false);
        int test = GameObject.FindGameObjectsWithTag("Enemy").Length;
        var enemyCount = GameObject.Find("EnemyCount").GetComponent<TMPro.TextMeshProUGUI>();
        enemyCount.text = "Enemies: " + test;
        StartCoroutine("enemyCountRoutine");

    }

    private void enemyCountCheck()
    {
        var test = GameObject.FindGameObjectsWithTag("Enemy").Length;
        var enemyCount = GameObject.Find("EnemyCount").GetComponent<TMPro.TextMeshProUGUI>();
        enemyCount.text = test + " Enemies";

        if (test == 0)
        {
            StopCoroutine("enemyCountRoutine");
            StartCoroutine("fadeOutRoutine");
            temp.SetActive(true);

            //SceneManager.LoadScene(0); //loads main menu
        }
    }

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

    private IEnumerator fadeOutRoutine()
    {
        float opacity = 0.0f;
        var fade = GameObject.Find("Fade").GetComponent<Image>();
        
        float delay = 0.05f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (opacity < 2)
        {
            fade.color = new Color(0, 0, 0, opacity);
            opacity += 0.05f;
            yield return wait;
        }

        
    }
}
