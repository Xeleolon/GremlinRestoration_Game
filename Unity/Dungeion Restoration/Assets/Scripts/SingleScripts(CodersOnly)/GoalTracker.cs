using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalTracker : MonoBehaviour
{
    public GameObject checkList;
    bool refreshWait;
    [Header("Goals")]
    int standardRepair = 1;
    int standardDestory = 1;
    int standardRestock = 1;
    public int[] labelFill;
    public Text[] Labels;
    // Start is called before the first frame update
    void Start()
    {
        UpdateChecklist();
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
    void UpdateChecklist()
    {
        Labels[0].text = new string("Repair " + standardRepair + " of " + labelFill[0]);
    }
}
