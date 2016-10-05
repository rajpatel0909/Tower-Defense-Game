using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObjects{

	[MenuItem("Assets/Create/New Soldier Properties")]
    public static void CreateMySoldier()
    {
        ScriptableSoldierProps asset = ScriptableObject.CreateInstance<ScriptableSoldierProps>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/Soldiers/NewSoldierProps.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/New Tower Properties")]
    public static void CreateMyTower()
    {
        ScriptableTowerProps asset = ScriptableObject.CreateInstance<ScriptableTowerProps>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/Towers/NewTowerProps.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/New Rank Properties")]
    public static void CreateMyRank()
    {
        ScriptableRankProperties asset = ScriptableObject.CreateInstance<ScriptableRankProperties>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/Ranks/NewRankProps.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Create/New King1 Properties")]
    public static void CreateMyKing()
    {
        ScriptableKingProperties asset = ScriptableObject.CreateInstance<ScriptableKingProperties>();
        AssetDatabase.CreateAsset(asset, "Assets/Data/King1/NewKingProps.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
