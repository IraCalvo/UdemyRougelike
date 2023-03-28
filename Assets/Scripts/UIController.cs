using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;
    public GameObject deathScreen;
    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadeToBlack;
    public bool fadeOutBlack;
    public string newGameScene;
    public string mainMenuScene;

    public GameObject pauseMenu;
    public GameObject mapDisplay;
    public GameObject bigMapText;

    public Image currentGun;
    public TextMeshProUGUI gunText;
    public Slider bossHealthBar;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;
        
        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponUI;
        gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;
        if(CameraController.instance.isBossRoom == true)
        {
            mapDisplay.SetActive(false);
        }
    }

    void Update()
    {
        if(fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }
        if(fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnPause();
    }
}
