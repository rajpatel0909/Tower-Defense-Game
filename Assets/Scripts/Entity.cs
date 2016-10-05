using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Entity : MonoBehaviour, IDamagable
{
    
    protected string myFirstName;
    protected float health;
    protected LinkedList<MyTargets> entityLL;
    public delegate void DeathOccurence(Transform t);
    public event DeathOccurence OnDeath;
    public GameObject healthSlider;

    // Use this for initialization
    protected virtual void Start()
    {
        myFirstName = "Entity";
        entityLL = new LinkedList<MyTargets>();
    }


    public void TakeDamage(float damage)//reduce health according to damage and check if the entity is still alive
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        if (OnDeath != null)
        {
            OnDeath(this.transform);//call all method registered to this event of all objects and pass this entity's transform as parameter 
        }
        GameObject.Destroy(gameObject);
    }

    protected bool IsTargetPresent(Transform t)
    {
        if (entityLL.Count > 0)
        {
            bool isPresent = false;
            foreach (MyTargets targets in entityLL)
            {
                if (targets.GetTransfrom() == t)
                {
                    isPresent = true;
                    break;
                }
            }
            return isPresent;
        }
        else
        {
            return false;
        }
    }

    protected MyTargets FindFromTargets(Transform t)
    {
        if (entityLL.Count > 0)
        {
            MyTargets target = null;
            foreach (MyTargets targets in entityLL)
            {
                if (targets.GetTransfrom() == t)
                {
                    target = targets;
                    break;
                }
            }
            if (target != null)
            {
                return target;
            }
            else
            {
                Debug.LogError("The target being accessed is not present in entityLL");
            }

        }
        else
        {
            Debug.LogError("Size of entityLL is zero. Target couldn't be found");
        }
        return null;
    }

    protected class MyTargets
    {
        Transform myTransform;
        bool attackState;
        string target_type;

        public MyTargets(Transform _tranform, bool _state, string type)
        {
            myTransform = _tranform;
            attackState = _state;
            target_type = type;
        }

        public void SetAttackingMode(bool state)
        {
            attackState = state;
        }

        public string GetTargetType()
        {
            return target_type;
        }

        public bool GetAttackingMode()
        {
            return attackState;
        }

        public Transform GetTransfrom()
        {
            return myTransform;
        }

    }

    void OnDestroy()
    {
        if(FindObjectOfType<GameManager>() != null)
        GameManager.Instance().EntityDestoryed(myFirstName);
    }

    public string GetMyName()
    {
        return myFirstName;
    }

    protected abstract void SetMyProperties();
}
