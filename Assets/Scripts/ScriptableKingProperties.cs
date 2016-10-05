using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScriptableKingProperties : ScriptableObject
{
    public string myFirstname = "entityFirstName";
    public float timeBetweenShoots = 0;
    public float damage;
    public int health;
    public List<int> damagePercentage;
    public int cost;
}

