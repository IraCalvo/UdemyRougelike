using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    public GameObject deletePanel;
    public CharacterSelector[] charactersToDeleteFromSave;

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DeleteSave()
    {
        deletePanel.SetActive(true);
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(false);
        foreach(CharacterSelector theChar in charactersToDeleteFromSave)
        {
            PlayerPrefs.SetInt(theChar.playerToSpawn.name, 0);
        }
    }

    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }
}
