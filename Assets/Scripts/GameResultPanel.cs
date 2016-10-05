using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameResultPanel : MonoBehaviour {

    public Button nextButton, mainMenuButton;
    public Text tower, barricades, gate, totalXP, gems, rank;
    public GameObject gameOverPanelObject;
    
    private static GameResultPanel gameOverPanel;

    public static GameResultPanel Instance()
    {
        if (!gameOverPanel)
        {
            gameOverPanel = FindObjectOfType(typeof(GameResultPanel)) as GameResultPanel;
            if (!gameOverPanel)
                Debug.LogError("There needs to be one active gameOverPanel script on a GameObject in your scene.");
        }

        return gameOverPanel;
    }
    
    public void ShowResult(UnityAction nextMapAction, UnityAction mainMenuAction,int towersXp, int barricadesXp, int gateXp)
    {
        gameOverPanelObject.SetActive(true);
        tower.text = "Towers : " + towersXp.ToString();
        barricades.text = "Barricades : " + barricadesXp.ToString();
        gate.text = "Gate : " + gateXp.ToString();
        int previousXp = StatisticsManager.SM.GetDetails("Player_XP");
        totalXP.text = "Total XP : " + (previousXp + towersXp + barricadesXp + gateXp).ToString();
        StatisticsManager.SM.SetDetails("Player_XP", totalXP.text.ToString());

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(nextMapAction);
        nextButton.onClick.AddListener(ClosePanel);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(mainMenuAction);
        mainMenuButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        gameOverPanelObject.SetActive(false);
    }
}
