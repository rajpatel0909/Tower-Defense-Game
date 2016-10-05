using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ScriptableTowerProps : ScriptableObject
{
    public string myFirstname = "entityFirstName";
    public float timeBetweenShoots = 0;
    public float initialForce;
    public int health;
    public List<int> damagePercentage;
}
