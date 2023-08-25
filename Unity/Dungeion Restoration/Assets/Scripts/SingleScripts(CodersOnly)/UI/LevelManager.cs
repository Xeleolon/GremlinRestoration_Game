using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager instance;
    private PlayerInputActions playerControls;


    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
        playerControls = new PlayerInputActions();

    }
    #endregion
    ////////////////////////
    [System.Serializable]
    public class InteractionsIcons
    {
        public GameObject icon;
        public Color onColor;
        public Color offColor;
    }
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
    public InteractionsIcons repair;
    public InteractionsIcons destory;
    public InteractionsIcons restock;

    private InputAction cancel;

    #region Methods Before Start
    void OnValidate() //only calls if change when script reloads or change in value
    {
        if (changeScene)
        {
        ChangeInteractUI(testOnColors);
        //Debug.Log(restockIcon.name + repairIcon.name + destoryIcon.name);
        //changeScene = false;
        }
    }
    void OnEnable()
    {
        cancel = playerControls.UI.Cancel;
        cancel.Enable();
        cancel.performed += Cancel;
    }
    void OnDisable()
    {
        cancel.Disable();
    }
    #endregion
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
    void Cancel(InputAction.CallbackContext context)
    {
        /*if (menuCanvas != null)
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
        }*/
        Cursor.lockState = CursorLockMode.Confined;
        playerScript.interactActive = false;
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
            repair.icon.GetComponent<RawImage>().color = repair.offColor;
            repair.icon.transform.localScale = offScale;
            destory.icon.GetComponent<RawImage>().color = destory.offColor;
            destory.icon.transform.localScale = offScale;
            restock.icon.GetComponent<RawImage>().color = restock.offColor;
            restock.icon.transform.localScale = offScale;
            break;
            case 1:
            repair.icon.GetComponent<RawImage>().color = repair.onColor;
            repair.icon.transform.localScale = onScale;
            //turn off color not on
            destory.icon.GetComponent<RawImage>().color = destory.offColor;
            destory.icon.transform.localScale = offScale;
            restock.icon.GetComponent<RawImage>().color = restock.offColor;
            restock.icon.transform.localScale = offScale;
            break;
            case 2:
            destory.icon.GetComponent<RawImage>().color = destory.onColor;
            destory.icon.transform.localScale = onScale;
            //turn off color not on
            repair.icon.GetComponent<RawImage>().color = repair.offColor;
            repair.icon.transform.localScale = offScale;
            restock.icon.GetComponent<RawImage>().color = restock.offColor;
            restock.icon.transform.localScale = offScale;
            break;
            case 3:
            restock.icon.GetComponent<RawImage>().color = restock.onColor;
            restock.icon.transform.localScale = onScale;
            //turn off color not on
            repair.icon.GetComponent<RawImage>().color = repair.offColor;
            repair.icon.transform.localScale = offScale;
            destory.icon.GetComponent<RawImage>().color = destory.offColor;
            destory.icon.transform.localScale = offScale;
            break;
            case 4:
            repair.icon.GetComponent<RawImage>().color = repair.onColor;
            repair.icon.transform.localScale = onScale;
            destory.icon.GetComponent<RawImage>().color = destory.onColor;
            destory.icon.transform.localScale = onScale;
            restock.icon.GetComponent<RawImage>().color = restock.onColor;
            restock.icon.transform.localScale = onScale;            
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
