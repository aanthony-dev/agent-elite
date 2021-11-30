using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private float timerValue;
    private float timer;
    private float modIncrement;
    private float mod;
    private float tiltValue;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        timerValue = 0.05f;
        timer = timerValue;
        modIncrement = 0.1f;
        mod = modIncrement;
        tiltValue = 0.0f;
    }

    void Update()
    {
        cameraTilt();
    }

    //slowly tilts the camera to make things more interesting
    void cameraTilt()
    {
        if (playerMovement.getMoving())
        {
            Vector3 rotation = new Vector3(0, 0, tiltValue);
            this.transform.eulerAngles = rotation;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                tiltValue += mod;
                timer = timerValue;
            }
            if (transform.eulerAngles.z > 3.0f && transform.eulerAngles.z < 5.0f)
            {
                mod = -modIncrement;
            }
            else if (transform.eulerAngles.z < 357.0f && transform.eulerAngles.z > 350.0f)
            {
                mod = modIncrement;
            }
        }
    }
}

