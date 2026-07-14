using System;
using System.Collections;
using UnityEngine;

public class EnterBank : MonoBehaviour, IInteractable
{
    [SerializeField] FirstPersonMovement player;
    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject requiredObject;

    [SerializeField] GameObject blackScreenPanel;
    [SerializeField] int blackScreenDuration;
    [SerializeField] Vector3 teleportPosition;

    [SerializeField] string infoMessage;

    public bool hasBeenUsed { get; set; } = false;
    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Do nothing.";
    }



    public void Interact()
    {
        if (playerInventory.AlreadyInInventory(requiredObject))
        {
            StartCoroutine(ShowPanel());
            TeleportIntoElevator();
            playerInventory.RemoveEquippedItem();
        }
        else
        {
            Debug.Log("You need money to enter the bank.");
        }
    }



    // Show BlackScreen panel for a certain duration
    private IEnumerator ShowPanel()
    {
        // Disable player input
        player.enabled = false;

        blackScreenPanel.SetActive(true);
        yield return new WaitForSeconds(blackScreenDuration);
        blackScreenPanel.SetActive(false);

        // Re-enable player input
        player.enabled = true;
    }



    private void TeleportIntoElevator()
    {
        CharacterController characterController = player.transform.GetComponent<CharacterController>();

        characterController.enabled = false;
        player.transform.position = teleportPosition;
        characterController.enabled = true;
    }
}