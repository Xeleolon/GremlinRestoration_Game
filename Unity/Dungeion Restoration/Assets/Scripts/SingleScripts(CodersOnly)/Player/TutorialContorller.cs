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
        LevelManager.instance.PauseGame(true);
        
        PlayerMovements player = GameObject.FindWithTag("Player").GetComponent<PlayerMovements>();
        player.interactions.toolMax = numbertoolOn;
        player.interactions.hideDestory = hideDestory;

        if (hideIcon != null && hideIcon.activeSelf)
        {
            hideIcon.SetActive(false);
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
