using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private float modIncrement;
    [SerializeField] private float timerVal;

    private PlayerMovement playerMovement;
    private float timer;
    private float mod;
    private float zVal;
    
    // Start is called before the first frame update
    void Start()
    {
        modIncrement = 0.1f;
        timerVal = 0.05f;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        timer = timerVal;
        mod = modIncrement;
        zVal = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        cameraTilt();
    }

    //slowly tilts the camera to make things more interesting
    void cameraTilt()
    {
        if (playerMovement.getMoving())
        {
            Vector3 rotation = new Vector3(0, 0, zVal);
            this.transform.eulerAngles = rotation;

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                zVal += mod;
                timer = timerVal;
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
