using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatisticsManager : MonoBehaviour {

    public SoldierType[] typesOfSoldiers;
    public static StatisticsManager SM;
    Dictionary<string, string> stats;
    public ScriptableTowerProps[] towers;
    public bool firstGame = false;

    void Awake()
    {
        if (!SM)
        {
            SM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadStatistics();
    }

    void LoadStatistics()
    {
        //create a dictionary of player statistics

        //Check whether data is present on the disk, then fetch it and store the player current stats
        /*
            Code goes here
        */

        //Get the internet connectivity and set the dictionary here
        /*
            Code goes here
        */

        //else check if the dictionary is null then so can go and set the InitialDetails
        stats = new Dictionary<string, string>();
        SetThePlayerInitialDetails();
    }

    void SetThePlayerInitialDetails()
    {
        SetDetails("Player_XP", 10000.ToString());
        SetDetails("Player_Rank", 1.ToString());
        SetDetails("Player_Coins", Constants.PLAYER_INITIAL_COINS.ToString());
        SetDetails("Player_Gems", Constants.PLAYER_INITIAL_GEMS.ToString());
        SetDetails("Sword_Soldier_State", Constants.BUYABLE.ToString());
        SetDetails("Arrow_Soldier_State", Constants.BUYABLE.ToString());
        SetDetails("Hammer_Soldier_State", Constants.BUYABLE.ToString());
        Debug.Log("Added EveryThing to Dictionary");
    }

    public void SetDetails(string key, string value)
    {
        if(stats.ContainsKey(key))
        {
            stats[key] = value;
        }
        else
        {
            stats.Add(key, value);
        }
    }

    public int GetDetails(string key)
    {
        if (stats.ContainsKey(key))
        {
            return int.Parse(stats[key]);
        }
        else
        {
            Debug.LogError("Key Not Found In Player Statistics Dictionary");
        }
        return -1;
    }

    public ScriptableSoldierProps GetSoldierProperties(int type, int level)
    {
        return typesOfSoldiers[type].soldierPropsArray[level-1];
    }

    public ScriptableTowerProps GetTowerProperties(int type)
    {
        return towers[type];
    }

    [System.Serializable]
    public class SoldierType
    {
        public ScriptableSoldierProps[] soldierPropsArray;
    }

}
