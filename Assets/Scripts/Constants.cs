using UnityEngine;
using System.Collections;

public static class Constants{

    //SCENE DETAILS
    public const int START_MENU_SCENE = 0;
    public const int SHOP_MENU_SCENE = 1;
    public const int GAME_SCENE = 2;

    //PLAYER DETAILS
    public const int PLAYER_INITIAL_COINS = 500;
    public const int PLAYER_INITIAL_GEMS = 200;

    //SOLDIER DETAILS

    public const int SOLDIER_TYPES = 3;

    public const int KING = 99;
    public const int SWORD_SOLDIER = 0;
    public const int ARROW_SOLDIER = 1;
    public const int HAMMER_SOLDIER = 2;

    public const int SOLDIER_HEALTH = 0;
    public const int SOLDIER_DAMAGE = 1;
    public const int SOLDIER_RATE = 2;
    public const int SOLDIER_COST = 3;
    public const int SOLDIER_BUY = 4;

    public const int TOTAL_LOCKED = 0; //Cannot be bought
    public const int BOUGHT_LEVEL1 = 1;
    public const int BOUGHT_LEVEL2 = 2;
    public const int BOUGHT_LEVEL3 = 3;
    public const int BUYABLE = 4; //Can be bought and the soldier is of level 0

    //DEFENSE ENTITIES
    public const int ARROW_TOWER = 0;
    public const int BOMB_TOWER = 1;
    public const int BLOCK_BARRICADE = 2;
    public const int GROUND_BARRICADE = 3;
    public const int GATE = 4;
   
}
