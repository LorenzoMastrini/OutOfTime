using System.Collections;
using UnityEngine;

public class OpenGate : MonoBehaviour, IInteractable
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject requiredObject;
    [SerializeField] GameObject gate;

    [SerializeField] string infoMessage;

    [SerializeField] AudioClip audioFX;
    [SerializeField] float pitch = 1;

    public bool hasBeenUsed { get; set; } = false;
    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Do nothing.";
    }



    // When user interact, start the movement
    public void Interact()
    {
        if (playerInventory.AlreadyInInventory(requiredObject))
        {
            // Avoid multiple interactions
            hasBeenUsed = true;

            info = "You finally made it!";

            // Destroy item
            playerInventory.RemoveEquippedItem();

            SoundManager.instance.PlaySound(audioFX, transform, 1, pitch);

            StartCoroutine(LowerBarrier());
        }
        else
        {
            Debug.Log("You can't just hack the computer and open the gate.");
        }
    }



    private IEnumerator LowerBarrier()
    {
        while(gate.transform.position.y > -2)
        {
            gate.transform.position += Vector3.down * Time.deltaTime * 1.3f;

            yield return null;
        }
    }
}
