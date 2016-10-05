using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

    public GameObject swordSoldierPrefab;
    public GameObject arrowSoldierPrefab;
    public GameObject hammerSoldierPrefab;
    Vector3 spawnPoint;
    bool isClickable = true;
    float currentAmount = 0;
    public float spwanSpeed = 10;

    // Use this for initialization
    void Start () {
        spawnPoint = GameManager.Instance().spawnPosition;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(currentAmount);
        if (currentAmount < 100f)
        {
            //Debug.Log(currentAmount + "is less than 100");
            currentAmount += spwanSpeed * Time.deltaTime;
            isClickable = false;
        }
        else {
            isClickable = true;
        }
        this.GetComponent<Image>().fillAmount = currentAmount / 100;
	}
    public void SpawnSwordSoldier()
    {
        if (isClickable)
        {
            Instantiate(swordSoldierPrefab, spawnPoint, Quaternion.identity);
            currentAmount = 0;
        }
    }
    public void SpawnArrowSoldier()
    {
        if (isClickable)
        {
            Instantiate(arrowSoldierPrefab, spawnPoint, Quaternion.identity);
            currentAmount = 0;
        }
    }
    public void SpawnHammerSoldier()
    {
        if (isClickable)
        {
            Instantiate(hammerSoldierPrefab, spawnPoint + Vector3.up * 0.6f, Quaternion.identity);
            currentAmount = 0;
        }
    }
}
