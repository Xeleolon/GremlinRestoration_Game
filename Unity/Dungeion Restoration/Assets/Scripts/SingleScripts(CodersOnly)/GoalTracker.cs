using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GoalTracker : MonoBehaviour
{
    [System.Serializable]
    public class GoalData //hold all revelant data for Each Goal
    {
        public string name = new string("gameData");// booth name and desrctiption on contaniener;
        public int target; //int max goal private element
        public int progress;//int progress to goal changable
        public int place; //int placement within order changble
        public bool active; //Should it be active and rendering
        public bool toggleON; //should the toggle be used of the numCheck System used
        public GameObject label; //gameObject with checklist of items
        private TMP_Text objective; //text object with the goal the player trying to achieve
        private TMP_Text numCheck; // text object with progress of target.
        private Toggle toggle; // toggle for target of one.

        public void ActiveLabel()
        {
            //Debug.Log("stage three " + name + " " + target);
            if (label != null && !active)
            {
                objective = label.GetComponent<TMP_Text>();
                objective.SetText(name);

                GameObject child = label.transform.GetChild(0).gameObject; 
                if (toggleON)
                {
                    toggle = child.GetComponent<Toggle>();
                }
                else
                {
                    numCheck = child.GetComponent<TMP_Text>();
                    numCheck.SetText((progress + " of " + target));
                }
                //Debug.Log("stage four " + name + " " + target);
                active = true;
            }
        }

        public void GoalAchieved()
        {
            if (toggleON)
            {
                toggle.isOn = true;
            }
            else
            {
                numCheck.SetText((progress + " of " + target));
            }
        }


    }
    [System.Serializable]
    public class ChangableGoalData //a object which will be public and used to change variable inside spefic GoalData;
    {
        public string name = new string("ChangableGoalData"); //name of container

        [Tooltip("Text shown on the label")]
        public string description;

    }
    #region Singleton
    public static GoalTracker instance;

    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogWarning("More Than One instance of inventory found!");
        }
        instance = this;
    }
    #endregion
    
    public GameObject checkList;
    public GameObject labelNumPrefab;
    public GameObject labelCheckPrefab;
    [Tooltip("The title ontop of the labels")]
    public Transform TitlePosition;
    [Range(1, 8)]
    [SerializeField] int numOfLabels = 1;
    [Range(10f, 100)]
    [SerializeField] float labelOffest = 1;
    public bool updateGoals;
    bool refreshWait;
    [Header("Testing Only")]
    public bool testCompleteTop;
    public bool emptyLists;
    [Range(0, 10)]
    public int testInt;
    [Header("Goals")]
    [Tooltip("standard labels used")]
    [SerializeField] ChangableGoalData[] standardGoals;

    [Range(10, 100)]
    public int maxGoals = 10;
    private int filledGoals = 0;
    public int[] order; //hold the order of goals
    public bool goalSetup = false;
    public GoalData[] goalData; //will become private after finished testing

    GameObject[] tempObject;
    // Start is called before the first frame update
    void OnValidate()
    {
        if (updateGoals)
        {
            SetupStandardLabels();
        }
        if (emptyLists)
        {
            goalData = new GoalData[0];
            goalData = new GoalData[maxGoals];
            emptyLists = false;
        }
        

    }
    void Start()
    {
        //SetupStandardLabels();

        /*
        if (checkList.activeSelf)
        {
            checkList.SetActive(false);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Dropdown") != 0 && !refreshWait)
        {
            //Debug.Log("Button working");
            if (checkList.activeSelf)
            {
                checkList.SetActive(false);
            }
            else
            {
                checkList.SetActive(true);
            }
            refreshWait = true;

        }
        else if (refreshWait && Input.GetAxisRaw("Dropdown") == 0)
        {
            refreshWait = false;
        }

        if (testCompleteTop)
        {
            CreateLabels();
            testCompleteTop = false;
        }
    }
    void SetupStandardLabels()
    {
        order = new int[maxGoals];
        for(int i = 0; i < maxGoals; i++)
        {
            order[i] = -1;
        }

        //goalData = new GoalData[standardGoals.Length];
        goalData = new GoalData[maxGoals];
        for(int i = 0; i < standardGoals.Length; i++)
        {
            goalData[i] = new GoalData(); //must do this for class to make it not null do it every time you found a new labels!!
            goalData[i].name = standardGoals[i].description;
            filledGoals = i + 1;
            order[(i + 3)] = i;
            goalData[i].place = i + 3;
        }
        goalSetup = true;
    }
    public int CreateGoalData(string state)
    {
        if (goalData.Length != maxGoals || order.Length != maxGoals)
        {
            SetupStandardLabels();
        }
        switch(state)
        {
            case "1":
            goalData[0].target += 1;
            //Debug.Log("Target" + goalData[0].name + " = " + goalData[0].target);
            return 0;

            case "2": 
            goalData[1].target += 1;
            //Debug.Log("Target" + goalData[1].name + " = " + goalData[1].target);
            return 1;

            case "3": 
            goalData[2].target += 1;
            //Debug.Log("Target" + goalData[2].name + " = " + goalData[2].target);
            return 2;

            default:
            if (goalData.Length > 3)
            {
                for (int i = 3; i < goalData.Length; i++) //check if they already a conanter for this variable and fill it
                {
                    if (goalData[i] != null && goalData[i].name == state)
                    {
                        
                        goalData[i].target += 1;
                        
                        return i;
                    }
                }
            
            if (filledGoals < maxGoals) //make a new container
            {
                goalData[filledGoals] = new GoalData();
                goalData[filledGoals].name = state;
                goalData[filledGoals].target += 1;
                if (filledGoals <= 6 && filledGoals >= 4)
                {
                    order[(filledGoals - 3)] = filledGoals;
                    goalData[filledGoals].place = filledGoals - 3;
                }
                else
                {
                    order[filledGoals] = filledGoals;
                    goalData[filledGoals].place = filledGoals;
                }

                filledGoals += 1;
                return (filledGoals - 1);
            }

            }

            Debug.Log("Need to increase max size to contain any more interactables");
            return (-1);

        }
    }
    void CreateLabels()
    {
        //Debug.Log("Testing 2 " + goalData[0].target);
        //take top 6 and give them labels
        for(int i = 0; i < numOfLabels; i++)
        {
            if (order[i] >= 0 && goalData[order[i]] != null)
            {
                Vector3 tempPostion = TitlePosition.position;
                tempPostion.y -= labelOffest * (i + 1);

                Debug.Log("stage one " + goalData[order[i]].name + " " + goalData[order[i]].target);



                if (goalData[order[i]].target > 1)
                {
                    goalData[order[i]].label = Instantiate(labelNumPrefab, tempPostion,  Quaternion.identity);
                    goalData[order[i]].toggleON = false;
                }
                else
                {
                    goalData[order[i]].label = Instantiate(labelCheckPrefab, tempPostion,  Quaternion.identity);  
                    goalData[order[i]].toggleON = true;
                }

                //Debug.Log("stage two " + goalData[order[i]].name + " " + goalData[order[i]].target);
                
                goalData[order[i]].label.transform.parent = TitlePosition;
                goalData[order[i]].ActiveLabel();
            }
        }
    }
    public void CompletedGoal(int ticket)
    {
        goalData[ticket].progress += 1;
        goalData[ticket].GoalAchieved();

        if (goalData[ticket].target == goalData[ticket].progress)
        {
            //Then we completed and move all up one.
        }
    }

    void CycleLables()
    {
        
    }
}
