using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string MenuName;
    [SerializeField] GameObject MenuCanvas;
    PlayerMovements playerScript;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (MenuCanvas.activeSelf)
        {
            MenuCanvas.SetActive(false);
        }
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();
        //Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && MenuCanvas != null)
        {
            if (MenuCanvas.activeSelf)
            {
                ExitMenu();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                playerScript.interactActive = false;
                MenuCanvas.SetActive(true);
            }
        }
    }
    public void ExitMenu()
    {
        if (MenuCanvas != null && MenuCanvas.activeSelf)
        {
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.interactActive = true;
        MenuCanvas.SetActive(false);
        }
    }

    public void LoadMenu()
    {
        {
            Debug.Log("Loading " + MenuName);
            SceneManager.LoadScene(MenuName);
        }
    }

    public void ReloadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
    public void LastCheckPoint()
    {
        ExitMenu();
        playerScript.MoveToCheckPoint();
        PlayerChat.instance.NewMessage("Player Respawned");
    }

}
