using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool openWhenEnemiesCleared;
    public bool closeWhenEntered;
    public GameObject[] doors;
    public List<GameObject> enemies = new List<GameObject>();

    private bool roomActive;

    void Update()
    {
        if(enemies.Count > 0 && roomActive && openWhenEnemiesCleared) 
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            if(enemies.Count == 0)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(false);
                    closeWhenEntered = false;
                }
            }
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
