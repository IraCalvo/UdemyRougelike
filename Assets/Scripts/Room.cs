using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool closeWhenEntered;
    public GameObject[] doors;
    public GameObject mapHider;

    [HideInInspector]
    public bool roomActive;

    void Update()
    {

    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
            closeWhenEntered = false;
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);
            roomActive = true;
            if(closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            mapHider.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            roomActive = false;
        }
    }
}
