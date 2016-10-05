using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    // Use this for initialization

    Vector3 rot = new Vector3(0.0f, 0.0f, 0.0f);

    void Start()
    {
        
    }

	// Update is called once per frame
	void Update () {
        transform.eulerAngles = rot;
	}
}
