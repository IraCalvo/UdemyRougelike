using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public float moveSpeed;
    public Transform target;
    public Camera MainCamera;
    public Camera bigMapCamera;
    private bool bigMapActive;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if(!LevelManager.instance.isPaused)
        {
            bigMapActive = true;
            bigMapCamera.enabled = true;
            MainCamera.enabled = false;
            PlayerController.instance.canMove = false;
            Time.timeScale = 0f;
            UIController.instance.mapDisplay.SetActive(false);
            UIController.instance.bigMapText.SetActive(true);
        }
    }

    public void DeactivateBigMap()
    {
        if(!LevelManager.instance.isPaused)
        {
            bigMapActive = false;
            bigMapCamera.enabled = false;
            MainCamera.enabled = true;
            PlayerController.instance.canMove = true;
            Time.timeScale = 1f;
            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.bigMapText.SetActive(false);
        }
    }
}
