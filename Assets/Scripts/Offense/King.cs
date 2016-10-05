using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class King : Offense
{
    public float kingHealth;
    enum KingStates { IDLE, WALK, ATTACK };
    KingStates currentState;
    Camera mainCamera;
    public LayerMask pathLayerMask;
    public LayerMask defenseLayerMask;

    float timeBetweenShoots = 1;
    float lastShootTime = 0;
    float damage = 1;
    Animator kingAnimator;
   // LinkedList<Transform> targets = null;
    Transform currentTarget = null;
    bool attack = false;
    bool isWalking = false;
    bool isAttacking = false;
    bool isIdle = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        this.SetMyProperties();
        kingAnimator.SetTrigger("idleTrigger");
        isIdle = true;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.GetComponent<Image>().fillAmount = health / kingHealth;
        // movement
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())//on mouse click go to destination
        {
            Vector3 clickPos = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(clickPos);
            Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

            RaycastHit rayhit;
            if (Physics.Raycast(ray.origin, ray.direction * 10000, out rayhit, 5000, pathLayerMask))
            {
                Vector3 final = rayhit.point;
                currentState = KingStates.WALK;
                agent.SetDestination(final + Vector3.up * 0.4f);
                if (!isWalking)
                {
                    isAttacking = false;
                    isIdle = false;
                    isWalking = true;
                    kingAnimator.ResetTrigger("idleTrigger");
                    kingAnimator.ResetTrigger("attackTrigger");
                    kingAnimator.SetTrigger("moveTrigger");
                }
            }
        }

        // if destination reached and there is target in range(in targets linked list) then attack else idle
        if (currentState == KingStates.WALK && this.transform.position == agent.destination) {
            if (currentTarget != null) {
                currentState = KingStates.ATTACK;
                agent.SetDestination(currentTarget.position);
            }
            else if (entityLL.Count > 0)
            {
                currentState = KingStates.ATTACK;
                currentTarget = entityLL.First.Value.GetTransfrom();
                agent.SetDestination(currentTarget.position);
            }
            else
            {
               currentState = KingStates.IDLE;
            }
        }

        //shoot at regular interval
        if (attack && currentTarget != null)
        {
            if (Time.time > lastShootTime + timeBetweenShoots)
            {
                lastShootTime = Time.time;
                Shoot();
            }
            else
            {
            }
        }

        if(currentState == KingStates.IDLE)
        {
            if(!isIdle)
            {
                isIdle = true;
                isAttacking = false;
                isWalking = false;
                kingAnimator.ResetTrigger("moveTrigger");
                kingAnimator.ResetTrigger("attackTrigger");
                kingAnimator.SetTrigger("idleTrigger");
            }
        }

    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "TowerBase")
        {
            entityLL.AddLast(new MyTargets(col.transform.parent.transform,true,myFirstName));
            Entity defenseEntity = entityLL.Last.Value.GetTransfrom().GetComponent<Entity>();
            defenseEntity.OnDeath += ChangeTarget;
            //targets.AddLast(col.gameObject.transform);
            if (col.gameObject.transform.parent.tag == "BlockBarricade")
            {
                currentTarget = entityLL.Last.Value.GetTransfrom();
                agent.SetDestination(this.transform.position);
                currentState = KingStates.ATTACK;
                attack = true;
            }
            else if(currentTarget == null && (currentState == KingStates.IDLE || currentState == KingStates.WALK))
            {
                currentTarget = entityLL.First.Value.GetTransfrom();
                currentState = KingStates.ATTACK;
                attack = true;
            }
        }
        
    }

    void OnTriggerExit(Collider col) {
        if (col.transform.tag == "TowerBase")
        {
            Transform defenseTransform = col.transform.parent.transform;
            Entity defenseEntity = defenseTransform.GetComponent<Entity>();
            defenseEntity.OnDeath -= ChangeTarget;
            if (currentTarget == defenseTransform)
            {  
                attack = false;
                if (IsTargetPresent(currentTarget))
                { 
                    entityLL.Remove(FindFromTargets(currentTarget));
                    //Debug.Log("target removed");
                    currentTarget = null;
                    if (entityLL.Count > 0 && currentState == KingStates.ATTACK)
                    {
                        currentTarget = entityLL.First.Value.GetTransfrom();
                        agent.SetDestination(currentTarget.position);
                        attack = true;
                    }
                }
            }
            else
            {
                if(IsTargetPresent(defenseTransform))
                    entityLL.Remove(FindFromTargets(defenseTransform));
            }
        }
    }

    void Shoot() {
        //Debug.Log(currentTarget);
        if(!isAttacking)
        {
            isWalking = false;
            isIdle = false;
            isAttacking = true;
            kingAnimator.ResetTrigger("moveTrigger");
            kingAnimator.ResetTrigger("idleTrigger");
            kingAnimator.SetTrigger("attackTrigger");
        }
        currentTarget.gameObject.GetComponent<Entity>().TakeDamage(damage);
    }

    void ChangeTarget(Transform t) {//called when tower dies
        isIdle = true;
        isAttacking = false;
        isWalking = false;
        kingAnimator.ResetTrigger("moveTrigger");
        kingAnimator.ResetTrigger("attackTrigger");
        kingAnimator.SetTrigger("idleTrigger");
        if (IsTargetPresent(t))
        {
            if (currentTarget == t)
            {
                attack = false;
                entityLL.Remove(FindFromTargets(t));
                currentTarget = null;
                if (entityLL.Count > 0)
                {
                    currentTarget = entityLL.First.Value.GetTransfrom();
                    agent.SetDestination(currentTarget.position);
                    attack = true;
                }
            }
            else
            {
                entityLL.Remove(FindFromTargets(t));
                currentState = KingStates.IDLE;
            }
        }
        else
        {
            Debug.LogError("Couldn't change target as the previous target is not getting removed");
        }
        
    }

    protected override void SetMyProperties()
    {
        mainCamera = Camera.main;
        health = kingHealth;
        currentState = KingStates.IDLE;
        agent = GetComponent<NavMeshAgent>();
        kingAnimator = GetComponent<Animator>();
    }
}
