using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public float waitForAnyKeyText;
    public GameObject anyKeyText;
    public string mainMenuScene;

    void Start()
    {
        Time.timeScale = 1f;
        Destroy(PlayerController.instance.gameObject);
    }

    void Update()
    {
        if(waitForAnyKeyText > 0)
        {
            waitForAnyKeyText -= Time.deltaTime;
            if(waitForAnyKeyText <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
