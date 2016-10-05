using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BombTower : Tower
{
    Levels level;
    float startHealth;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        properties = StatisticsManager.SM.GetTowerProperties(Constants.BOMB_TOWER);
        SetMyProperties();
        towerController.SetTowerType(myFirstName);
        startHealth = health;
    }
    // Update is called once per frame
    void Update()
    {
        healthSlider.GetComponent<Image>().fillAmount = health / startHealth;
        if (currentTarget != null)
        {
            towerController.LookAtEnemy(currentTarget);
            if (Time.time > lastShootTime + timeBetweenShoot)
            {
                lastShootTime = Time.time;
                towerController.Shoot(initialForce);
            }
        }

        if (level == Levels.Level2) {
            base.SetTarget(FindTarget());
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
        if (entityLL.Count < 1) {
            return null;
        }

        if (entityLL.Count == 1) {
            return entityLL.First.Value.GetTransfrom();
        }

        switch (level) {

            case Levels.Level1:
                //AI FOR LEVEL1 -- first come first serve
                //Debug.Log("this is case level 1");
                return entityLL.First.Value.GetTransfrom();

            case Levels.Level2:
                //AI FOR LEVEL2 -- soldier in center
                //LinkedList<Transform> temp = entityLL;
                //Debug.Log("this is case level 2");
                Vector3 avg = new Vector3(0, 0, 0);
                foreach (MyTargets t in entityLL) {
                    avg += t.GetTransfrom().position;
                }
                avg = avg / entityLL.Count;
                //Debug.Log(avg);
                float distance = 100f;
                float tempDistance = 0f;
                Transform temp = null;
                foreach (MyTargets t in entityLL) {
                    tempDistance = Mathf.Sqrt((avg.x * t.GetTransfrom().position.x) + (avg.z * t.GetTransfrom().position.z));
                    if (tempDistance < distance) {
                        distance = tempDistance;
                        temp = t.GetTransfrom();
                    }
                }

                //temp.position = avg;
                //temp.rotation = Quaternion.identity;
                //temp.localScale = new Vector3(1,1,1);
                //Debug.Log(distance);
                return temp;

            default:
                //Debug.Log("this is case default");
                return null;
        }
    }

    public override void ChangeTarget()// finds and set it to current
    {
        base.SetTarget(FindTarget());
    }

    protected override void SetMyProperties()
    {
        myFirstName = properties.myFirstname;
        health = properties.health;
        timeBetweenShoot = properties.timeBetweenShoots;
        initialForce = properties.initialForce;
        level = Levels.Level2;
    }
}
