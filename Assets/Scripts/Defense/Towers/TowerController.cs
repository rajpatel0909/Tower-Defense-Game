using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {

    private GameObject projectile;
    private Transform dome;
    private string type;

    public GameObject bulletPrefab;
    public Transform nozzlePosition;

    void Start()
    {
        dome = this.transform.FindChild("Armature").GetComponent<Transform>();
    }

    public void SetTowerType(string _type)
    {
        type = _type;
    }

	// Use this for initialization
    public void LookAtEnemy(Transform target)//called from update of arrowtower and bombtower
    {
        if(type == "Arrow_Tower")
        {
            dome.LookAt(target.FindChild("Center"));
        }
        else if(type == "Bomb_Tower")
        {
            CalcuteAngleForBomb(target);
        }
        
    }
    
    public void Shoot(float initialForce)//called from update() of bombtower and arrowtower
    {
        projectile = Instantiate(bulletPrefab, nozzlePosition.position, nozzlePosition.rotation) as GameObject;
        if (type == "Arrow_Tower")
        {
            projectile.GetComponent<Arrow>().SetSpeed(initialForce);
            projectile.GetComponent<Arrow>().myDamage = 1;
            projectile.GetComponent<Arrow>().targetLayer = 13;
        }
        else if (type == "Bomb_Tower")
        {
            projectile.GetComponent<Bomb>().SetSpeed(initialForce);
        }
    }

    private void CalcuteAngleForBomb(Transform target)
    {
        float v = this.GetComponent<Tower>().GetInitialForce();
        float x = Vector3.Distance(new Vector3(target.position.x, 0, target.position.z), new Vector3(nozzlePosition.parent.parent.parent.position.x, 0, nozzlePosition.parent.parent.parent.position.z));
        float y = -(nozzlePosition.parent.parent.parent.position.y + dome.transform.position.y - target.position.y);
        float g = 9.81f / 2f;
        float temp = (g * x * x) / (v * v);
        float thita = Mathf.Atan((x + Mathf.Sqrt((x * x) - 4 * temp * (y - temp))) / (2 * temp));
        thita = Mathf.Rad2Deg * thita;
        dome.LookAt(target);
        dome.transform.eulerAngles = new Vector3(-thita, dome.transform.eulerAngles.y, dome.transform.eulerAngles.z);
    }
}
