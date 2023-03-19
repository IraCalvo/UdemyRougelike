using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string weaponName;
    public Sprite weaponUI;
    public GameObject bulletToFire;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;
    public int shootSFX;
    public int gunItemCost;
    public Sprite shopDisplaySprite;


    void Update()
    {
        if(shotCounter > 0)
        {
            shotCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) && !LevelManager.instance.isPaused && PlayerController.instance.canMove)
            {
                Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                shotCounter = timeBetweenShots;
                AudioManager.instance.PlaySFX(shootSFX);
            }
        }

    }
}
