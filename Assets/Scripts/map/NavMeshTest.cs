using UnityEngine;
using System.Collections;

public class NavMeshTest : MonoBehaviour {

    NavMeshAgent agent;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(GameObject.Find("Cube(Clone)").transform.position);
        Debug.Log(GameObject.Find("Cube(Clone)").transform.position);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
