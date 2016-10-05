using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScriptableRankProperties : ScriptableObject
{
    public int mapSize;
    public int turns;
    public int numOfArrowTowers;
    public int numOfBombTowers;
    public int numOfBlockBarricade;
    public int numOfGroundBarricade;
    public int towersXp;
    public int allTowers;
    public int barricadeXp;
    public int gateXp;
}
