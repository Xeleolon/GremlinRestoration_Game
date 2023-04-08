using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
    }
    #endregion
    ////////////////////////
    [Header("Menu Systems")]
    public string nextLevel;
    public string MenuName;
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject victoryCanvas;
    [SerializeField] GameObject deathCanvas;
    PlayerMovements playerScript;
    ///////////////////////////
    [Header("Level Functions")]
    private int numLevels = 0;
    [Range(0, 2)]
    public int currentLevel;
    ///////////////////////////
    [Header("Player Ui System")]
    public bool changeScene = false;
    [Tooltip("0, for off, 4 for on, 1,2,3 for interactState")]
    [Range(0, 4)]
    public int testOnColors = 0;
    public Vector3 onScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 offScale = new Vector3(0.6f, 0.6f, 0.6f);
    [SerializeField] GameObject repairIcon;
    [SerializeField] Color repairOnColor;
    [SerializeField] Color repairOffColor;
    [SerializeField] GameObject destoryIcon;
    [SerializeField] Color destoryOnColor;
    [SerializeField] Color destoryOffColor;
    [SerializeField] GameObject restockIcon;
    [SerializeField] Color restockOnColor;
    [SerializeField] Color restockOffColor;
    void OnValidate() //only calls if change when script reloads or change in value
    {
        if (changeScene)
        {
        ChangeInteractUI(testOnColors);
        //Debug.Log(restockIcon.name + repairIcon.name + destoryIcon.name);
        //changeScene = false;
        }
    }
    void Start()
    {
        //ChangeInteractUI(0);
        Cursor.lockState = CursorLockMode.Locked;
        if (menuCanvas.activeSelf)
        {
            menuCanvas.SetActive(false);
        }
        if (victoryCanvas.activeSelf)
        {
            victoryCanvas.SetActive(false);
        }
        if (deathCanvas.activeSelf)
        {
            deathCanvas.SetActive(false);
        }
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();
        //Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && menuCanvas != null)
        {
            if (menuCanvas.activeSelf)
            {
                ExitMenu();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                playerScript.interactActive = false;
                menuCanvas.SetActive(true);
            }
        }
    }
    public int ChangeNumLevels(bool newLevel)
    {

        if (newLevel)
        {
            numLevels += 1;
            return numLevels;
        }
        return 0;
    }
    #region InteractUI Color Change Function
    public void ChangeInteractUI(int newState)
    {
        switch (newState)
        {
            case 0:
            repairIcon.GetComponent<RawImage>().color = repairOffColor;
            repairIcon.transform.localScale = offScale;
            destoryIcon.GetComponent<RawImage>().color = destoryOffColor;
            destoryIcon.transform.localScale = offScale;
            restockIcon.GetComponent<RawImage>().color = restockOffColor;
            restockIcon.transform.localScale = offScale;
            break;
            case 1:
            repairIcon.GetComponent<RawImage>().color = repairOnColor;
            repairIcon.transform.localScale = onScale;
            //turn off color not on
            destoryIcon.GetComponent<RawImage>().color = destoryOffColor;
            destoryIcon.transform.localScale = offScale;
            restockIcon.GetComponent<RawImage>().color = restockOffColor;
            restockIcon.transform.localScale = offScale;
            break;
            case 2:
            destoryIcon.GetComponent<RawImage>().color = destoryOnColor;
            destoryIcon.transform.localScale = onScale;
            //turn off color not on
            repairIcon.GetComponent<RawImage>().color = repairOffColor;
            repairIcon.transform.localScale = offScale;
            restockIcon.GetComponent<RawImage>().color = restockOffColor;
            restockIcon.transform.localScale = offScale;
            break;
            case 3:
            restockIcon.GetComponent<RawImage>().color = restockOnColor;
            restockIcon.transform.localScale = onScale;
            //turn off color not on
            repairIcon.GetComponent<RawImage>().color = repairOffColor;
            repairIcon.transform.localScale = offScale;
            destoryIcon.GetComponent<RawImage>().color = destoryOffColor;
            destoryIcon.transform.localScale = offScale;
            break;
            case 4:
            repairIcon.GetComponent<RawImage>().color = repairOnColor;
            repairIcon.transform.localScale = onScale;
            destoryIcon.GetComponent<RawImage>().color = destoryOnColor;
            destoryIcon.transform.localScale = onScale;
            restockIcon.GetComponent<RawImage>().color = restockOnColor;
            restockIcon.transform.localScale = onScale;            
            break;
        }
    }
    #endregion
    #region Menu Systems
    public void ExitMenu()
    {
        if (menuCanvas != null && menuCanvas.activeSelf)
        {
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.interactActive = true;
        menuCanvas.SetActive(false);
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
    public void DeathMenu(bool on)
    {
        if (on && !deathCanvas.activeSelf)
        {
            deathCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (!on && deathCanvas.activeSelf)
        {
            deathCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Victory()
    {
        victoryCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void NextLevel()
    {
        if (nextLevel != "")
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene(MenuName);
        }
    }
    #endregion

}
