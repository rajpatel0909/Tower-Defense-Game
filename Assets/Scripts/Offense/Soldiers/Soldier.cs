using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Soldier : Offense
{
    Transform gateEnd;
    protected Transform currentTarget = null;


    protected enum States { WALK, SET, ATTACK };//SET is when soldier finds new target and moves towards it for attacking
    protected States currentState;
    protected GameObject OffenseHeadquaters;
    protected ScriptableSoldierProps myProperties;
   // protected Material originalMaterial;
    protected float timeBetweenShoots;
    protected float lastShootTime = 0;
    protected float damage;
    protected List<int> damagePercentage = null;
    protected int[] target_priority;

    protected override void Start()
    {
        base.Start();
        myFirstName = "Soldier";
        gateEnd = GameObject.Find("Gate").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(gateEnd.position + Vector3.up * 0.4f);
        currentState = States.WALK;
        OffenseHeadquaters = GameObject.Find("OffenseHeadQuaters");
    }


    //Add the code of checking whether the transform is present in the entityLL
    public void TargetEntry(Transform t)//called from censor whenever it detects a towerbase or barricade
    {
        if (!IsTargetPresent(t))
        {
            bool isTargetChanged = false;
            if (CheckCondition(t, OffenseHeadquaters.GetComponent<OffenseHeadquaters>().Defensedetails[t]))
            {
                Entity defenseEntity = t.gameObject.GetComponent<Entity>();
                defenseEntity.OnDeath += ChangeTarget;
                entityLL.AddLast(new MyTargets(t, false, defenseEntity.GetMyName()));
                OffenseHeadquaters oHQ = OffenseHeadquaters.GetComponent<OffenseHeadquaters>();
                //Debug.Log("Soldier's next target added : " + defenseEntity.myFirstName);
                if (currentTarget == null)
                {
                    isTargetChanged = true;
                    currentTarget = ComputeHighPriorityTarget();
                }
                else
                {
                    Transform newTarget = ComputeHighPriorityTarget();
                    if (newTarget != currentTarget)
                    {
                        oHQ.RemoveMeFromDefense(currentTarget, myFirstName);
                        isTargetChanged = true;
                        currentTarget = newTarget;
                    }
                }
                if (isTargetChanged)
                {
                    agent.SetDestination(currentTarget.position + Vector3.up * 0.4f);
                    currentState = States.SET;
                    oHQ.AddMeToDefense(currentTarget, myFirstName);
                }
            }
        }
        else
        {
            //Target is already added. Do something in future.
        }
    }

    protected abstract bool CheckCondition(Transform t, int[] entities_count);

   

    void ChangeTarget(Transform t)//registered to OnDeath event of towers enrolled in this soldier
    {
        try {
            if (currentTarget == t)
            {
                OffenseHeadquaters.GetComponent<OffenseHeadquaters>().RemoveMeFromDefense(t, myFirstName);
                agent.SetDestination(gateEnd.position + Vector3.up * 0.4f);
                currentTarget = null;
            }
            entityLL.Remove(FindFromTargets(t));
            if (entityLL.Count > 0)
            {
                currentTarget = ComputeHighPriorityTarget();
                currentState = States.SET;
                agent.SetDestination(currentTarget.position + Vector3.up * 0.4f);
                OffenseHeadquaters.GetComponent<OffenseHeadquaters>().AddMeToDefense(currentTarget, myFirstName);
            }
            else
            {
                currentState = States.WALK;
                agent.SetDestination(gateEnd.position + Vector3.up * 0.4f);
            }
        }catch
        {
            Debug.LogWarning("The NavmeshAgent Destroyed. Check for a weird soldier");
        }
    }

    private Transform ComputeHighPriorityTarget()
    {
        MyTargets priorityTarget = null;
        int temp_priority = 100;
        foreach(MyTargets t in entityLL)
        {
            int priority = target_priority[GameManager.Instance().GetDefenseType(t.GetTargetType())];
            if(priority < temp_priority)
            {
                temp_priority = priority;
                priorityTarget = t;
            }
        }
        if(priorityTarget == null)
        {
            Debug.LogError("Priority Target couldn't be found and thus it is null");
        }
        return priorityTarget.GetTransfrom();
    }

    void OnDestroy()
    {
        if (currentTarget != null) {
            OffenseHeadquaters.GetComponent<OffenseHeadquaters>().RemoveMeFromDefense(currentTarget, myFirstName);
        }
    }

    protected void OnSoldierColliderEntry(Collider col)
    {
        if (col.gameObject.tag == "TowerBase")
        {
            if (currentState == States.SET && col.transform.parent.transform == currentTarget)
            {
                //agent.enabled = false;
                currentState = States.ATTACK;
            }
        }
        //else if (col.gameObject.tag == "BlockBarricade")
        //{
        //    if (currentState == States.SET)
        //    {
        //        if(col.transform == currentTarget)
        //        {

        //        }else if(col.transform != currentTarget)
        //        {
        //            currentTarget = col.transform;
        //        }
        //        //agent.enabled = false;
        //        currentState = States.ATTACK;
        //    }
        //}
        //else if (col.gameObject.tag == "GroundBarricade")
        //{
        //    if (currentState == States.SET && col.transform == currentTarget)
        //    {
        //        //agent.enabled = false;
        //        currentState = States.ATTACK;
        //    }
        //}
    }
    

}
