using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    public int maxHealth;
    public int currentHealth;

    public static BossController instance;
    [NonReorderable]
    public BossAction[] actions;
    private int currentAction;
    private float actionCounter;
    private float shotCounter;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    public GameObject deathEffect, hitFX;
    public GameObject levelExit;

    [NonReorderable]
    public BossSequence[] sequences;
    public int currentSequence;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        actions = sequences[currentSequence].actions;

        actionCounter = actions[currentAction].actionLength;
        currentHealth = maxHealth;
        UIController.instance.bossHealthBar.maxValue = maxHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    void Update()
    {
        if(actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;
            moveDirection = Vector2.zero;
            if(actions[currentAction].shouldMove)
            {
                if(actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                }

                if(actions[currentAction].moveToPoints && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > 0.5f)
                {
                    Debug.Log("Goes into this if statement");
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                    moveDirection.Normalize();
                }
            }
            
            rb.velocity = moveDirection * actions[currentAction].moveSpeed;

            if(actions[currentAction].shouldShoot)
            {
                ShootPhase();
            }
        }
        else
        {
            currentAction++;
            if(currentAction >= actions.Length)
            {
                currentAction = 0;
            }
            actionCounter = actions[currentAction].actionLength;
        }
    }

    public void ShootPhase()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            shotCounter = actions[currentAction].timeBetweenShots;
            foreach (Transform t in actions[currentAction].shotPoints)
            {
                Instantiate(actions[currentAction].itemToShoot, t.position, t.rotation);
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            Instantiate(deathEffect, transform.position, transform.rotation);

            if(Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }
            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        else
        {
            if(currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            }
        }
        UIController.instance.bossHealthBar.value = currentHealth;
        Debug.Log(UIController.instance.bossHealthBar.value);
    }
}


[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool shouldMove;
    public bool shouldChasePlayer;
    public bool moveToPoints;
    public Transform pointToMoveTo;
    public float moveSpeed;

    public bool shouldShoot;
    public GameObject itemToShoot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    [NonReorderable]
    public BossAction[] actions;
    public int endSequenceHealth;
}
