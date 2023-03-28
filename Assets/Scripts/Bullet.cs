using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public Rigidbody2D rb;
    public GameObject impactFX;
    public int damageToDeal;
    public int impactSFX;

    void Update()
    {
        rb.velocity = transform.right * bulletSpeed;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Instantiate(impactFX, transform.position, transform.rotation);
        Destroy(gameObject);
        AudioManager.instance.PlaySFX(impactSFX);

        if(otherCollider.tag == "Enemy")
        {
            otherCollider.GetComponent<EnemyController>().DamageEnemy(damageToDeal);
        }
        if(otherCollider.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToDeal);
            Instantiate(BossController.instance.hitFX, transform.position, transform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
