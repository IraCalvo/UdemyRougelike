using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCharacter : MonoBehaviour
{
    private bool canUnlock;
    public GameObject message;

    public CharacterSelector[] charSelects;
    private CharacterSelector playerToUnlock;

    public SpriteRenderer cagedSR;
    // Start is called before the first frame update
    void Start()
    {
        playerToUnlock = charSelects[Random.Range(0, charSelects.Length)];
        cagedSR.sprite = playerToUnlock.playerToSpawn.bodySR.sprite;
    }

    void Update()
    {
        if(canUnlock)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);
                Instantiate(playerToUnlock, transform.position, transform.rotation);
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
