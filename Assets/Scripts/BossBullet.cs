using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 direction;
    public int impactSFX;

    void Start()
    {
        direction = transform.right;
    }

    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
        if(!BossController.instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
            Destroy(gameObject);
        }
        AudioManager.instance.PlaySFX(impactSFX);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
