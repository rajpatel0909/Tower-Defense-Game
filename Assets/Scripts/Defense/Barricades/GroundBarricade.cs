using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GroundBarricade : DefenseEntity {

    public float groundbarricadeHealth = 5f;
    private WaitForSeconds waitLock = new WaitForSeconds(0.2f);
    float timeBetweenShoots = 1;
    float lastShootTime = 0;
    float damage = 1;

    bool isShooting = false;
    List<Transform> tempAddedList;
    List<Transform> tempRemovedList;

    float myHealth;

    // Use this for initialization
    protected override void Start () {
       base.Start();
       this.SetMyProperties();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == 9)
        {
            Entity entity = col.gameObject.GetComponent<Entity>();
            if (isShooting)
            {
                tempAddedList.Add(col.gameObject.transform);
            }
            else
            {
                entity.OnDeath += RemoveEntity;
                entityLL.AddLast(new MyTargets(col.transform, true, entity.GetMyName()));
            }
        }
    }

    void OnTriggerExit(Collider col) {
        Entity entity = col.gameObject.GetComponent<Entity>();
        if (col.gameObject.layer == offenseLayer)
        {
            if (isShooting)
            {
                tempRemovedList.Add(col.gameObject.transform);
            }
            else
            {
                entity.OnDeath -= RemoveEntity;
                entityLL.Remove(FindFromTargets(col.transform));
            }
        }
    }

    void RemoveEntity(Transform t) {
        if(IsTargetPresent(t))
            entityLL.Remove(FindFromTargets(t));
    }

	// Update is called once per frame
	void Update () {

        healthSlider.GetComponent<Image>().fillAmount = health / myHealth;
        if (entityLL.Count > 0) {
            if (Time.time > lastShootTime + timeBetweenShoots && !isShooting)
            {
                isShooting = true;
                lastShootTime = Time.time;
                StartCoroutine(Shoot());
            }
            else
            {
                if (!isShooting)
                {
                    if (tempAddedList.Count > 0)
                    {
                        foreach (Transform targetsAdded in tempAddedList)
                        {
                            if (targetsAdded != null)
                            {
                                Entity entity = targetsAdded.gameObject.GetComponent<Entity>();
                                entityLL.AddLast(new MyTargets(targetsAdded, true, entity.GetMyName()));
                                entity.OnDeath += RemoveEntity;
                            }
                        }
                        tempAddedList.Clear();
                    }
                    if (tempRemovedList.Count > 0)
                    {
                        foreach (Transform targetsRemoved in tempRemovedList)
                        {
                            if (targetsRemoved != null)
                            {
                                Entity entity = targetsRemoved.gameObject.GetComponent<Entity>();
                                if (IsTargetPresent(targetsRemoved))
                                {
                                    entityLL.Remove(FindFromTargets(targetsRemoved));
                                    entity.OnDeath += RemoveEntity;
                                }
                            }
                        }
                        tempRemovedList.Clear();
                    }
                }
            }
        }
	}

    IEnumerator Shoot()
    {
        for(LinkedListNode<MyTargets> node = entityLL.First; node != null; node = node.Next)
        {
            IDamagable damagableObject = node.Value.GetTransfrom().GetComponent<IDamagable>();
            if (damagableObject != null)
            {
                damagableObject.TakeDamage(damage);
            }
        }
        yield return waitLock;
        isShooting = false;
    }

    protected override void SetMyProperties()
    {
        myFirstName = "Ground_Barricade";
        tempAddedList = new List<Transform>();
        tempRemovedList = new List<Transform>();
        health = groundbarricadeHealth;
        myHealth = health;
    }
}


