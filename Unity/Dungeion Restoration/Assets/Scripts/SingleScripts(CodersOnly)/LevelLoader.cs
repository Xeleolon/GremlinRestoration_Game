using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public GameObject loadingScreen;
    public Slider slider;
    private string currentscene;
    private GameObject player;



    

    public void ReloadLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel ( string sceneName)
    {
        currentscene = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadAsynchronously(sceneName));
    }


    IEnumerator LoadAsynchronously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        loadingScreen.SetActive(true);
        if (player != null)
        {
            player.GetComponent<PlayerMovements>().interactActive = false;
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log(operation.progress);
            slider.value = progress;

            yield return null;
        }

        if (operation.isDone)
        {
            Debug.Log(sceneName + " was Loaded asyncly!");
            StartCoroutine(UnLoadAsynchronously(currentscene));
            currentscene = sceneName;
            GameObject[] findPlayer = GameObject.FindGameObjectsWithTag("Player");
            GameObject oldPlayer = player;
            foreach (GameObject newPlayer in findPlayer)
            {
                if (newPlayer != player)
                {
                    player = newPlayer;
                    player.GetComponent<PlayerMovements>().interactActive = false;
                }
                
            }
            if (player == oldPlayer)
            {
                player = null;
            }
        }
    }

    IEnumerator UnLoadAsynchronously (string oldSceneName)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldSceneName);

        while (!asyncUnload.isDone)
        {
            float progress = Mathf.Clamp01(asyncUnload.progress / 0.9f);
            progress += 1;
            slider.value = progress;
            yield return null;
        }

        if (asyncUnload.isDone)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentscene));
            Debug.Log(oldSceneName + " was unloaded");
            loadingScreen.SetActive(false);
            if (player != null && LevelManager.instance.pauseRequest <= 0)
            {
                player.GetComponent<PlayerMovements>().interactActive = true;
            }
        }
    }
}
