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
        public new string name = new string("gameData");// booth name and desrctiption on contaniener;
        public int target; //int max goal private element
        public int progress;//int progress to goal changable
        public int place; //int placement within order changble
        public bool active; //Should it be active and rendering
        public bool toggleON; //should the toggle be used of the numCheck System used
        public GameObject label; //gameObject with checklist of items
        private TMP_Text objective; //text object with the goal the player trying to achieve
        private TMP_Text numCheck; // text object with progress of target.
        private Toggle toggle; // toggle for target of one.


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
    public int[] labelFill;
    public TMP_Text[] labels; //need to removed
    public List<GoalData> goalData; //will become private after finished testing
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
            goalData.Clear();
            emptyLists = false;
        }
        

    }
    void Start()
    {
        SetupStandardLabels();

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
    }
    void SetupStandardLabels()
    {
        //goalData = new GoalData[standardGoals.Length];
        goalData.Clear();
        for(int i = 0; i < standardGoals.Length; i++)
        {
            //goalData[i] = new GoalData(); //must do this for class to make it not null do it every time you found a new labels!!
            GoalData temp = new GoalData();
            temp.name = standardGoals[i].description;
            goalData.Add(temp);
        }
    }
    public void CreateGoalData(string state)
    {
        GoalData tempFind = new GoalData();
        GoalData temp;
        switch(state)
        {
            case "1":
            tempFind.name = standardGoals[0].description;
            temp = goalData.Find(tempFind);
            temp.target += 1;
            Debug.Log("Target" + temp.name + " = " + temp.target);
            break;

            case "2": 
            tempFind.name = standardGoals[1].description;
            temp = goalData.Find(tempFind);
            temp.target += 1;
            Debug.Log("Target" + temp.name + " = " + temp.target);
            break;

            case "3": 
            tempFind.name = standardGoals[2].description;
            temp = goalData.Find(tempFind);
            temp.target += 1;
            Debug.Log("Target" + temp.name + " = " + temp.target);
            break;

            default:
            tempFind.name = state;
            if (goalData.Contains(tempFind))
            {
                temp = goalData.Find(tempFind);
                temp.target += 1;
                Debug.Log("Target" + temp.name + " = " + temp.target);
            }
            else
            {
                temp = new GoalData();
                temp.name = state;
                temp.target += 1;
                goalData.Add(temp);
                Debug.Log("Target" + temp.name + " = " + temp.target);
            }

            Debug.Log("creating new goal with string");
            break;

        }
    }
    
    void TestCreateLabels()
    {
        if (tempObject.Length != 0)
        {
        foreach(GameObject i in tempObject)
        {
            Destroy(i);
        }
        }


        tempObject = new GameObject[numOfLabels];
        labels = new TMP_Text[numOfLabels];
        for(int i = 0; i < numOfLabels; i++)
        {
            Vector3 tempPostion = TitlePosition.position;
            tempPostion.y -= labelOffest * (i + 1);

            tempObject[i] = Instantiate(labelCheckPrefab, tempPostion,  Quaternion.identity);

            tempObject[i].transform.parent = TitlePosition;
            labels[i] = tempObject[i].GetComponent<TMP_Text>();
        }
    }

    void CycleLables()
    {
        
    }
    public void LogNewGoal(string description)
    {

    }
}
