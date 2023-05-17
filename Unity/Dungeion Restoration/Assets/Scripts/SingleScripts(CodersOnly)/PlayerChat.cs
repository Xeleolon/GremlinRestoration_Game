using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerChat : MonoBehaviour
{
    #region Singleton
    public static PlayerChat instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
    }
    #endregion


    public GameObject canvas;
    public TMP_Text chatText;
    bool active;
    public float maxTime = 3;
    private float timer;
    void Update ()
    {
        if (active)
        {
            if (timer <= 0)
            {
                chatText.SetText("");
                canvas.SetActive(false);
                active = false;
            }
            else
            {
                timer -= 1 * Time.deltaTime;
            }
        }
    }

    public void NewMessage(string message)
    {
        Debug.Log("New message");
        if (!active)
        {
        canvas.SetActive(true);
        }

        chatText.SetText(message);
        timer = maxTime;
        active = true;
    }
}
