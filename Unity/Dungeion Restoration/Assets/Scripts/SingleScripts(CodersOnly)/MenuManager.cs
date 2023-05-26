using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public string[] level;
    public string testScene;
    private int currentScene = 0;
    public bool testSceneLock = true;
    public TMP_Text levelText;
    public TMP_InputField codeInput;
    void Start()
    {
        if (level.Length == 0)
        {
            levelText.SetText("No Scenes");
        }
        else if (currentScene > level.Length)
        {
            currentScene = level.Length - 1;
            levelText.SetText(level[currentScene]);
        }
        else
        {
            levelText.SetText(level[currentScene]);
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            //codeText.SetText(string.Empty);
        }
    }
    
    public void LoadScene()
    {
        if (currentScene < 0)
        {
            Debug.Log("Loading Test Scene");
            SceneManager.LoadScene(testScene);
        }
        else if (level[currentScene] != "")
        {
        Debug.Log("Scene " + level[currentScene]);
        SceneManager.LoadScene(level[currentScene]);
        }
        else
        {
            Debug.Log("No Scene to Load");
        }
    }
    public void ChangeScene(int num)
    {
        num += currentScene;
        if (level.Length != 0 && num > level.Length)
        {
            currentScene = level.Length - 1;
            levelText.SetText(level[currentScene]);
        }
        else if (num < 0)
        {
            if (testSceneLock)
            {
                currentScene = 0;
                levelText.SetText(level[currentScene]);
            }
            else
            {
                currentScene = -1;
                levelText.SetText(testScene);
            }
        }
        else if (level.Length != 0 && num < level.Length)
        {
            currentScene = num;
            levelText.SetText(level[currentScene]);
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
}
