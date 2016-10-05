using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OffenseHeadquaters : MonoBehaviour {


    //public List<Transform> DefenseEntities;
    public LayerMask DefenseLayer;
    GameObject map;

    [HideInInspector]
    public Dictionary<Transform, int[]> Defensedetails;

	// Use this for initialization
	void Start () {
        map = GameObject.Find("MyMap");
        Defensedetails = new Dictionary<Transform, int[]>();
        Transform[] mapElements = map.GetComponentsInChildren<Transform>();
        foreach(Transform t in mapElements)
        {
            if(t.gameObject.layer == 11)
            {
                int[] entities = new int[Constants.SOLDIER_TYPES] { 0, 0, 0 };
                Defensedetails.Add(t, entities);
                //Debug.Log(t.transform.name + "has values " + Defensedetails[t][0] + " " + Defensedetails[t][0] + " " + Defensedetails[t][0]);
            }
        }
        //Defensedetails = new Dictionary<Transform, int[]>();
        //foreach (Transform t in DefenseEntities) {
        //    int[] entities = new int[Constants.SOLDIER_TYPES] { 0,0,0};
        //    Defensedetails.Add(t, entities );
        //}

    }

    //increasing the count of particular soldier to target tower
    public void AddMeToDefense(Transform target, string myFirstName) {
        //Debug.Log(target + " details is " + Defensedetails[target]);
        if (myFirstName == "Sword_Soldier")
        {
            Defensedetails[target][Constants.SWORD_SOLDIER]++;
        }
        else if (myFirstName == "Arrow_Soldier")
        {
            Defensedetails[target][Constants.ARROW_SOLDIER]++;
        }
        else if (myFirstName == "Hammer_Soldier")
        {
            Defensedetails[target][Constants.HAMMER_SOLDIER]++;
        }

    }

    //decreasing the count of particular soldier from target tower
    public void RemoveMeFromDefense(Transform target, string myFirstName)
    {
        if (myFirstName == "Sword_Soldier")
        {
            Defensedetails[target][Constants.SWORD_SOLDIER]--;
        }
        else if (myFirstName == "Arrow_Soldier")
        {
            Defensedetails[target][Constants.ARROW_SOLDIER]--;
        }
        else if (myFirstName == "Hammer_Soldier")
        {
            Defensedetails[target][Constants.HAMMER_SOLDIER]--;
        }

    }
}
