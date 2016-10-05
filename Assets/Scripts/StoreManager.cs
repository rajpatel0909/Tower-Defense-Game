using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class StoreManager : MonoBehaviour {

    public GameObject[] soldierPanels;
    public GameObject[] kingPanels;
    public Text gems;
    public Text coins;

    int player_gems;
    int player_coins;
    string health = "";
    string damage = "";
    string rate = "";
    string cost = "";
    bool isBuyable = false;
    bool isUpgradable = false;

    Button currentUpgradedButton;

	public void ChangeLevel(int i)
    {
        UIManager.UM.LoadScene(i);
    }

    void Start()
    {
        player_gems = StatisticsManager.SM.GetDetails("Player_Gems");
        player_coins = StatisticsManager.SM.GetDetails("Player_Coins");
        SetTheStore();
    }

    void SetTheStore()
    {
        gems.text =  player_gems.ToString();
        coins.text =  player_coins.ToString();
        for (int i = 0; i < soldierPanels.Length; i++)
        { 
            Text[] details = soldierPanels[i].GetComponentsInChildren<Text>();

            //wE HAVE TO SET THE IMAGE IN FUTURE
            //Image image = soldierPanels[i].GetComponentInChildren<Image>();

            Button buyButton = soldierPanels[i].GetComponentInChildren<Button>();
            SetSoldierValues(i);
            details[Constants.SOLDIER_DAMAGE].text = damage;
            details[Constants.SOLDIER_HEALTH].text = health;
            details[Constants.SOLDIER_RATE].text = rate;
            details[Constants.SOLDIER_COST].text = cost;
            if(isBuyable)
            {
                details[Constants.SOLDIER_BUY].text = "BUY";
            }
            else if(isUpgradable)
            {
                details[Constants.SOLDIER_BUY].text = "UPGRADE";
            }
            else
            {
                buyButton.enabled = false;
            }
        }
    }

    public void OnBuyButtonClicked(int soldier_type)
    {
        bool isBought = false;
        int level = -1;
        string state_string = "";
        ScriptableSoldierProps props = null;
        if (soldier_type == Constants.SWORD_SOLDIER)
        {
            state_string = "Sword_Soldier_State";
        }
        else if (soldier_type == Constants.ARROW_SOLDIER)
        {
            state_string = "Arrow_Soldier_State";
        }
        else if (soldier_type == Constants.HAMMER_SOLDIER)
        {
            state_string = "Hammer_Soldier_State";
        }
        int state = StatisticsManager.SM.GetDetails(state_string);
        if (state == Constants.BUYABLE)
        {
            level = 1;
            props = StatisticsManager.SM.GetSoldierProperties(soldier_type, level);
            isBought = BuySoldier(props.cost, soldier_type, true);
            if (isBought)
            {
                state = Constants.BOUGHT_LEVEL1;
            }
        }
        else if (state == Constants.BOUGHT_LEVEL1)
        {
            level = 2;
            props = StatisticsManager.SM.GetSoldierProperties(soldier_type, level);
            isBought = BuySoldier(props.cost, soldier_type, true);
            if (isBought)
            {
                state = Constants.BOUGHT_LEVEL2;
            }
        }
        else if (state == Constants.BOUGHT_LEVEL2)
        {
            level = 3;
            props = StatisticsManager.SM.GetSoldierProperties(soldier_type, level);
            isBought = BuySoldier(props.cost, soldier_type, true);
            currentUpgradedButton.enabled = false;
            if (isBought)
            {
                state = Constants.BOUGHT_LEVEL3;
            }
        }
        StatisticsManager.SM.SetDetails(state_string, state.ToString());
        StatisticsManager.SM.SetDetails("Player_Gems", player_gems.ToString());
        if(isBought)
        {
            SetTheStore();
        }
    }

    private bool BuySoldier(int cost, int soldier_type, bool upgradable)
    {
        bool bought = false;
        player_gems = StatisticsManager.SM.GetDetails("Player_Gems");
        if (player_gems >= cost)
        {
            //Let him buy
            //Decrease his gems and Update in the Dictionary and GemsText
            //Change the text of buy button to upgrade
            //Change the level to 1
            player_gems -= cost;
            gems.text = player_gems.ToString();
            if (upgradable)
            {
                currentUpgradedButton = soldierPanels[soldier_type].GetComponentInChildren<Button>();
                Text t = currentUpgradedButton.GetComponentInChildren<Text>();
                if(!t.text.Equals("UPGRADE"))
                {
                    t.text = "UPGRADE";
                }
            }
            bought = true;
        }
        else
        {
            //Display dialog you don't have enough gems
            bought = false;
        }
        return bought;
    }

    void SetSoldierValues(int type)
    {
        ScriptableSoldierProps props;
        int level = -1;
        String state = "";

        if (type == Constants.SWORD_SOLDIER)
            state = "Sword_Soldier_State";
        else if (type == Constants.ARROW_SOLDIER)
            state = "Arrow_Soldier_State";
        else if (type == Constants.HAMMER_SOLDIER)
            state = "Hammer_Soldier_State";

        if (StatisticsManager.SM.GetDetails(state) == Constants.TOTAL_LOCKED)
        {
            //Sword soldier is cannot be bought
            //Set the panel a bit dull
            //Values of Level 1 of the soldier will be displayed
            level = 1;
            isBuyable = false;
            isUpgradable = false;
        }
        else if (StatisticsManager.SM.GetDetails(state) == Constants.BUYABLE)
        {
            //Sword soldier is can be bought
            //Set the panel active
            //Values of Level 1 of the soldier will be displayed
            level = 1;
            isBuyable = true;
            isUpgradable = false;
        }
        else if (StatisticsManager.SM.GetDetails(state) == Constants.BOUGHT_LEVEL1)
        {
            //Sword soldier is of level 1
            //Set the details of level 2
            level = 2;
            isBuyable = false;
            isUpgradable = true;
        }
        else if (StatisticsManager.SM.GetDetails(state) == Constants.BOUGHT_LEVEL2)
        {
            //Sword soldier is of level 2
            //Set the details of level 3
            level = 3;
            isBuyable = false;
            isUpgradable = true;
        }
        else if(StatisticsManager.SM.GetDetails(state) == Constants.BOUGHT_LEVEL3)
        {
            //Sword soldier is of level 3
            //Set the details of level 3
            level = 3;
            isBuyable = false;
            isUpgradable = false;
        }
        props = StatisticsManager.SM.GetSoldierProperties(type, level);
        health = props.health.ToString();
        damage = props.damage.ToString();
        rate = props.timeBetweenShoots.ToString();
        cost = props.cost.ToString();
    }

}
