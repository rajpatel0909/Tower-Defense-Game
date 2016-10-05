using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class PausePanel : MonoBehaviour {

    public Button resumeButton, mainMenuButton;

    private static PausePanel pausePanel;

    public GameObject pausePanelObject;

    public static PausePanel Instance()
    {
        if (!pausePanel)
        {
            pausePanel = FindObjectOfType(typeof(PausePanel)) as PausePanel;
            if (!pausePanel)
                Debug.LogError("There needs to be one active gameOverPanel script on a GameObject in your scene.");
        }

        return pausePanel;
    }

    public void PauseGame(UnityAction resumeAction, UnityAction mainMenuAction)
    {
        Time.timeScale = 0f;
        pausePanelObject.SetActive(true);

        resumeButton.onClick.RemoveAllListeners();
        resumeButton.onClick.AddListener(resumeAction);
        resumeButton.onClick.AddListener(ClosePanel);

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(mainMenuAction);
        mainMenuButton.onClick.AddListener(ClosePanel);
    }

    void ClosePanel()
    {
        Time.timeScale = 1f;
        pausePanelObject.SetActive(false);
    }
}
