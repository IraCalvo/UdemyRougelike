using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private bool canSelect;
    public GameObject message;
    public PlayerController playerToSpawn;
    public bool needsToBeUnlocked;

    void Start()
    {
        if(needsToBeUnlocked)
        {
            if(PlayerPrefs.HasKey(playerToSpawn.name))
            {
                if(PlayerPrefs.GetInt(playerToSpawn.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(canSelect)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Vector3 playerPos = PlayerController.instance.transform.position;
                Destroy(PlayerController.instance.gameObject);
                PlayerController newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);
                PlayerController.instance = newPlayer;
                gameObject.SetActive(false);
                CameraController.instance.target = newPlayer.transform;

                CharacterSelectManager.instance.activePlayer = newPlayer;
                CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
                CharacterSelectManager.instance.activeCharSelect = this;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}
