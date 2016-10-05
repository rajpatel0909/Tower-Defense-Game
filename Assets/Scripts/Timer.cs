using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    // Use this for initialization
    private bool isAnimationStarted;
    float currentTime;
    public float timeInSeconds;
    public float angle;
    private RectTransform clock;

	void Start () {
        currentTime = 0;
        isAnimationStarted = false;
        clock = this.gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        if(this.gameObject.activeSelf)
        {
            if (currentTime <= timeInSeconds)
            {
                currentTime += Time.deltaTime;
                this.GetComponent<Image>().fillAmount = currentTime / timeInSeconds;
                if ((timeInSeconds - currentTime) <= 10 && !isAnimationStarted)
                {
                    isAnimationStarted = true;
                    InvokeRepeating("StartVibration", 0, 0.3f);
                    InvokeRepeating("StartScaling", 0, 0.1f);
                }
            }
            else
            {
                //Deactive self
                this.gameObject.SetActive(false);
                //Set GameOver
                GameManager.Instance().ShowGameResult();
            }
        }
	}

    private void StartVibration()
    {
        clock.rotation = Quaternion.Euler(0, 0, angle);
        angle = -(angle);
    }

    private void StartScaling()
    {
        
    }
}
