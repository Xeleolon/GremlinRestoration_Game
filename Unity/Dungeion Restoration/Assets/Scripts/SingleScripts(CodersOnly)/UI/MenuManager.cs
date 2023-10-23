using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private int numLevels = 5;
    public string testScene;
    private int currentScene = 0;
    public bool testSceneLock = true;
    [SerializeField] private GameObject settingCanvas;
    [SerializeField] private Slider settingSlider;
    public TMP_Text levelText;
    public TMP_InputField codeInput;
    GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        if (numLevels == 0)
        {
            levelText.SetText("No Scenes");
        }
        else if (currentScene > numLevels + 1)
        {
            currentScene = numLevels;
        }

        LevelData currentLevel = gameManager.levelData[currentScene];
        if (currentLevel.levelName != "")
        {
            levelText.SetText(currentLevel.levelName);
        }
        else
        {
            levelText.SetText(currentLevel.name);
        }
        settingSlider.value = gameManager.cameraSensitivity;
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
        if (num > numLevels)
        {
            currentScene = numLevels;
            LevelData currentLevel = gameManager.levelData[currentScene];
            if (currentLevel.levelName != "")
            {
            levelText.SetText(currentLevel.levelName);
            }
            else
            {
                levelText.SetText(currentLevel.name);
            }
            settingSlider.value = gameManager.cameraSensitivity;
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
            settingSlider.value = gameManager.cameraSensitivity;
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
        codeInput.image.enabled = false;
        codeInput.text = string.Empty;
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
