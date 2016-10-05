using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

    public static DataManager DM;
	// Use this for initialization
	void Awake()
    {
        if(DM == null)
        {
            DontDestroyOnLoad(gameObject);
            DM = this;
        }
        else if(DM != this)
        {
            Destroy(gameObject);
        }
    }
	
	


}
