using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    
    [System.Serializable] 
    public class MenuCanvas
    {
        //area for referneced canvas and gameobjects by the level manager to be kept so organised better
        public  GameObject menuCanvas;
        public  GameObject victoryCanvas;
        public  GameObject deathCanvas;
        public  GameObject replensihCanvas;
        [Tooltip("Place the target replenish inventory slot here")]
        public InventorySlot replenishTargetSlot;
        public GameObject inventoryCanvas;

        public Transform replenishInventory;
        public Item[] mobItems;
        public GameObject[] dragableItem;
        public GameObject titleCanvas;
        public TMP_Text titleText;
    }

    [System.Serializable]

    public class SharedPrefabs // a counter with all the sharedprefabs required like effects
    {
        [Header("Partical Effects")]
        public GameObject successPE;
        public GameObject destoryFailedPE;
        public GameObject repairFailedPE;

        public GameObject tokenPrefab;
    }
    [Header("Menu Systems")]
    [HideInInspector]public LevelData levelData;
    public bool freeze; //debug system only don't refenece
    private bool curFreeze = false; //debug system only don't refenece
    private bool gamePaused = false;
    [HideInInspector] public int pauseRequest;

    [SerializeField] MenuCanvas menuCanvas;
    private ReplenishInteract lastCustomer;
    PlayerMovements playerScript;
    GameObject player;
    

    [Header("Player Ui System")]
    public bool changeScene = false;
    [Tooltip("0, for off, 4 for on, 1,2,3 for interactState")]
    [Range(0, 4)]
    public int testOnColors = 0;
    public Vector3 onScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 offScale = new Vector3(0.6f, 0.6f, 0.6f);
    public InteractionsIcons repair;
    public InteractionsIcons destory;
    public InteractionsIcons bomb;

    public SharedPrefabs sharedPrefabs;


    private InputAction cancel;
    private Inventory inventory;

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
        player = GameObject.FindWithTag("Player");
        //ChangeInteractUI(0);
        Cursor.lockState = CursorLockMode.Locked;
        if (menuCanvas.menuCanvas.activeSelf)
        {
            menuCanvas.menuCanvas.SetActive(false);
        }
        if (menuCanvas.victoryCanvas.activeSelf)
        {
            menuCanvas.victoryCanvas.SetActive(false);
        }
        if (menuCanvas.deathCanvas.activeSelf)
        {
            menuCanvas.deathCanvas.SetActive(false);
        }
        SecondStart();
        //Cursor.lockState = CursorLockMode.None;
    }

    public void SecondStart()
    {
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();
        GameManager gameManager = GameManager.instance;
        int levelDataTicket = gameManager.FindLevelData(SceneManager.GetActiveScene().name);
        if (levelDataTicket < 0)
        {
            Debug.LogWarning("no Level Data With matching name to scene");
        }
        else
        {
            levelData = gameManager.levelData[levelDataTicket];
        }
        InteractControl interactScript = player.GetComponent<InteractControl>();
        interactScript.toolMax = levelData.numTools;

        if (levelData.numTools < 3)
        {
            switch (levelData.numTools)
            {
                case 0:
                repair.icon.transform.parent.gameObject.SetActive(false);
                destory.icon.transform.parent.gameObject.SetActive(false);
                bomb.icon.transform.parent.gameObject.SetActive(false);
                break;

                case 1:
                destory.icon.transform.parent.gameObject.SetActive(false);
                bomb.icon.transform.parent.gameObject.SetActive(false);
                break;

                case 2:
                bomb.icon.transform.parent.gameObject.SetActive(false);
                break;
            }
        }

        if (menuCanvas.titleCanvas != null)
        {
            if (!menuCanvas.titleCanvas.activeSelf)
            {
                menuCanvas.titleCanvas.SetActive(true);
            }
            PauseGame(true);
            if (levelData.levelName != "")
            {
                menuCanvas.titleText.SetText(levelData.levelName);
            }
            else
            {
                menuCanvas.titleText.SetText(levelData.name);
            }
        }


        inventory = Inventory.instance;
        inventory.StartInventory();
    }

    void Update()
    {
        if (playerScript == null)
        {
            playerScript = player.GetComponent<PlayerMovements>();
        }
        if (freeze && !curFreeze)
        {
            Debug.Log("Freazing game by update");
            Cursor.lockState = CursorLockMode.Confined;
            playerScript.interactActive = false;
            //DialogueManager.instance.freeze = true;
            curFreeze = freeze;
        }
        else if (!freeze && curFreeze)
        {
            Debug.Log("UNFreazing game by update");
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.interactActive = true;
            //DialogueManager.instance.freeze = false;
            curFreeze = freeze;
        }
    }
    void Cancel(InputAction.CallbackContext context)
    {
        OpenMenu();
    }

    public void OpenMenu()
    {
        if (menuCanvas.replensihCanvas != null && menuCanvas.replensihCanvas.activeSelf)
        {
            CloseReplenishUi();
        }
        else if (menuCanvas.titleCanvas != null && menuCanvas.titleCanvas.activeSelf)
        {
            PauseGame(false);
            menuCanvas.titleCanvas.SetActive(false);
        }
        else if (menuCanvas.menuCanvas != null)
        {
            if (!gamePaused &&menuCanvas.menuCanvas.activeSelf)
            {
                PauseGame(false);
                menuCanvas.menuCanvas.SetActive(false);
            }
            else
            {
                PauseGame(true);
                menuCanvas.menuCanvas.SetActive(true);
            }
        }
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

            bomb.icon.GetComponent<RawImage>().color = bomb.offColor;
            bomb.icon.transform.localScale = offScale;
            break;
            case 1:
            repair.icon.GetComponent<RawImage>().color = repair.onColor;
            repair.icon.transform.localScale = onScale;
            //turn off color not on
            destory.icon.GetComponent<RawImage>().color = destory.offColor;
            destory.icon.transform.localScale = offScale;

            bomb.icon.GetComponent<RawImage>().color = bomb.offColor;
            bomb.icon.transform.localScale = offScale;
            break;
            case 2:
            destory.icon.GetComponent<RawImage>().color = destory.onColor;
            destory.icon.transform.localScale = onScale;
            //turn off color not on
            repair.icon.GetComponent<RawImage>().color = repair.offColor;
            repair.icon.transform.localScale = offScale;

            bomb.icon.GetComponent<RawImage>().color = bomb.offColor;
            bomb.icon.transform.localScale = offScale;
            break;
            case 3:
            destory.icon.GetComponent<RawImage>().color = destory.offColor;
            destory.icon.transform.localScale = offScale;
            //turn off color not on
            repair.icon.GetComponent<RawImage>().color = repair.offColor;
            repair.icon.transform.localScale = offScale;
            bomb.icon.GetComponent<RawImage>().color = bomb.onColor;
            bomb.icon.transform.localScale = onScale; 


            break;
            case 4:
            repair.icon.GetComponent<RawImage>().color = repair.onColor;
            repair.icon.transform.localScale = onScale;
            destory.icon.GetComponent<RawImage>().color = destory.onColor;
            destory.icon.transform.localScale = onScale;
            bomb.icon.GetComponent<RawImage>().color = bomb.onColor;
            bomb.icon.transform.localScale = onScale;     
            break;
        }
    }

    public void UpdateItemNumber(int bombNum)
    {
        Slider bombSlider = bomb.icon.GetComponent<Slider>();
        if (bombNum < 0)
        {
            bombNum = 0;
        }
        
        if (bombNum > 10)
        {
            bombNum = 10;
        }
        bombSlider.value = -bombNum;
    }

    public void activeIcon(int num)
    {
        switch (num)
        {
            case 1:
            repair.icon.transform.parent.gameObject.SetActive(true);
            break;

            case 2:
            destory.icon.transform.parent.gameObject.SetActive(true);
            break;

            case 3:
            bomb.icon.transform.parent.gameObject.SetActive(true);
            break;
        }
    }
    #endregion
    #region Menu Systems

    public void LoadMenu()
    {
        {
            Debug.Log("Loading " + levelData.MenuName);
            LevelLoader.instance.LoadLevel(levelData.MenuName);
        }
    }


    public void ReloadScene()
    {
        LevelLoader.instance.ReloadLevel();
    }

    public void NextLevel()
    {
        if (levelData.nextLevel != "")
        {
            LevelLoader.instance.LoadLevel(levelData.nextLevel);
        }
        else
        {
            LevelLoader.instance.LoadLevel(levelData.MenuName);
        }
    }
    public void LastCheckPoint()
    {
        if (menuCanvas.menuCanvas != null)
        {
            if (menuCanvas.menuCanvas.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                playerScript.interactActive = true;
                menuCanvas.menuCanvas.SetActive(false);
            }
        }
        playerScript.MoveToCheckPoint();
        Dialogue dialogue = new Dialogue(gameObject.name, "Player Respawned", 0);
        DebugController.instance.AddLog(dialogue);
    }
    public void DeathMenu(bool on)
    {
        if (on && !menuCanvas.deathCanvas.activeSelf)
        {
            menuCanvas.deathCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (!on && menuCanvas.deathCanvas.activeSelf)
        {
            menuCanvas.deathCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Victory()
    {
        menuCanvas.victoryCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Debug.Log("pausing game");
            Dialogue dialogue = new Dialogue("LevelManager", "Pausing Game", 0);
            DebugController.instance.AddLog(dialogue);

            Cursor.lockState = CursorLockMode.Confined;
            playerScript.interactActive = false;
            pauseRequest += 1;
            gamePaused = true;
        }
        else if (pauseRequest <= 1)
        {
            Debug.Log("pausing game");
            Dialogue dialogue = new Dialogue("LevelManager", "unPausing Game", 0);
            DebugController.instance.AddLog(dialogue);
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.interactActive = true;
            pauseRequest = 0;
            gamePaused = false;
        }
        else
        {
            pauseRequest -= 1;
        }
    }

    #endregion
    #region ReplenishUI
    public void CloseReplenishUi()
    {
        if (menuCanvas.replensihCanvas != null && menuCanvas.replensihCanvas.activeSelf)
        {
            if (menuCanvas.inventoryCanvas != null && !menuCanvas.inventoryCanvas.activeSelf)
            {
                menuCanvas.inventoryCanvas.SetActive(true);
            }
            menuCanvas.replensihCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.interactActive = true;
        }
    }

    public void OpenReplenishUI(Item target, ReplenishInteract customer, bool requestMobs) //open the replenish ui and updates it to the current inventory UI layout
    {
        lastCustomer = customer; // set the desestion for the reply call to be made to

        menuCanvas.replenishTargetSlot.UpdateTarget(target, requestMobs); //sets the item desired by the target

        if (menuCanvas.inventoryCanvas != null && menuCanvas.inventoryCanvas.activeSelf)
        {
            menuCanvas.inventoryCanvas.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Confined;
        playerScript.interactActive = false;

        if (menuCanvas.replensihCanvas != null && !menuCanvas.replensihCanvas.activeSelf)
        {
            if (requestMobs) //true for requesting mode inventory type
            {
                if (menuCanvas.mobItems.Length > 0 && menuCanvas.replenishInventory.childCount > 0)
                {
                    for (int i = 0; i < menuCanvas.replenishInventory.childCount; i++)
                    {
                        if (menuCanvas.mobItems.Length > i)
                        {
                            RequestInventorySlot(menuCanvas.mobItems[i], i, -1);
                            
                            Debug.Log("Replenish Mob Hunt active");
                        }
                        else if (menuCanvas.dragableItem[i].activeSelf)
                        {
                            menuCanvas.dragableItem[i].SetActive(false);
                            menuCanvas.replenishInventory.GetChild(i).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(-1);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("Inventory slot active");
                if (inventory.items.Length > 0 && menuCanvas.replenishInventory.childCount > 0)
                {
                    int startPlace = 0;

                    bool checkTargetSpawned = false;

                    bool infinity = inventory.infiniteItems;
                    int availableSpace = 0;

                    for (int i = 0; i < menuCanvas.replenishInventory.childCount; i++)
                    {
                        
                        startPlace = inventory.checkNull(startPlace);
                        Debug.Log("start place = " + startPlace);

                        if (inventory.items.Length > startPlace && inventory.items[startPlace] != null)
                        {
                            RequestInventorySlot(inventory.items[startPlace], i, inventory.itemNumber[startPlace]);

                            if (infinity && target == inventory.items[startPlace])
                            {
                                checkTargetSpawned = true;
                            }
                            startPlace++;
                            availableSpace++;
                        }
                        else if (menuCanvas.dragableItem[i].activeSelf)
                        {
                            menuCanvas.dragableItem[i].SetActive(false);
                            menuCanvas.replenishInventory.GetChild(i).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(-1);
                        }
                    }

                    if (infinity && !checkTargetSpawned)
                    {
                        Debug.Log("start place = " + startPlace);
                        if (availableSpace < menuCanvas.replenishInventory.childCount && availableSpace >= 0)
                        {
                            RequestInventorySlot(target, availableSpace, 1);
                        }
                        else
                        {
                            RequestInventorySlot(target, 0, 1);
                        }
                    }
                }


            }
            menuCanvas.replensihCanvas.SetActive(true);
        }
    }

    void RequestInventorySlot(Item item, int menuPlace, int itemNum)
    {
        if (!menuCanvas.dragableItem[menuPlace].activeSelf)
        {
            menuCanvas.dragableItem[menuPlace].SetActive(true);
        }
            menuCanvas.dragableItem[menuPlace].GetComponent<DraggableItem>().NewItem(item, menuCanvas.replensihCanvas);
            menuCanvas.replenishInventory.GetChild(menuPlace).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(itemNum);
    }

    public void ReplenishReceipt(bool receipt)
    {
        if (menuCanvas.inventoryCanvas != null && !menuCanvas.inventoryCanvas.activeSelf)
        {
            menuCanvas.inventoryCanvas.SetActive(true);
        }
        menuCanvas.replensihCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerScript.interactActive = true;
        if (lastCustomer != null)
        {
            lastCustomer.Refill(receipt);
            lastCustomer = null;
        }
    }
    #endregion

}
