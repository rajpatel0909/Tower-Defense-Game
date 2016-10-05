using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]

public class Bomb : Projectiles
{
    public float blastRadius = 1.2f;
    public Transform explosion;
    public GameObject blastEffect;
    Rigidbody rb;
    bool speedSet = false;
    bool explode = false;
    LayerMask offenseBase;


    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        pathMask = 8;
        damage = 2;
        offenseBase = 13;
    }


    void Update()
    {
        if (!speedSet) {//set speed only once at beginning after SetSpeed()
            rb.velocity = transform.forward * speed;
            speedSet = true;
        }
    }

    void FixedUpdate()
    {
        if(explode)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach(Collider col in hitColliders)
            {
                if (col.gameObject.layer == collisionMask)
                {
                    IDamagable damagableObject = col.gameObject.GetComponent<IDamagable>();
                    if (damagableObject != null)
                    {
                        damagableObject.TakeDamage(damage);
                    }
                }
            }
            Object bombBlast = Instantiate(blastEffect, this.transform.position, this.transform.rotation);
            Destroy(bombBlast, 1);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == offenseBase || col.gameObject.layer == pathMask)//check if it is colliding with the soldier and not its censor AND destroy bomb
        {
            explode = true;
        }
    }

}
