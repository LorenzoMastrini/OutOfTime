using System;
using UnityEngine;



public enum PauseState
{
    None,
    Tutorial,
    Menu,
    Defeat
}



public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public event Action<PauseState> OnPauseChanged;

    private PauseState isPaused = PauseState.None;

    private bool tutorialWasInterrupted = false;

    public bool TutorialWasInterrupted
    {
        get => tutorialWasInterrupted;
        set => tutorialWasInterrupted = value;
    }


    public PauseState paused => isPaused;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }



    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }

        // Evita che il gioco rimanga bloccato se viene distrutto mentre era in pausa
        Time.timeScale = 1f;
    }



    public void SetPaused(PauseState paused)
    {
        if (isPaused == paused)
            return;

        isPaused = paused;

        Time.timeScale =
            paused == PauseState.None ? 1f : 0f;

        OnPauseChanged?.Invoke(paused);
    }



    public void PauseForTutorial()
    {
        SetPaused(PauseState.Tutorial);
    }



    public void ResumeFromTutorial()
    {
        SetPaused(PauseState.None);
    }
}