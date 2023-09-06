using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    public bool showDebugLog;
    bool showHelp;
    string input;
    public delegate void OnSkeltonKeyChanged();
    public OnSkeltonKeyChanged onSkeltonKeyCallback;
    private bool skeltonKey = false;
    public static DebugCommand infinte_Items;
    public static DebugCommand<Vector3> teloport;
    public static DebugCommand help;
    public static DebugCommand skelton_Key;

    public List<object> commandList;

    public List<Dialogue> debugList;
    public int debugLogSize = 20;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
        if (LevelManager.instance != null)
        {
            LevelManager.instance.PauseGame(showConsole);
        }
        Debug.Log("toggle debug");
    }
    public void OnSubmit(InputValue value)
    {
        if (showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    public void OnToggleDebugLog(InputValue value)
    {
        showDebugLog = !showDebugLog;
    }

    public static DebugController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
        //add coomand here
        infinte_Items = new DebugCommand("infinty_toggle", "Toggles Infinte Items on For inventory.", "infinty_toggle", () =>
        {
            Inventory.instance.infiniteItems = !Inventory.instance.infiniteItems;
            Dialogue dialogue = new Dialogue("DebugController", "Infinty now On", 0);
            AddLog(dialogue);
        });

        teloport = new DebugCommand<Vector3>("teloport_player", "move the player to the given quadents", "teloport_player x y z", (x) =>
        {
            Debug.Log("Teloport the player to " + x);
            Dialogue dialogue = new Dialogue("DebugController", "moving player to " + x, 0);
            AddLog(dialogue);
        });

        help = new DebugCommand("help", "Show a list of Debug Commands", "help", () =>
        {
            showHelp = !showHelp;
        });

        skelton_Key = new DebugCommand("skelton_key", "all door locks are overiden and unlocked", "skelton_key", () =>
        {
            SkeltonKeyChanged();
            skeltonKey = !skeltonKey;
            Dialogue dialogue;
            if (skeltonKey)
            {
                dialogue = new Dialogue("DebugController", "SkeltonKey now activate", 0);
            }
            else
            {
                dialogue = new Dialogue("DebugController", "SkeltonKey off", 0);
            }
            AddLog(dialogue);
        });

        commandList = new List<object>
        {
            help,
            infinte_Items,
            skelton_Key,
            teloport,
        };
    }

    Vector2 scroll;
    Vector2 scrollLog;
    private void OnGUI()
    {
        float y = 0;
        if (showConsole)
        {
            if (showHelp)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");
    
                Rect viewport = new Rect(0,0, Screen.width - 30, 20 * commandList.Count);
    
                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
    
                for (int i = 0; i < commandList.Count; i++)
                {
                    DebugCommandBase command = commandList[i] as DebugCommandBase;
    
                    string label = $"{command.commandFormat} - {command.commandDescription}";
    
                    Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    
                    GUI.Label(labelRect, label);
                }
    
                GUI.EndScrollView();
                
                y += 100;
            }

            CreateLogGUI(y + 20);
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
    
            GUI.backgroundColor = new Color(0, 0, 0, 0);
    
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width -20f, 20f), input);
    

        }
        else
        {
            CreateLogGUI(y);
        }


    }

    void CreateLogGUI(float y)
    {
        if (showDebugLog)
        {
            GUI.Box(new Rect(0, y, Screen.width, 200 - y), "");

            Rect viewport = new Rect(0,0, Screen.width - 30, 20 * debugList.Count);

            scrollLog = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 190), scrollLog, viewport);

            for (int i = 0; i < debugList.Count; i++)
            {
                Dialogue log = debugList[i] as Dialogue;

                string label = $"{log.name} - {log.sentences[0]}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            }

            GUI.EndScrollView();
        }
    }


    public void AddLog(Dialogue log)
    {
        if (debugList.Count >= debugLogSize)
        {
            debugList.RemoveAt(0);
        }
        debugList.Add(log);
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (commandBase != null && input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    //Cast to this type and invoke the command
                    (commandList[i] as DebugCommand).Invoke();

                }
                else if(commandList[i] as DebugCommand<Vector3> != null)
                {
                    (commandList[i] as DebugCommand<Vector3>).Invoke(new Vector3(int.Parse(properties[1]), int.Parse(properties[2]), int.Parse(properties[3])));
                }
            }
        }
    }

    private void SkeltonKeyChanged()
    {
        if (onSkeltonKeyCallback != null)
        {
            onSkeltonKeyCallback.Invoke();
        }
    }
}
