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
        public void UnActiveLabel()
        {
            if (label != null && !active)
            {
                Destroy(label);
                active = false;
            }
        }

        public void GoalAchieved()
        {
            if (label != null)
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
    [SerializeField] float labelOffet = 10;
    bool refreshWait;
    private bool updateGoals = false;
    [Header("Testing Only")]
    public bool testCompleteTop;
    public bool emptyLists;
    [Range(0, 10)]
    public int testInt;
    private float setupMax = 3;
    [Header("Goals")]
    [Tooltip("standard labels used")]
    [SerializeField] ChangableGoalData[] standardGoals;

    [Range(10, 100)]
    public int maxGoals = 10;
    private int filledGoals = 0;
    private int[] order; //hold the order of goals
    private bool goalSetup = false;
    private GoalData[] goalData; //will become private after finished testing

    GameObject[] tempObject;
    // Start is called before the first frame update
    void OnValidate()
    {
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
        if (setupMax <= 0 && !testCompleteTop)
        {
            
            //Debug.Log("checkList Made");
            if (goalData == null || goalData.Length != maxGoals || order.Length != maxGoals)
            {
                SetupStandardLabels();
            }

            //CreateLabels();
            Debug.Log("Create Labels");

            testCompleteTop = true;
        }
        else
        {
            setupMax -= 1 * Time.deltaTime;
        }

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
    }
    void SetupStandardLabels()
    {
        order = new int[maxGoals + 1];
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
            CreateLables(i);
        }
        goalSetup = true;
    }
    public int CreateGoalData(string state)
    {
        if (!updateGoals)
        {
            SetupStandardLabels();
            testInt += 1;
            Debug.Log(testInt);
            updateGoals = true;
        }
        switch(state)
        {
            case "1":
            goalData[0].target += 1;
            if (goalData[0].target > 1 && goalData[0].toggleON);
            {
                goalData[0].UnActiveLabel();
                goalData[0].toggleON = false;
                CreateLables(0);
            }
            //Debug.Log("Target" + goalData[0].name + " = " + goalData[0].target);
            return 0;

            case "2": 
            goalData[1].target += 1;
            if (goalData[1].target > 1 && goalData[1].toggleON);
            {
                    goalData[1].UnActiveLabel();
                    goalData[1].toggleON = false;
                    CreateLables(1);
            }
            //Debug.Log("Target" + goalData[1].name + " = " + goalData[1].target);
            return 1;

            case "3": 
            goalData[2].target += 1;
            if (goalData[2].target > 1 && goalData[2].toggleON);
            {
                    goalData[2].UnActiveLabel();
                    goalData[2].toggleON = false;
                    CreateLables(2);
            }
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
                        if (goalData[0].toggleON)
                        {
                        goalData[i].UnActiveLabel();
                        goalData[i].toggleON = false;
                        CreateLables(i);
                        }
                        
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

                    CreateLables(filledGoals);
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
    void OldCreateLabels()
    {
        //Debug.Log("Testing 2 " + goalData[0].target);
        //take top 6 and give them labels
        for(int i = 0; i < numOfLabels; i++)
        {
            //Debug.Log(i);
            if (order[i] >= 0 && goalData[order[i]] != null)
            {
                Vector3 tempPostion = TitlePosition.position;
                tempPostion.y -= labelOffet * (i + 1);

                //Debug.Log("stage one " + goalData[order[i]].name + " " + goalData[order[i]].target);



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
    void CreateLables(int ticket) //identity for the spefiic goalData being refernceced
    {
        if (!goalData[ticket].active)
        {
            int place = goalData[ticket].place;
            //create the location of the label
            Vector3 tempPostion = TitlePosition.position;
            tempPostion.y -= labelOffet * (place + 1);

            //identifty what type of label needs to be created
            if (goalData[ticket].target > 1)
            {
                goalData[ticket].label = Instantiate(labelNumPrefab, tempPostion, Quaternion.identity);
                goalData[ticket].toggleON = false;
            }
            else
            {
                goalData[ticket].label = Instantiate(labelCheckPrefab, tempPostion,  Quaternion.identity);  
                goalData[ticket].toggleON = true;
            }

            //Set new Label as child of object
            goalData[ticket].label.transform.SetParent(TitlePosition, true);

            //Activate the Label of the object
            goalData[ticket].ActiveLabel();
        }
    }
    public void CompletedGoal(int ticket)
    {
        Debug.Log(ticket);
        goalData[ticket].progress += 1;
        goalData[ticket].GoalAchieved();
        if (goalData[ticket].place != 0)
        {
            int newPlace = goalData[ticket].place;
            //move current occupant if filled.
            if (order[0] != -1)
            {
                order[filledGoals] = order[0];
                goalData[order[0]].place = filledGoals;
                MoveLable(order[0]);
            }
            //move new occunat to fill gap
            order[0] = ticket;
            goalData[ticket].place = 0;
            MoveLable(ticket);

            
        }

        if (goalData[ticket].target == goalData[ticket].progress)
        {
            //Then we completed and move all up one.
        }
    }
    void MoveLable(int ticket) //take one label and move it to new position
    {
        if (goalData[ticket].place < numOfLabels)
        {
            //need to consider how to add movement in furture design improvements
            if (goalData[ticket].active)
            {
                int place = goalData[ticket].place;
                Vector3 tempPostion = TitlePosition.position;
                tempPostion.y -= labelOffet * (place + 1);
                goalData[ticket].label.transform.position = tempPostion;
            }
            else
            {
                CreateLables(ticket);
            }
        }
        else
        {
            goalData[ticket].UnActiveLabel();
        }
    }
    void CycleLables(int space) // the new space that has now been made that must be filled
    {
        for (int i = space + 1; i < goalData.Length; i ++)
        {
            order[i - 1] = order[i];
            goalData[order[i]].place = i - 1;
            MoveLable(order[i - 1]);
        }
    }
}
