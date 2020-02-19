using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    public float gravity = -9.81f;

    Vector3 velocity;

    public GameObject pauseScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move*speed*Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.JoystickButton7))//Code for pause button - see https://www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/
        {
            if (Time.timeScale == 1) //Pauses
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                pauseScreen.SetActive(true);
                //showPaused(); 
            }
            else if (Time.timeScale == 0) // Unpauses
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                pauseScreen.SetActive(false);
                //hidePaused();
            }
        }
    }
}
