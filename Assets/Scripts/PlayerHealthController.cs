using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth;
    public int maxHealth;

    public float invincibilityLength;
    private float invincibilityCounter;
    
    public int playerDieSFX;
    public int playerHurtSFX;

    void Awake()
    {
        instance = this;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    void Update()
    {
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;

            if(invincibilityCounter <= 0)
            {
                PlayerController.instance.sr.color = new Color(PlayerController.instance.sr.color.r, PlayerController.instance.sr.color.g, PlayerController.instance.sr.color.b,1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if(invincibilityCounter <= 0)
        {
            AudioManager.instance.PlaySFX(playerHurtSFX);
            currentHealth--;
            invincibilityCounter = invincibilityLength;
            PlayerController.instance.sr.color = new Color(PlayerController.instance.sr.color.r, PlayerController.instance.sr.color.g, PlayerController.instance.sr.color.b,0.5f);
            if(currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);
                UIController.instance.deathScreen.SetActive(true);
                AudioManager.instance.PlayGameOver();
                AudioManager.instance.PlaySFX(playerDieSFX);
            }
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }

    public void MakeInvincible(float length)
    {
        invincibilityCounter = length;
        PlayerController.instance.sr.color = new Color(PlayerController.instance.sr.color.r, PlayerController.instance.sr.color.g, PlayerController.instance.sr.color.b,0.5f);
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }
}
