using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScriptableSoldierProps : ScriptableObject {

    public string myFirstname = "entityFirstName";
    public Material originalMaterial;
    public float timeBetweenShoots = 0;
    public float damage;
    public int health;
    public List<int> damagePercentage;
    public int[] priority;
    public int cost;
}
