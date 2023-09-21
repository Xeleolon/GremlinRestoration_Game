using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialContorller : MonoBehaviour
{
    public InputAction tutorialButton;
    public GameObject tutorialStage1;
    public GameObject tutorialStage2;
    public int numbertoolOn = 2;
    public bool hideDestory = false;
    public GameObject hideIcon;
    public GameObject levelManager;
    public GameObject player;
    public bool started;
    private void OnEnable()
    {
        tutorialButton.Enable();
        tutorialButton.performed += EnableCompletion;
    }

    private void OnDisable()
    {
        tutorialButton.Disable();
    }
    void Start()
    {
        //LevelManager.instance.PauseGame(true);

        levelManager.GetComponent<LevelManager>().PauseGame(true);

        Dialogue dialogue = new Dialogue("Tutorial Controller", "tutorial started", 0);
        DebugController.instance.AddLog(dialogue);
        
        PlayerMovements playerScript = player.GetComponent<PlayerMovements>();
        playerScript.interactions.toolMax = numbertoolOn;
        playerScript.interactions.hideDestory = hideDestory;

        if (hideIcon != null && hideIcon.activeSelf)
        {
            hideIcon.SetActive(false);
        }
        //LevelManager.instance.PauseGame(true);

        /*GameObject[] findLevelmanagers = GameObject.FindGameObjectsWithTag("LevelManager");
        foreach (GameObject levelmanager in findLevelmanagers)
        {
            levelmanager.GetComponent<LevelManager>().PauseGame(true);
        }*/
        started = true;
    }
    void Update()
    {
        if (!started)
        {
            levelManager.GetComponent<LevelManager>().PauseGame(true);

            Dialogue dialogue = new Dialogue("Tutorial Controller", "tutorial started", 0);
            DebugController.instance.AddLog(dialogue);
        
            PlayerMovements playerScript = player.GetComponent<PlayerMovements>();
            playerScript.interactions.toolMax = numbertoolOn;
            playerScript.interactions.hideDestory = hideDestory;

            if (hideIcon != null && hideIcon.activeSelf)
            {
                hideIcon.SetActive(false);
            }
            started = true;
        }
    }

    void EnableCompletion(InputAction.CallbackContext context)
    {
        if (tutorialStage2 != null && !tutorialStage2.activeSelf)
        {
            tutorialStage2.SetActive(true);
        }

        if (tutorialStage1 != null && tutorialStage1.activeSelf)
        {
            tutorialStage1.SetActive(false);
        }
    }
    public void TutorialComplete()
    {
        LevelManager.instance.PauseGame(false);
        Destroy(gameObject);
    }

}
