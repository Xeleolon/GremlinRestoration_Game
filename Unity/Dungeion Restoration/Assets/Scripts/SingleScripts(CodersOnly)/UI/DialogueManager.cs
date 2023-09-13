using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    public static DialogueManager instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of DialogueManager found!");
        }
        instance = this;
    }
    #endregion
    
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject dialogueBox;

    public Animator animator;
    private float count;
    private float dialogueCount;
    private bool nextMessage = false;
    public bool freeze = false;
    private Queue<string> sentences;
    
    [Header("Dialogue Trigger")]
    [SerializeField]private bool onStart = false;
    public Dialogue[] dialogues;
    void Start()
    {
        //sentences = new Queue<string>();
        if (onStart)
        {
            PlayDialogue(0);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Debug.Log ("Starting Conversation with " + dialogue.name);
        if (!dialogueBox.activeSelf)
        {
            dialogueBox.SetActive(true);
        }
        animator.SetBool("IsOpen", true);

        nameText.SetText(dialogue.name);
        sentences = new Queue<string>();
        sentences.Clear();


        dialogueCount = dialogue.time;

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void Update()
    {
        if (nextMessage)
        {
            if (count <= 0)
            {
                nextMessage = false;
                DisplayNextSentence();
            }
            else if (!freeze)
            {
                count -= 1 * Time.deltaTime;
            }
        }
    }

    public void DisplayNextSentence()
    {
        Debug.Log(sentences.Count);
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        count = dialogueCount;

        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);

        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        nextMessage = true;
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        Debug.Log ("End of Conversation.");
        animator.SetBool("IsOpen", false);
        //dialogueBox.SetActive(false);

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
