using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    public static UIManager instance = null; // Singleton

    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] TextMeshProUGUI timeLeftText;
    [SerializeField] GameObject outsideLimitsPanel;
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] GameObject eventsPanel;
    [SerializeField] GameObject defeatMenu;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject backroundBlur;

    private Dictionary<GameObject, bool> panelStates = new Dictionary<GameObject, bool>();



    void Awake()
    {
        // Set singleton instances
        if (instance == null)
            instance = this;
        // If instance already exists and it's not this destroy current gameObject
        else if (instance != this)
            Destroy(gameObject);
    }



    void Start()
    {
        if (PauseManager.instance != null)
        {
            PauseManager.instance.OnPauseChanged += HandlePauseUI;
        }
    }



    private void HandlePauseUI(PauseState paused)
    {
        // Not paused
        if (paused == PauseState.None)
        {
            // Lock and hide cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }

            if (backroundBlur != null)
            {
                backroundBlur.SetActive(false);
            }
        }

        // Normal pause
        if (paused == PauseState.Menu)
        {
            // Enable and show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pausePanel.SetActive(true);
            backroundBlur.SetActive(true);
        }

        // Game paused by tutorial
        if (paused == PauseState.Tutorial)
        {
            // Enable and show cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pausePanel.SetActive(false);
            backroundBlur.SetActive(false);
        }

        if (paused == PauseState.Defeat)
        {
            // Unlock mouse for menu interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }



    private void OnDestroy()
    {
        if (PauseManager.instance != null)
        {
            PauseManager.instance.OnPauseChanged -= HandlePauseUI;
        }

        // Rimove the singleton
        if (instance == this)
        {
            instance = null;
        }
    }



    public bool IsESCPressedOnOptionsMenu()
    {
        // If in options, go back to pause menu
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
            pausePanel.SetActive(true);

            return true;
        }

        return false;
    }


    public void PlayLv1()
    {
        SceneManager.LoadScene(1);
        GameManager.instance.NewGame();
    }



    public void PlayLv2()
    {
        SceneManager.LoadScene(2);
        GameManager.instance.NewGame();
    }



    public void PlayLv3()
    {
        SceneManager.LoadScene(3);
        GameManager.instance.NewGame();
    }


    public void Resume()
    {
        switch (PauseManager.instance.paused)
        {
            // Game running -> pause it normally
            case PauseState.None:
                PauseManager.instance.SetPaused(PauseState.Menu);
                break;

            // Game on pause menu
            case PauseState.Menu:
                // If menu was opened over a tutorial panel
                if (PauseManager.instance.TutorialWasInterrupted)
                {
                    // Reset tutorial as before
                    PauseManager.instance.TutorialWasInterrupted = false;
                    PauseManager.instance.SetPaused(PauseState.Tutorial);
                }
                else
                {
                    // Close pause menu normally
                    PauseManager.instance.SetPaused(PauseState.None);
                }

                break;

            case PauseState.Tutorial:
                // Open pause menu, interrupting current tutorial
                PauseManager.instance.TutorialWasInterrupted = true;
                PauseManager.instance.SetPaused(PauseState.Menu);
                break;
        }
    }



    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }



    // Quit the application
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }



    // If player press ESC while options are open, go back to pause menu
    public bool IsOptionsPanelOpen()
    {
        return optionsPanel.activeSelf;
    }



    public void CloseOptionsOnESC()
    {
        optionsPanel.SetActive(false);
        pausePanel.SetActive(true);
    }



    public void RestartLevel()
    {
        GameManager.instance.NewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    public void DisableAllPanels()
    {
        panelStates.Clear();

        GameObject[] panels =
        {
            outsideLimitsPanel,
            infoPanel,
            tutorialPanel,
            eventsPanel,
            defeatMenu,
        };

        foreach (GameObject panel in panels)
        {
            if (panel != null)
            {
                panelStates[panel] = panel.activeSelf; // Save last state of the panel
                panel.SetActive(false); // Disable
            }
        }

        if (backroundBlur != null)
            backroundBlur.SetActive(false);
    }



    public void RestorePanels()
    {
        foreach (var panelState in panelStates)
        {
            panelState.Key.SetActive(panelState.Value);
        }

        // Riattiva il blur solo se almeno uno dei pannelli che lo usano era aperto
        if (backroundBlur != null)
        {
            bool blurNeeded = pausePanel.activeSelf || optionsPanel.activeSelf;
            backroundBlur.SetActive(blurNeeded);
        }
    }
}