using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ArrowSoldier : Soldier
{
    public GameObject arrow;
    public Transform nozzle;
    float myHealth;
    Animator arrowSoldierAnimator;
    bool startWalking = false;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        SetMyProperties();
        myHealth = health;
        arrowSoldierAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentState == States.WALK || currentState == States.SET) && !startWalking)
        {
            startWalking = true;
        }
        healthSlider.GetComponent<Image>().fillAmount = health / myHealth;
        //Debug.Log(currentState);
        if (currentState == States.ATTACK && currentTarget != null)
        {
            startWalking = false;
            if (Time.time > lastShootTime + timeBetweenShoots)
            {
                lastShootTime = Time.time;
                Shoot();
            }
            else
            {
                //this.gameObject.GetComponent<Renderer>().material = originalMaterial;
            }
        }

        if (currentState == States.SET && this.transform.position == agent.destination + Vector3.up * 0.4f && currentTarget != null)
        {
            currentState = States.ATTACK;
        }

        if (startWalking)
        {
            arrowSoldierAnimator.ResetTrigger("attackTrigger");
            arrowSoldierAnimator.SetTrigger("moveTrigger");
        }
    }

    protected override bool CheckCondition(Transform t, int[] d)
    {// checks condition to add tower in list
        if (d[Constants.ARROW_SOLDIER] < 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnTriggerEnter(Collider col)
    {
        //base.OnSoldierColliderEntry(col);
        if (col.gameObject.tag == "TowerBase")
        {
            //Debug.Log("triggered enter");
            if (currentState == States.SET && col.transform.parent.transform == currentTarget)
            {
                    //agent.enabled = false;
                    currentState = States.ATTACK;
                    Vector3 tempPos = this.transform.position;
                    agent.destination = tempPos;

            }
        }
    }

    void Shoot()
    {
        // this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        arrowSoldierAnimator.ResetTrigger("moveTrigger");
        arrowSoldierAnimator.SetTrigger("attackTrigger");
        Entity entity = currentTarget.gameObject.GetComponent<Entity>();
        int type_of_target = GameManager.Instance().GetDefenseType(entity.GetMyName());
        //nozzle.LookAt(currentTarget.FindChild("DefenseBase").GetComponent<BoxCollider>().center);
        nozzle.LookAt(currentTarget.FindChild("Center").transform.position);
        //nozzle.LookAt(currentTarget.transform.position);
        //Debug.Log("global coordinates of the target are " + currentTarget.FindChild("DefenseBase").transform.position);
        GameObject tempArrow = Instantiate(arrow, nozzle.position, nozzle.rotation) as GameObject;
        tempArrow.GetComponent<Arrow>().myDamage = (damage * damagePercentage[type_of_target] / 100);
        tempArrow.GetComponent<Arrow>().targetLayer = 12;
        //entity.TakeDamage(damage * damagePercentage[type_of_target] / 100);
    }

    protected override void SetMyProperties()
    {
        int level = StatisticsManager.SM.GetDetails("Arrow_Soldier_State");
        int type = Constants.ARROW_SOLDIER;
        if (level >= 1)
        {
            if (level > 3)
                level = 1;
            myProperties = StatisticsManager.SM.GetSoldierProperties(type, level);
            myFirstName = myProperties.myFirstname;
            health = myProperties.health;
            //originalMaterial = myProperties.originalMaterial;
            timeBetweenShoots = myProperties.timeBetweenShoots;
            damage = myProperties.damage;
            damagePercentage = myProperties.damagePercentage;
            target_priority = myProperties.priority;
        }
        else
        {
            Debug.LogError("You have still not bought the Arrow Soldier");
        }
    }
}