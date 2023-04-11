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
        public new string name; //string name and Objective of the object
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
    [Range(0, 10)]
    public int testInt;
    [Header("Goals")]
    [SerializeField] string textRepair;
    int standardRepair = 0;
    int trackerRepair = 0;
    [SerializeField] string textDestroy;
    int standardDestory = 0;
    int trackerDestory = 0;
    [SerializeField] string textRestock;
    int standardRestock = 0;
    int trackerRestock = 0;
    public int[] labelFill;
    public TMP_Text[] labels;
    public GoalData goalData;
    GameObject[] tempObject;
    // Start is called before the first frame update
    void OnValidate()
    {
        if (updateGoals)
        {
            //TestCreateLabels();
            //FindGoals();
        }
        

    }
    void Start()
    {
        FindGoals();
        UpdateChecklist(0);
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
    void FindGoals()
    {
        //standardRepair = 0;
        RepairInteract[] tempRepair = GameObject.FindObjectsOfType<RepairInteract>();
        /*foreach (RepairInteract i in tempRepair)
        {
            //standardRepair += 1;
        }*/

        standardRepair = tempRepair.Length;

        Debug.Log("found " + standardRepair + " Repairs");
        standardDestory = 0;
        DestroyInteract[] tempDestroy = GameObject.FindObjectsOfType<DestroyInteract>();
        foreach ( DestroyInteract i in tempDestroy)
        {
            //standardDestory += 1;
        }

        standardDestory = tempDestroy.Length;

        Debug.Log("found " + standardDestory + " Destroys");
        standardRestock = 0;
        ReplenishInteract[] tempRestock = GameObject.FindObjectsOfType<ReplenishInteract>();
        foreach ( DestroyInteract i in tempDestroy)
        {
            //standardRestock += 1;
        }

        standardRestock = tempRestock.Length;

        Debug.Log("found " + standardRestock + " Restocks");
        updateGoals = false;
    }
    public void UpdateChecklist(int state)
    {
        switch (state)
        {
            case 0:
            labels[0].text = (textRepair + ": " + standardRepair + " of " + trackerRepair);
            labels[1].text = (textDestroy + ": " + standardDestory + " of " + trackerDestory);
            labels[2].text = (textRestock + ": " + standardRestock + " of " + trackerRestock);
            break;
            case 1:
            trackerRepair += 1;
            labels[0].text = (textRepair + ": " + standardRepair + " of " + trackerRepair);

            break;

            case 2:
            trackerDestory += 1;
            labels[1].text = (textDestroy + ": " + standardDestory + " of " + trackerDestory);

            break;

            case 3:
            trackerRestock += 1;
            labels[2].text = (textRestock + ": " + standardRestock + " of " + trackerRestock);

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
}
