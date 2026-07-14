using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] TextMeshProUGUI interactionText;
    [SerializeField] GameObject infoPanel;
    [SerializeField] TextMeshProUGUI objectInfoText;
    [SerializeField] float interactionDistance = 3f;

    IInteractable currentTargetedInteractable;

    private bool tutorialComplete = false;


    void Update()
    {
        UpdateCurrentInteractable();
        UpdateInteractionText();
        DisplayInfo();
        CheckForInteractionInput();
    }



    private void UpdateCurrentInteractable()
    {
        var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        Physics.Raycast(ray, out var hit, interactionDistance);
        
        currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();

        if (!tutorialComplete && currentTargetedInteractable != null && !PauseManager.instance.TutorialWasInterrupted)
        {
            Tutorial();
        }
    }



    private void UpdateInteractionText()
    {
        if (currentTargetedInteractable != null && !currentTargetedInteractable.hasBeenUsed)
        {
            interactionText.text = "E Interact";
        }
        else
        {
            interactionText.text = string.Empty;
        }
    }



    private void CheckForInteractionInput()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame && currentTargetedInteractable != null)
        {
            currentTargetedInteractable.Interact();
        }
    }



    private void DisplayInfo()
    {
        if (currentTargetedInteractable != null)
        {
            infoPanel.SetActive(true);
            objectInfoText.text = currentTargetedInteractable.info;
        }
        else
        {
            infoPanel.SetActive(false);
            interactionText.text = string.Empty;
        }
    }


    private void Tutorial()
    {
        if (TutorialManager.instance == null) return;

        // Show Interaction tutorial only on LV1
        if(SceneManager.GetActiveScene().buildIndex == 1)
            TutorialManager.instance.TriggerTutorial(TutorialStep.Interact);
    }
}