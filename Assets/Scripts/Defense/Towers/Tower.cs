using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TowerController))]
public abstract class Tower : DefenseEntity
{ 
    //Add the states for tower
    protected ScriptableTowerProps properties;
    protected TowerController towerController;
    protected float initialForce;
    protected Transform currentTarget;
    protected float timeBetweenShoot;
    protected float lastShootTime = 0;

    protected override void Start()
    {
        base.Start();
        myFirstName = "Tower";
        offenseLayer = 9;
        towerController = GetComponent<TowerController>();
    }

    protected void OnEntry(Collider col)//called from OnTriggerEnter of childTowers
    {
        Entity entity = col.gameObject.GetComponent<Entity>();
        entity.OnDeath += RemoveEntity;
        entityLL.AddLast(new MyTargets(col.gameObject.transform, false, entity.GetMyName()));
    }

    protected void OnExit(Collider col)
    {
        Transform exitingTransform = col.gameObject.transform;
        Entity entity = col.gameObject.GetComponent<Entity>();
        entity.OnDeath -= RemoveEntity;
        RemoveEntity(exitingTransform);
    }


    protected void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    protected void RemoveEntity(Transform entity)
    {
        entityLL.Remove(FindFromTargets(entity));
        if (entityLL.Count > 0)
        {
            if (currentTarget == entity)
            {
                ChangeTarget();
            }     
        }
        else
        {
            currentTarget = null;
        }
    }

    public int[] GetAttackingEntitiesCount()
    {
        int[] entities = new int[Constants.SOLDIER_TYPES];
        foreach(MyTargets t in entityLL)
        {
            string type = t.GetTargetType();
            if(type == "Sword_Soldier")
            {
                entities[Constants.SWORD_SOLDIER]++;
            }else if(type == "Arrow_Soldier")
            {
                entities[Constants.ARROW_SOLDIER]++;
            }else if(type == "Hammer_Soldier")
            {
                entities[Constants.HAMMER_SOLDIER]++;
            }
        }
        return entities;
    }

    public float GetInitialForce()
    {
        return initialForce;
    }

    public abstract void ChangeTarget();
}



