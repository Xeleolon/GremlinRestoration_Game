using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    #region Singleton
    public static DialogueTrigger instance;
    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one instance of DialogueTrigger found!");
        }
        instance = this;
    }
    #endregion

    public bool onStart = false;
    public Dialogue[] dialogues;
    public void Start()
    {
        if (onStart)
        {
            PlayDialogue(0);
        }
    }
    public void PlayDialogue(int place)
    {
        if (dialogues.Length > place)
        {
            DialogueManager.instance.StartDialogue(dialogues[place]);
        }
        else
        {
            Debug.Log(place + " not enough space");
        }
    }
}
