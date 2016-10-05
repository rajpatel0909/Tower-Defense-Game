using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

    public Text loading;
    public float timeGap;
    public float existenceTime;

    [HideInInspector]
    public bool startLoading = false;

    int i = 0;
    float lastTime = 0;
    int length;
    string originalString;

	void Start()
    {
        originalString = loading.text.ToString();
        length = originalString.Length;
    }

    void Update()
    {
        if(Time.time > lastTime + timeGap)
        {
            lastTime = Time.time;
            loading.text = originalString.Substring(0, length - i);
            i = (i + 1) % (length + 1);
        }
        if(startLoading)
        {

        }
    }

}
