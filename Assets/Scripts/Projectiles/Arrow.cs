using UnityEngine;
using System.Collections;

public class Arrow : Projectiles
{
    [HideInInspector]
    public float myDamage;
    [HideInInspector]
    public LayerMask targetLayer;
    protected override void Start()
    {

        Destroy(gameObject, 2);
        base.Start();
        speed = 5;
        myDamage = 1;
    }



    void Update()
    {
        float moveDistance = this.speed * Time.deltaTime;
        CheckCollision(moveDistance);//check collision before hitting using raycast
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollision(float distance)//check collision before hitting using raycast
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.gameObject.layer == targetLayer)
            {
                OnHitObject(hit);
            }
            else if (hit.collider.gameObject.layer == pathMask)
                Destroy(gameObject);
            }
        }

    private void OnHitObject(RaycastHit hit)//gives damage to colliding tower and destroys bullet
    {
        //Debug.Log("onhitobject");
        IDamagable damagableObject = hit.collider.gameObject.transform.parent.GetComponent<IDamagable>();
        if (damagableObject != null)
        {
            damagableObject.TakeDamage(myDamage);
            //Debug.Log("damage given");
        }
        GameObject.Destroy(this.gameObject);
    }

}

