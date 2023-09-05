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

        public Transform replenishInventory;
        public Item[] mobItems;
        public GameObject[] dragableItem;
    }
    [Header("Menu Systems")]
    [SerializeField] LevelData levelData;

    [SerializeField] MenuCanvas menuCanvas;
    private ReplenishInteract lastCustomer;
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

    [SerializeField] private bool freeze;
    private bool curFreeze;

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
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovements>();

        if (levelData == null)
        {
            Debug.LogWarning("No Level Data Attached to LevelManager");
        }
        else if (levelData.name != SceneManager.GetActiveScene().name)
        {
            Debug.LogWarning("Level Data name (" + levelData.name + ") isn't matching up with scene name (" + SceneManager.GetActiveScene().name + ")");
        }

        inventory = Inventory.instance;
        //Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (freeze && !curFreeze)
        {
            Cursor.lockState = CursorLockMode.Confined;
            playerScript.interactActive = false;
            curFreeze = freeze;
        }
        else if (!freeze && curFreeze)
        {
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.interactActive = true;
            curFreeze = freeze;
        }
    }
    void Cancel(InputAction.CallbackContext context)
    {
        if (menuCanvas.replensihCanvas != null && menuCanvas.replensihCanvas.activeSelf)
        {
            CloseReplenishUi();
        }
        else if (menuCanvas.menuCanvas != null)
        {
            if (menuCanvas.menuCanvas.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                playerScript.interactActive = true;
                menuCanvas.menuCanvas.SetActive(false);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                playerScript.interactActive = false;
                menuCanvas.menuCanvas.SetActive(true);
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

    public void LoadMenu()
    {
        {
            Debug.Log("Loading " + levelData.MenuName);
            SceneManager.LoadScene(levelData.MenuName);
        }
    }


    public void ReloadScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }

    public void NextLevel()
    {
        if (levelData.nextLevel != "")
        {
            SceneManager.LoadScene(levelData.nextLevel);
        }
        else
        {
            SceneManager.LoadScene(levelData.MenuName);
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
        PlayerChat.instance.NewMessage("Player Respawned");
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
    public void CloseReplenishUi()
    {
        if (menuCanvas.replensihCanvas != null && menuCanvas.replensihCanvas.activeSelf)
        {
            menuCanvas.replensihCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            playerScript.interactActive = true;
        }
    }

    public void OpenReplenishUI(Item target, ReplenishInteract customer, bool requestMobs) //open the replenish ui and updates it to the current inventory UI layout
    {
        lastCustomer = customer; // set the desestion for the reply call to be made to

        menuCanvas.replenishTargetSlot.UpdateTarget(target); //sets the item desired by the target
    

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
                            if (!menuCanvas.dragableItem[i].activeSelf)
                            {
                                menuCanvas.dragableItem[i].SetActive(true);
                            }
                            menuCanvas.dragableItem[i].GetComponent<DraggableItem>().NewItem(menuCanvas.mobItems[i], menuCanvas.replensihCanvas);
                            menuCanvas.replenishInventory.GetChild(i).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(-1);
                            
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
                    for (int i = 0; i < menuCanvas.replenishInventory.childCount; i++)
                    {
                        
                        startPlace = inventory.checkNull(startPlace);

                        if (inventory.items.Length > startPlace && inventory.items[startPlace] != null)
                        {
                            if (!menuCanvas.dragableItem[i].activeSelf)
                            {
                                menuCanvas.dragableItem[i].SetActive(true);
                            }
                            menuCanvas.dragableItem[i].GetComponent<DraggableItem>().NewItem(inventory.items[startPlace], menuCanvas.replensihCanvas);
                            menuCanvas.replenishInventory.GetChild(i).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(inventory.itemNumber[startPlace]);

                            Debug.Log("Replenish Mob Hunt active");
                            startPlace++;
                        }
                        else if (menuCanvas.dragableItem[i].activeSelf)
                        {
                            menuCanvas.dragableItem[i].SetActive(false);
                            menuCanvas.replenishInventory.GetChild(i).gameObject.GetComponent<InventorySlot>().UpdateNumOnly(-1);
                        }
                    }
                }


            }

            Cursor.lockState = CursorLockMode.Confined;
            playerScript.interactActive = false;
            menuCanvas.replensihCanvas.SetActive(true);
        }
    }

    public void ReplenishReceipt(bool receipt)
    {
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
