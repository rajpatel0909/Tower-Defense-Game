using UnityEngine;
using System.Collections;

public class SoldierCensor : MonoBehaviour {


    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("triggered");
        if (col.gameObject.tag == "TowerBase") {
            //Debug.Log("triggered");
            this.transform.parent.GetComponent<Soldier>().TargetEntry(col.transform.parent.transform);
        }
        //else if (col.gameObject.tag == "BlockBarricade") {
        //    //Debug.Log("barricade detected");
        //    this.transform.parent.GetComponent<Soldier>().TargetEntry(col.transform);
        //}
        //else if (col.gameObject.tag == "GroundBarricade")
        //{
        //    Debug.Log("barricade detected");
        //    this.transform.parent.GetComponent<Soldier>().TargetEntry(col.transform);
        //}
    }
}
