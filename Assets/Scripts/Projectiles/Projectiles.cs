using UnityEngine;
using System.Collections;

public class Projectiles : MonoBehaviour {

    protected float speed;
    protected float damage;

    protected LayerMask collisionMask;
    protected LayerMask pathMask;
    protected LayerMask DefenseMask;
    // Use this for initialization
    protected virtual void Start () {
        collisionMask = 9;
        pathMask = 8;
        DefenseMask = 12;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}
