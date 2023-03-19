using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    public GameObject buyMessage;
    private bool inBuyZone;

    public bool isHealthItem;
    public bool isUpgradeHealthItem;
    public bool isWeapon;
    public int itemCost;
    public int shopHealAmount;
    public int shopHealthUpgradeAmount;

    public int buySFX;
    public int notEnoughMoneySFX;
    public Gun[] potentialGuns;
    private Gun theGun;
    public SpriteRenderer gunSpriteDisplay;
    public TextMeshProUGUI infoText;

    void Start() 
    {
        if(isWeapon)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];
            gunSpriteDisplay.sprite = theGun.shopDisplaySprite;
            infoText.text = theGun.weaponName + "\n - " + theGun.gunItemCost + " Gold - ";
            itemCost = theGun.gunItemCost;
        }
    }

    void Update()
    {
        if(inBuyZone)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(LevelManager.instance.currentCoins >= itemCost && isHealthItem)
                {
                    LevelManager.instance.SpendCoins(itemCost);
                    PlayerHealthController.instance.HealPlayer(shopHealAmount);
                    AudioManager.instance.PlaySFX(buySFX); 
                    gameObject.SetActive(false);
                    inBuyZone = false;
                }
                else
                {
                    AudioManager.instance.PlaySFX(notEnoughMoneySFX);
                }
                if(LevelManager.instance.currentCoins >= itemCost && isUpgradeHealthItem)
                {
                    LevelManager.instance.SpendCoins(itemCost);
                    PlayerHealthController.instance.IncreaseMaxHealth(shopHealthUpgradeAmount);
                    AudioManager.instance.PlaySFX(buySFX);
                    gameObject.SetActive(false);
                    inBuyZone = false;
                }
                else
                {
                    AudioManager.instance.PlaySFX(notEnoughMoneySFX);
                }
                if(LevelManager.instance.currentCoins >= itemCost && isWeapon)
                {
                    LevelManager.instance.SpendCoins(itemCost);

                    Gun gunClone = Instantiate(theGun);
                    gunClone.transform.parent = PlayerController.instance.gunArm;
                    gunClone.transform.position = PlayerController.instance.gunArm.position;
                    gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    gunClone.transform.localScale = Vector3.one;

                    PlayerController.instance.availableGuns.Add(gunClone);
                    PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                    PlayerController.instance.SwitchGun();

                    AudioManager.instance.PlaySFX(buySFX);
                    gameObject.SetActive(false);
                    inBuyZone = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) 
    {
        if(otherCollider.tag == "Player")
        {
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player")
        {
            buyMessage.SetActive(false);
           inBuyZone = false; 
        }
    }
}
