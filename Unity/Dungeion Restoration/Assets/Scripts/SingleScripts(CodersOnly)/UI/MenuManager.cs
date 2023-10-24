using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region Singleton
    public static MenuManager instance;
    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
    }
    #endregion
    [SerializeField] private int numLevels = 5;
    private int activeLevels;
    public string testScene;
    private int currentScene = 0;
    public bool testSceneLock = true;
    [SerializeField] private GameObject settingCanvas;
    [SerializeField] private Slider settingSlider;
    public TMP_Text levelText;
    public TMP_InputField codeInput;
    [SerializeField] private GameObject increaseLevelButton1;
    [SerializeField] private GameObject increaseLevelButton2;
    [SerializeField] private GameObject decreaseLevelButton1;
    [SerializeField] private GameObject decreaseLevelButton2;

    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        activeLevels = gameManager.AccessUnlockedLevels(false);
        if (activeLevels > numLevels)
        {
            activeLevels = numLevels;
            Debug.Log("Number of levels locked in menu manager at " + numLevels);
        }
        if (numLevels == 0)
        {
            levelText.SetText("No Scenes");
        }
        else if (currentScene > numLevels + 1)
        {
            currentScene = numLevels;
        }

        LevelData currentLevel = gameManager.levelData[activeLevels];
        if (currentLevel.levelName != "")
        {
            levelText.SetText(currentLevel.levelName);
        }
        else
        {
            levelText.SetText(currentLevel.name);
        }
        settingSlider.value = gameManager.cameraSensitivity;
        ChangeScene(currentScene);
    }

    void FixedUpdate()
    {
        if (!testSceneLock && currentScene == 1)
        {
            if (decreaseLevelButton1 != null && !decreaseLevelButton1.activeSelf)
            {
                decreaseLevelButton1.SetActive(true);
            }

            if (decreaseLevelButton2 != null && !decreaseLevelButton2.activeSelf)
            {
                decreaseLevelButton2.SetActive(true);
            }
        }

        if (currentScene < activeLevels)
        {
            if (increaseLevelButton1 != null && !increaseLevelButton1.activeSelf)
            {
                increaseLevelButton1.SetActive(true);
            }
    
            if (increaseLevelButton2 != null && !increaseLevelButton2.activeSelf)
            {
                increaseLevelButton2.SetActive(true);
            }
        }

    }
    
    
    public void LoadScene()
    {
        if (currentScene < 0)
        {
            Debug.Log("Loading Test Scene");
            LevelLoader.instance.LoadLevel(testScene);
        }
        else if (gameManager.levelData[currentScene].name != "")
        {
        Debug.Log("Scene " + gameManager.levelData[currentScene].name);
        LevelLoader.instance.LoadLevel(gameManager.levelData[currentScene].name);
        }
        else
        {
            Debug.Log("No Scene to Load");
        }
    }
    public void ChangeScene(int num)
    {
        num += currentScene;
        if (num > activeLevels)
        {
            Debug.Log("Level locked");
            //Do something to show levels locked
            if (increaseLevelButton1 != null && increaseLevelButton1.activeSelf)
            {
                increaseLevelButton1.SetActive(false);
            }
    
            if (increaseLevelButton2 != null && increaseLevelButton2.activeSelf)
            {
                increaseLevelButton2.SetActive(false);
            }
            
        }
        else if (num < 1)
        {
            if (testSceneLock)
            {
                currentScene = 1;
                LevelData currentLevel = gameManager.levelData[currentScene];
            if (currentLevel.levelName != "")
            {
            levelText.SetText(currentLevel.levelName);
            }
            else
            {
                levelText.SetText(currentLevel.name);
            }
            }
            else
            {
                currentScene = 0;
                LevelData currentLevel = gameManager.levelData[currentScene];
                if (currentLevel.levelName != "")
                {
                levelText.SetText(currentLevel.levelName);
                }
                else
                {
                    levelText.SetText(currentLevel.name);
                }
            }

            if (decreaseLevelButton1 != null && decreaseLevelButton1.activeSelf)
            {
                decreaseLevelButton1.SetActive(false);
            }

            if (decreaseLevelButton2 != null && decreaseLevelButton2.activeSelf)
            {
                decreaseLevelButton2.SetActive(false);
            }

            if (currentScene < activeLevels) 
            {
                if (increaseLevelButton1 != null && !increaseLevelButton1.activeSelf)
                {
                    increaseLevelButton1.SetActive(true);
                }
    
                if (increaseLevelButton2 != null && !increaseLevelButton2.activeSelf)
                {
                    increaseLevelButton2.SetActive(true);
                }
            }
            else
            {
                if (increaseLevelButton1 != null && increaseLevelButton1.activeSelf)
                {
                    increaseLevelButton1.SetActive(false);
                }
    
                if (increaseLevelButton2 != null && increaseLevelButton2.activeSelf)
                {
                    increaseLevelButton2.SetActive(false);
                }
            }
        }
        else if ( num < numLevels + 1)
        {
            currentScene = num;
            LevelData currentLevel = gameManager.levelData[currentScene];
            if (currentLevel.levelName != "")
            {
            levelText.SetText(currentLevel.levelName);
            }
            else
            {
                levelText.SetText(currentLevel.name);
            }

            if (currentScene < activeLevels) 
            {
                if (increaseLevelButton1 != null && !increaseLevelButton1.activeSelf)
                {
                    increaseLevelButton1.SetActive(true);
                }
    
                if (increaseLevelButton2 != null && !increaseLevelButton2.activeSelf)
                {
                    increaseLevelButton2.SetActive(true);
                }
            }
            else
            {
                if (increaseLevelButton1 != null && increaseLevelButton1.activeSelf)
                {
                    increaseLevelButton1.SetActive(false);
                }
    
                if (increaseLevelButton2 != null && increaseLevelButton2.activeSelf)
                {
                    increaseLevelButton2.SetActive(false);
                }
            }

            if (decreaseLevelButton1 != null && !decreaseLevelButton1.activeSelf)
            {
                decreaseLevelButton1.SetActive(true);
            }

            if (decreaseLevelButton2 != null && !decreaseLevelButton2.activeSelf)
            {
                decreaseLevelButton2.SetActive(true);
            }
        }
    }
    public void CheatCodes()
    {
        string code = codeInput.text;
        Debug.Log(code);

        if (code.Contains("test"))
        {
            Debug.Log("Test Worked");
        }

        if (code.Contains("dungioneers"))
        {
            Debug.Log("code worked");
            if (testSceneLock)
            {
                testSceneLock = false;
                Debug.Log("Test Scene On");
            }
            else
            {
                Debug.Log("Test Scene Off");
                testSceneLock = true;
            }
        }

        if (code.Contains("UnlockLevelsPlease"))
        {
            UnlockLevel(true);
        }

        if (code.Contains("LockLevels"))
        {
            UnlockLevel(false);
        }
        codeInput.image.enabled = false;
        codeInput.text = string.Empty;
    }
    public void UnlockLevel(bool enable)
    {
        if (enable)
        {
            Debug.Log("unLock Levels");
            activeLevels = numLevels;
            
        }
        else
        {
            Debug.Log("Lock Levels");
            activeLevels = gameManager.AccessUnlockedLevels(false);
        }
    }

    public void openSettingMenu(bool open)
    {
        if (open && !settingCanvas.activeSelf)
        {
            settingCanvas.SetActive(true);
        }
        else if (!open && settingCanvas.activeSelf)
        {
            settingCanvas.SetActive(false);
        }
    }

    public void SetMouseSensitivity(float value)
    {
        gameManager.cameraSensitivity = value;
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
