using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Components")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    private Vector3 moveDirection;
    public Animator anim;
    public Transform[] patrolPoints;
    private int currentControlPoint;

    [Header("Enemy Type")]
    public bool shouldChasePlayer;
    public bool shouldRunAway;
    public bool shouldShoot;
    public bool shouldWander;
    public bool shouldPatrol;


    [Header("Enemy Stats")]
    public int health;
    public float moveSpeed;
    public float shootRange;
    public float rangeToChasePlayer;
    public float runAwayRange;
    public float fireRate;
    public float wanderLength, wanderPauseLength;
    private float wanderCounter, wanderPauseCounter;
    private Vector3 wanderDirection;

    [Header("Enemy FX")]

    public GameObject[] deathSplatters;
    public GameObject hitFX;
    public int deathSound;
    public int hurtSound;
    public int shootSFX;

    [Header("Enemy Shoot")]
    public GameObject bullet;
    public Transform firePoint;
    private float fireCounter;

    [Header("Drop Rates")]
    public bool shouldDropItems;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    void Start()
    {
        if(shouldWander)
        {
            wanderPauseCounter = Random.Range(wanderPauseLength * 0.75f, wanderPauseLength * 1.25f);
        }
    }

    void Update()
    {
        MoveEnemy();
        EnemyShoot();
    }

    public void MoveEnemy()
    {
        if(sr.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;
            if(shouldChasePlayer && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if(shouldWander)
                {
                    WanderRoom();
                }
                if(shouldPatrol)
                {
                    MoveToPatrol();
                }
            }
            if(shouldRunAway)
            {
                RunAwayFromPlayer();
            }
            
            moveDirection.Normalize();
            rb.velocity = moveDirection * moveSpeed;

            if(moveDirection != Vector3.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void EnemyShoot()
    {
        if(shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange && sr.isVisible)
        {
            fireCounter -= Time.deltaTime;
            if(fireCounter <= 0)
            {
                fireCounter = fireRate;
                Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                AudioManager.instance.PlaySFX(shootSFX);
            }
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySFX(hurtSound);
        Instantiate(hitFX, transform.position, transform.rotation);
        if(health <= 0)
        {
            Destroy(gameObject);
            AudioManager.instance.PlaySFX(deathSound);
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            int rotation = Random.Range(0, 4);
            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
            RNGDrop();
        }
    }

    public void RunAwayFromPlayer()
    {
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runAwayRange)
        {
            moveDirection = transform.position - PlayerController.instance.transform.position;
        }
    }

    public void WanderRoom()
    {
        if(wanderCounter > 0)
        {
            wanderCounter -= Time.deltaTime;

            //move enemy
            moveDirection = wanderDirection;

            if(wanderCounter <= 0)
            {
                wanderPauseCounter = Random.Range(wanderPauseLength * 0.75f, wanderPauseLength * 1.25f);
            }
        }
        if(wanderPauseCounter > 0)
        {
            wanderPauseCounter -= Time.deltaTime;
            if(wanderPauseCounter <= 0)
            {
                wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);
                wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            }
        }
    }

    public void MoveToPatrol()
    {
        moveDirection = patrolPoints[currentControlPoint].position - transform.position;
        if(Vector3.Distance(transform.position, patrolPoints[currentControlPoint].position) < 0.2f)
        {
            currentControlPoint++;
            if(currentControlPoint >= patrolPoints.Length)
            {
                currentControlPoint = 0;
            }
        }
    }

    public void RNGDrop() 
    {
        if (shouldDropItems)
        {
            float dropChance = Random.Range(0f, 100f);
            if (dropChance <= itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);
                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }
}
