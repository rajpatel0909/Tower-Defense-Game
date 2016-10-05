using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArrowTower : Tower {

    Levels level = Levels.Level1;

    float myHealth;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        properties = StatisticsManager.SM.GetTowerProperties(Constants.ARROW_TOWER);
        SetMyProperties();
        towerController.SetTowerType(myFirstName);
        myHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.GetComponent<Image>().fillAmount = health / myHealth;
        if (currentTarget != null)
        {
            towerController.LookAtEnemy(currentTarget);
            if (Time.time > lastShootTime + timeBetweenShoot)
            {
                lastShootTime = Time.time;
                towerController.Shoot(initialForce);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 9)
        {
            base.OnEntry(col);//Add to list and register tower ondeath
            base.SetTarget(FindTarget());
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == offenseLayer)
        {
            base.OnExit(col);//Remove from list And ChangeTarget() is called from removeentity()
            base.SetTarget(FindTarget());
        }
    }


    public Transform FindTarget() {//returns target according to AI
        if (entityLL.Count == 1) {
            return entityLL.First.Value.GetTransfrom();
        }
        switch (level) {
            case Levels.Level1:
                //AI FOR LEVEL1 -- first come first serve
                if (entityLL.Count > 0)
                {
                    return  entityLL.First.Value.GetTransfrom();
                }
                break;

            default:
                return null;
        }
        return null;
    }

    public override void ChangeTarget() {//find new target and set it to current // called from remove from tower.entity()
        base.SetTarget(FindTarget());
    }

    protected override void SetMyProperties()
    {
        myFirstName = properties.myFirstname;
        health = properties.health;
        timeBetweenShoot = properties.timeBetweenShoots;
        initialForce = properties.initialForce;
    }
}
