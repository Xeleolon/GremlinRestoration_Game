using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{
    public string name;
    [TextArea(3, 10)]
    public string[] sentences;
    public float time = 0;

    public Dialogue(string newName, string newSentences, float newTime)
    {
        name = newName;

        sentences = new string[1];
        sentences[0] = newSentences;
        time = newTime;
    }
}
