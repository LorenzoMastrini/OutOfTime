using System.Collections;
using TMPro;
using UnityEngine;

public class WorldLimits : MonoBehaviour
{
    private const string WORLD_LIMIT_MESSAGE = "You go for a walk: -10 time";

    [SerializeField] private Vector3 startingPosition;
    [SerializeField] private int timePenalty = 10;

    [SerializeField] private GameObject outsiteLimitsPanel; // UI panel
    [SerializeField] private TextMeshProUGUI textUI; // Text component on the panel
    [SerializeField] private float panelDuration = 2f;



    private void OnTriggerExit(Collider other)
    {
        // Get PlayerInput component to completely disable player input
        FirstPersonMovement player = other.GetComponent<FirstPersonMovement>();

        // Fade-in and fade-ouy UI panel
        StartCoroutine(ShowPanel(player));

        GameManager.instance.DecreaseTime(timePenalty);

        // Teleport player to starting position of the level
        Teleport(other);
    }



    // Show UI panel for a certain duration
    private IEnumerator ShowPanel(FirstPersonMovement player)
    {
        // Disable player input
        player.DisablePlayerInput();

        // Update message on screen
        textUI.text = WORLD_LIMIT_MESSAGE;

        outsiteLimitsPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(panelDuration);
        outsiteLimitsPanel.SetActive(false);

        // Re-enable player input
        player.EnablePlayerInput();
    }



    void Teleport(Collider other)
    {
        // Disable character controller to teleport the player via transform
        other.enabled = false;

        FirstPersonMovement player = other.GetComponent<FirstPersonMovement>();

        // Adjust the y-coordinate based on current active timeline
        switch (player.GetCurrentTimelineIndex())
        {
            case 0:
                startingPosition.y = player.TIMELINE_HEIGHTS[0];
                break;
            case 1:
                startingPosition.y = player.TIMELINE_HEIGHTS[1];
                break;
            case 2:
                startingPosition.y = player.TIMELINE_HEIGHTS[2];
                break;
        }

        // Teleport
        other.transform.position = startingPosition;

        // Re-enable character controller
        other.enabled = true;
    }
}