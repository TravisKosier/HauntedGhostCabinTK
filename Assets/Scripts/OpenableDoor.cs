using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour
{
    public Transform playerCamera;
    
    void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit raycastHit;
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton2)) //Controller support
        {
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == this.transform)
                {
                    transform.parent.gameObject.GetComponent<SpawnEnemy>().doorOpen = true;
                }
            }
        }
    }

    void OnMouseDown()
    {
        
    }
}
