using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Gate : Entity {

    public float gateHealth = 15;
    float myHealth;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        this.SetMyProperties();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.GetComponent<Image>().fillAmount = health / myHealth;
    }

    protected override void SetMyProperties()
    {
        myFirstName = "Gate";
        health = gateHealth;
        myHealth = health;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
