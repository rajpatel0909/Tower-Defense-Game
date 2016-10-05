using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BlockBarricade : DefenseEntity {

    float myHealth;
    protected override void Start() {
        base.Start();
        this.SetMyProperties();
    }

    // Update is called once per frame
    void Update () {
        healthSlider.GetComponent<Image>().fillAmount = health / myHealth;
    }

    protected override void SetMyProperties()
    {
        myFirstName = "Block_Barricade";
        health = 5;
        myHealth = health;
    }
}
