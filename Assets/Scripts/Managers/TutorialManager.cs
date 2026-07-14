using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum TutorialStep
{
    Introduction,
    Move,
    Past,
    Present,
    Future,
    Interact,
    Use,
    Enemy
}



public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    // Create a new dictionary: each couple is (tutorial step, done)
    public Dictionary<TutorialStep, bool> tutorialsDictionary = new Dictionary<TutorialStep, bool>();

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] private bool skipTutorials = false;

    private InputAction moveAction;
    private InputAction timeline1Action;
    private InputAction timeline2Action;
    private InputAction timeline3Action;
    private InputAction interactAction;
    private InputAction useAction;

    private bool waitingForInput;
    private TutorialStep currentStep;

    public bool IsTutorialRunning => waitingForInput;

    private readonly string[] messages =
    {
        "Follow the green temporal signature and pass the portal before the time runs out. Use past, present and future timelines to cross the obstacles you find.\n\n" +
            "Things you do in the past reflects in the present.\n" +
            "Objects from the future can be exploited there or brought in the present (but not in the past!).\n\n\n" +
            "Press ENTER to continue.",
        "Use WASD to move and use the mouse to look around.\n" +
            "Press SPACE to jump.",
        "Press 1 to jump in the past.",
        "Press 2 to return in the present.",
        "Press 3 to jump in the future.",
        "Press E to interact with objects.",
        "Some objects can be grabbed and used whenever you want.\n" +
            "Press F to use them.",
        "Watch out: this strange red trial want to steal all the time you got left!\n\n\nPress ENTER to continue."
    };



    private void Awake()
    {
        instance = this;
    }



    private void Start()
    {
        // Start the tutorial only in LV1
        if (SceneManager.GetActiveScene().buildIndex != 1 && SceneManager.GetActiveScene().buildIndex != 3)
            return;

        moveAction = InputSystem.actions.FindAction("Move");
        timeline1Action = InputSystem.actions.FindAction("Timeline1");
        timeline2Action = InputSystem.actions.FindAction("Timeline2");
        timeline3Action = InputSystem.actions.FindAction("Timeline3");
        interactAction = InputSystem.actions.FindAction("Interact");
        useAction = InputSystem.actions.FindAction("UseItem");

        tutorialPanel.SetActive(false);

        // Add all elements to the dictionary
        tutorialsDictionary.Add(TutorialStep.Introduction, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Move, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Past, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Present, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Future, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Interact, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Use, skipTutorials);
        tutorialsDictionary.Add(TutorialStep.Enemy, skipTutorials);

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(AutomaticTutorialSequence());
        }

        // Show this only on LV3
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            TriggerTutorial(TutorialStep.Enemy);
        }

        PauseManager.instance.OnPauseChanged += HandlePauseChanged;
    }



    private void HandlePauseChanged(PauseState paused)
    {
        if (!waitingForInput)
            return;

        tutorialPanel.SetActive(paused == PauseState.Tutorial);
    }



    private IEnumerator AutomaticTutorialSequence()
    {
        TutorialStep[] automaticSteps =
        {
            TutorialStep.Introduction,
            TutorialStep.Move,
            TutorialStep.Past,
            TutorialStep.Present,
            TutorialStep.Future
        };

        foreach (TutorialStep step in automaticSteps)
        {
            // Avoid to show panels after each 2 seconds if pause menu is open

            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(ShowTutorial(step));
        }
    }



    private IEnumerator ShowTutorial(TutorialStep step)
    {
        // Return if this step is already done
        if (tutorialsDictionary[step])
            yield break;

        currentStep = step;

        // Stop the game
        PauseManager.instance.PauseForTutorial();

        // Update UI panel
        tutorialPanel.SetActive(true);
        tutorialText.text = messages[(int)step];

        waitingForInput = true;

        while (waitingForInput)
        {
            if (PauseManager.instance.paused == PauseState.Menu)
            {
                yield return null;
            }

            if (PauseManager.instance.paused == PauseState.Tutorial)
            {
                CheckCurrentStep();
            }
            
            yield return null;
        }

        tutorialPanel.SetActive(false);

        // Un-freeze the game
        PauseManager.instance.ResumeFromTutorial();

        // Tutorial step done
        tutorialsDictionary[step] = true;
    }



    public void TriggerTutorial(TutorialStep step)
    {
        StartCoroutine(ShowTutorial(step));
    }



    private void CompleteCurrentStep()
    {
        waitingForInput = false;
    }



    private void CheckCurrentStep()
    {
        switch (currentStep)
        {
            case TutorialStep.Introduction:
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Move:
                if (moveAction.ReadValue<Vector2>() != Vector2.zero)
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Past:
                if (timeline1Action.WasPressedThisFrame())
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Present:
                if (timeline2Action.WasPressedThisFrame())
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Future:
                if (timeline3Action.WasPressedThisFrame())
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Interact:
                if (interactAction.WasPressedThisFrame())
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Use:
                if (useAction.WasPressedThisFrame())
                {
                    CompleteCurrentStep();
                }
                break;

            case TutorialStep.Enemy:
                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    CompleteCurrentStep();
                }
                break;
        }
    }
}