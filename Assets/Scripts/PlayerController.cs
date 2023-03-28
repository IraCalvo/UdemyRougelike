using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength;
    public float dashInvincibility;
    public float dashCooldown;
    private Vector2 moveInput;

    public Rigidbody2D rb;
    public SpriteRenderer bodySR;
    public Animator anim;

    public Transform gunArm;
    Vector3 mousePos;
    Vector3 screenPoint;

    private float dashCooldownCounter;
    [HideInInspector]
    public float dashCounter;
    public int dashSFX;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    [HideInInspector]
    public int currentGun;


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        activeMoveSpeed = moveSpeed;
        UIController.instance.currentGun.sprite = availableGuns[currentGun].weaponUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    void Update()
    {
        MovePlayer();
        Dash();
        RotateGunArm();
        FlipCharacterSprites();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (availableGuns.Count > 0)
            {
                currentGun++;
                if (currentGun >= availableGuns.Count)
                {
                    currentGun = 0;
                }
                SwitchGun();
            }
            else
            {
                Debug.Log("No Guns!?");
            }
        }
    }

    public void MovePlayer()
    {
        if(canMove && !LevelManager.instance.isPaused)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            moveInput.Normalize();
        
            rb.velocity = moveInput * activeMoveSpeed;

            if(moveInput != Vector2.zero)
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
            anim.SetBool("isMoving", false);
        }
    }

    public void Dash()
    {
        if(canMove && !LevelManager.instance.isPaused)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(dashCooldownCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;
                    anim.SetTrigger("dash");
                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);
                    AudioManager.instance.PlaySFX(dashSFX);
                }
            }
            if(dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if(dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCooldownCounter = dashCooldown;
                }
            }
            if(dashCooldownCounter > 0)
            {
                dashCooldownCounter -= Time.deltaTime;
            }
        }
    }


    public void RotateGunArm()
    {
        if(!LevelManager.instance.isPaused)
        {
            mousePos = Input.mousePosition;
            screenPoint = CameraController.instance.MainCamera.WorldToScreenPoint(transform.localPosition);

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void FlipCharacterSprites()
    {
        if(mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            gunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            gunArm.localScale = Vector3.one;
        }
    }
    
    public void SwitchGun()
    {
        foreach(Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);
        Debug.Log("Gun was switched to be active");
        UIController.instance.currentGun.sprite = availableGuns[currentGun].weaponUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
