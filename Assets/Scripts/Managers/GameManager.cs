using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Starting time for each level
    private readonly int[] STARTING_TIME = { 600, 400, 400 };

    public static GameManager instance = null; // Singleton

    private int timeLeft;

    [Header("Defeat")]
    private GameObject defeatMenu;
    private bool defeated = false;

    public Action<int> OnTimeChanged; // Notify listeners when timeLeft changes
    public PlayerStats playerStats;

    public int TimeLeft => timeLeft;


    void Awake()
    {
        // Set singleton instance
        if (instance == null)
            instance = this;
        // If instance already exists and it's not this destroy current gameObject
        else if (instance != this)
            Destroy(gameObject);

        // This script persists across different scenes
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        InitGame();
    }



    public void InitGame()
    {
        Debug.Log("Initializing game...");

        StopAllCoroutines();

        defeated = false;
        // Restart the game if it was frozen
        Time.timeScale = 1f;
        timeLeft = STARTING_TIME[0];

        StartCoroutine(Countdown());

        OnTimeChanged?.Invoke(timeLeft);

        // Reset player stats
        playerStats = new PlayerStats();
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindDefeatMenu();

        // Set initial time based on level (scenes indexes are 1, 2, 3 but array starts at 0)
        switch (scene.buildIndex)
        {
            case 1:
                timeLeft = STARTING_TIME[0];
                break;
            case 2:
                timeLeft = STARTING_TIME[1];
                break;
            case 3:
                timeLeft = STARTING_TIME[2];
                break;

            // If we are in main menu or end menu, do nothing
            default:
                break;
        }
    }



    private void FindDefeatMenu()
    {
        defeatMenu = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.CompareTag("DefeatMenu"));

        if (defeatMenu == null)
        {
            Debug.LogWarning("DefeatMenu not in scene");
        }
    }



    // Decrease timeLeft each second; if reaches 0, player is defeated
    IEnumerator Countdown()
    {
        while (timeLeft > 0 && !defeated)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1f);
            timeLeft--;
            OnTimeChanged?.Invoke(timeLeft);

            if (timeLeft <= 0)
            {
                Defeat();
                yield break;
            }
        }
    }



    public void DecreaseTime(int timeSpent)
    {
        if (defeated)
            return;

        timeLeft -= timeSpent;
        
        if (timeLeft <= 0)
            timeLeft = 0;

        OnTimeChanged?.Invoke(timeLeft);

        if (timeLeft == 0)
        {
            Defeat();
        }
    }



    // Utility functions to introduce a delay before executing an action
    public static void ExecuteAfterDelay(float delay, Action action)
    {
        if (instance != null)
        {
            instance.StartCoroutine(ExecuteCoroutine(delay, action));
        }
    }
    


    private static IEnumerator ExecuteCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }



    private void Defeat()
    {
        if (defeated)
            return;

        defeated = true;

        // Stop the game
        PauseManager.instance.SetPaused(PauseState.Defeat);

        if (defeatMenu != null)
        {
            defeatMenu.SetActive(true);
        }
        else
        {
            Debug.LogError("DefeatMenu not assigned.");
        }
    }



    public void NewGame()
    {
        InitGame();
    }
}