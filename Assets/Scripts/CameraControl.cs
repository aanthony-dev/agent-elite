using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private Camera camera;
    private bool followPlayer = true;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //player can freely move camera while holding down right click
        if (Input.GetMouseButton(1))
        {
            followPlayer = false;
            playerMovement.setMoving(false);
        }
        else
        {
            followPlayer = true;
        }

        if (followPlayer == true)
        {
            follow();
        }
        else
        {
            freeLook();
        }
    }

    //camera follows the player
    void follow()
    {
        Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        this.transform.position = position;
    }

    //camera moves freely in accordance with mouse movement
    void freeLook()
    {
        Vector3 cameraPosition = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10));
        Vector3 direction = cameraPosition - this.transform.position;
        if (player.GetComponent<SpriteRenderer>().isVisible == true)
        {
            transform.Translate(direction * 2 * Time.deltaTime);
        }


    }
}
