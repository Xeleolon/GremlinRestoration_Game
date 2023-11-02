using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    public bool showDebugLog;
    bool showHelp;
    bool showTeloportHelp;
    string input;
    private GameObject Player;
    private float clock;
    private bool ShowDebugPopUp;
    [Tooltip("on F1 if function called how long does Popup of Debug last")]
    [SerializeField] private float maxPopupLength = 3;

    [SerializeField] private Transform teloportGroup;

    private bool freazeGameAllowed;
    private bool freazeGame;
    private Transform[] teloportPoints;
    public delegate void OnSkeltonKeyChanged();
    public OnSkeltonKeyChanged onSkeltonKeyCallback;
    private bool skeltonKey = false;
    private bool unlockLevel = false;
    public static DebugCommand infinte_Items;
    public static DebugCommand<Vector3> teloport;
    public static DebugCommand<string> teloport_To;
    public static DebugCommand teloport_Help;
    public static DebugCommand help;
    public static DebugCommand skelton_Key;
    public static DebugCommand<string> load_Level;
    public static DebugCommand load_Menu;
    public static DebugCommand reload_Level;
    public static DebugCommand unlock_Levels;

    public static DebugCommand enableGameFreazing;

    

    public List<object> commandList;
    public List<object> menuCommandList;

    [HideInInspector]public List<Dialogue> debugList;
    public int debugLogSize = 20;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
        if (LevelManager.instance != null)
        {
            LevelManager.instance.PauseGame(showConsole);
        }

        if (showConsole)
        {
            showHelp = false;
            showTeloportHelp = false;
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

    public void OnToggleDebugFreaze(InputValue value)
    {
        if (freazeGameAllowed)
        {
            freazeGame = !freazeGame;
        }

        if (freazeGameAllowed && freazeGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public static DebugController instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;

        if (teloportGroup == null)
        {
            Debug.LogWarning("No teloport Group asigned");
            teloportPoints = new Transform[1];
            if (Player == null)
            {
                Player = GameObject.FindWithTag("Player");
            }
            if (Player != null)
            {
                teloportPoints[0] = Player.transform;
            }
        }
        else
        {
            teloportPoints = new Transform[teloportGroup.childCount + 1];
            if (Player == null)
            {
                Player = GameObject.FindWithTag("Player");
            }
            if (Player != null)
            {
                teloportPoints[0] = Player.transform;
                for (int i = 1; i < teloportGroup.childCount + 1; i++)
                {
                    teloportPoints[i] = teloportGroup.GetChild(i);
                }
            }
        }
        //add coomand here
        infinte_Items = new DebugCommand("infinty_toggle", "Toggles Infinte Items on For inventory.", "infinty_toggle", () =>
        {
            Inventory.instance.infiniteItems = !Inventory.instance.infiniteItems;
            Dialogue dialogue;
            if (Inventory.instance != null && Inventory.instance.infiniteItems)
            {
                dialogue = new Dialogue("DebugController", "Infinty now On", maxPopupLength);
            }
            else
            {
                dialogue = new Dialogue("DebugController", "Infinty now Off", maxPopupLength);
            }
            AddLog(dialogue);
        });

        teloport = new DebugCommand<Vector3>("teloport", "move the player to the given quadents", "teloport x y z", (x) =>
        {
            Debug.Log("Teloport the player to " + x);

            if (Player != null)
            {
                Player.transform.position = x;
                Dialogue dialogue = new Dialogue("DebugController", "moving player to " + x, maxPopupLength);
                AddLog(dialogue);
            }
            else
            {
                Dialogue dialogue = new Dialogue("DebugController", "No Player To Move", maxPopupLength);
                AddLog(dialogue);
            }

        });

        teloport_To = new DebugCommand<string>("teloport", "Teloport the player to set point", "teloport (Name of Point)", (x) =>
        {
            int point = FindTelportPoint(x);
            if (Player != null)
            {
                if (point == 0)
                {
                    Player.transform.position = teloportPoints[point].position;
                    Dialogue dialogue = new Dialogue("DebugController", "moving player to Start", maxPopupLength);
                    Debug.Log("Teloport the player to Start");
                    AddLog(dialogue);
                }
                else if (point >= 0)
                {
                    Player.transform.position = teloportPoints[point].position;
                    Dialogue dialogue = new Dialogue("DebugController", "moving player to " + x, maxPopupLength);
                    Debug.Log("Teloport the player to " + x);
                    AddLog(dialogue);
                }
            }
            else
            {
                Dialogue dialogue = new Dialogue("DebugController", "No Player To Move", maxPopupLength);
                AddLog(dialogue);
            }

        });

        help = new DebugCommand("help", "Show a list of Debug Commands", "help", () =>
        {
            if (showTeloportHelp)
            {
                showTeloportHelp = !showTeloportHelp;
            }
            showHelp = !showHelp;

        });

        skelton_Key = new DebugCommand("skelton_key", "all door locks are overiden and unlocked", "skelton_key", () =>
        {
            SkeltonKeyChanged();
            skeltonKey = !skeltonKey;
            Dialogue dialogue;
            if (skeltonKey)
            {
                dialogue = new Dialogue("DebugController", "SkeltonKey now activate", maxPopupLength);
            }
            else
            {
                dialogue = new Dialogue("DebugController", "SkeltonKey off", maxPopupLength);
            }
            AddLog(dialogue);
        });

        teloport_Help = new DebugCommand("teloport_Help", "open up the help window of telopoint points", "teloport_Help", () =>
        {
            if (showHelp)
            {
                showHelp = false;
            }
            showTeloportHelp = !showTeloportHelp;
        });

        load_Level = new DebugCommand<string>("load","load level by giving level name","load (level name)", (x) =>
        {
            LevelLoader.instance.LoadLevel(x);
        });

        load_Menu = new DebugCommand("load_menu","Load the menu level","load_menu", () =>
        {
            LevelLoader.instance.LoadLevel("MainMenu");
        });

        reload_Level = new DebugCommand("reload_level","Reload Current level","reload_level", () =>
        {
            LevelLoader.instance.ReloadLevel();
        });

        enableGameFreazing = new DebugCommand("enable_freaze", "Unlock the abitlity to freaze the game by F3", "enable_freaze", () =>
        {
            freazeGameAllowed = !freazeGameAllowed;

            Dialogue dialogue;
            if (freazeGameAllowed)
            {
                dialogue = new Dialogue("DebugController", "freaze game allowed", maxPopupLength);
            }
            else
            {
                dialogue = new Dialogue("DebugController", "freaze game not allowed", maxPopupLength);
            }
            AddLog(dialogue);
        });

        unlock_Levels = new DebugCommand("unlock_levels", "Unlock Levels for playing", "unlock_levels", () =>
        {
            unlockLevel = !unlockLevel;
            if (MenuManager.instance != null)
            {
                Dialogue dialogue;
                if (unlockLevel)
                {
                    MenuManager.instance.UnlockLevel(true);
                    dialogue = new Dialogue("DebugController", "Unlocked Levels", maxPopupLength);
                }
                else
                {
                    MenuManager.instance.UnlockLevel(false);
                    dialogue = new Dialogue("DebugController", "locked Levels", maxPopupLength);
                }

                AddLog(dialogue);
            }
            else
            {
                Debug.Log("Not in menu");
            }
        });

        commandList = new List<object>
        {
            help,
            infinte_Items,
            skelton_Key,
            teloport_Help,
            teloport,
            teloport_To,
            reload_Level,
            load_Menu,
            load_Level,
            enableGameFreazing,
            unlock_Levels,
        };

        menuCommandList = new List<object>
        {
            help,
            unlock_Levels,
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
                if (MenuManager.instance == null)
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
                else
                {
                    GUI.Box(new Rect(0, y, Screen.width, 100), "");
        
                    Rect viewport = new Rect(0,0, Screen.width - 30, 20 * menuCommandList.Count);
        
                    scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
        
                    for (int i = 0; i < menuCommandList.Count; i++)
                    {
                        DebugCommandBase command = menuCommandList[i] as DebugCommandBase;
        
                        string label = $"{command.commandFormat} - {command.commandDescription}";
        
                        Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                        
                        GUI.Label(labelRect, label);
                    }
        
                    GUI.EndScrollView();
                    
                    y += 100;
                }
            }
            else if (showTeloportHelp && teloportPoints != null)
            {
                GUI.Box(new Rect(0, y, Screen.width, 100), "");
    
                Rect viewport = new Rect(0,0, Screen.width - 30, 20 * teloportPoints.Length + 2);
    
                scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);
    
                for (int i = 0; i < teloportPoints.Length + 2; i++)
                {
                    if (i == 0)
                    {
                        string label = $"{teloport.commandFormat} - {teloport.commandDescription}";
    
                        Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    
                        GUI.Label(labelRect, label);
                        
                    }
                    else if (i == 1)
                    {
                        string label = $"{teloport_To.commandFormat} - {teloport_To.commandDescription}";
    
                        Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    
                        GUI.Label(labelRect, label);
                    }
                    else if (i == 2)
                    {
                        string label = $"{"Start"} - {"Return to Player Start Position"}";
    
                        Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    
                        GUI.Label(labelRect, label);

                    }
                    else if (i >= 3)
                    {
                        Transform point = teloportPoints[i - 2] as Transform;
    
                        string label = $"{point.name}";
    
                        Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                    
                        GUI.Label(labelRect, label);
                    }
                }
    
                GUI.EndScrollView();
                
                y += 100;
            }
            
            if (showDebugLog || ShowDebugPopUp)
            {
                CreateLogGUI(y + 20);
                if (showDebugLog)
                {
                    if (clock <= 0)
                    {
                        showDebugLog = false;
                    }
                    else
                    {
                        clock -= 1 * Time.deltaTime;
                    }
                }
            }
            
            GUI.Box(new Rect(0, y, Screen.width, 30), "");
    
            GUI.backgroundColor = new Color(0, 0, 0, 0);

            GUI.SetNextControlName("console");
            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f,20f), input);
            GUI.FocusControl("console");


            input = GUI.TextField(new Rect(10f, y + 5f, Screen.width -20f, 20f), input);
    

        }
        else
        {
            if (showDebugLog)
            {
                CreateLogGUI(y);
            }
        }


    }

    void CreateLogGUI(float y)
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


    public void AddLog(Dialogue log)
    {
        if (debugList.Count >= debugLogSize)
        {
            debugList.RemoveAt(0);
        }
        debugList.Add(log);
        if (log.time > 0)
        {
            showDebugLog = true;
            clock = log.time;
        }
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
                else if(commandList[i] as DebugCommand<string> != null && properties.Length >= 2 && properties.Length < 3)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                }
                else if(commandList[i] as DebugCommand<Vector3> != null && properties.Length >= 4)
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

    int FindTelportPoint(string name)
    {
        if (name.Contains("start"))
        {
            return 0;
        }
        for (int i = 1; i < teloportPoints.Length; i++)
        {
            if (name.Contains(teloportPoints[i].name))
            {
                return i;
            }
        }
        
        return -1;
    }
}
