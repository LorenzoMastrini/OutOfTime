using UnityEngine;

public class BuryMoney : MonoBehaviour, IInteractable
{
    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject requiredObject;

    [SerializeField] GameObject objectToActivate;
    [SerializeField] Vector3 position;
    [SerializeField] Quaternion rotation;

    [SerializeField] string infoMessage;

    [SerializeField] AudioClip[] sounds;
    [SerializeField] GameObject objectToSpawn;

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
            hasBeenUsed = true;
            objectToActivate.SetActive(true);
            playerInventory.RemoveEquippedItem();

            foreach(AudioClip clip in sounds)
            {
                SoundManager.instance.PlaySound(clip, transform, 1, 1);
            }

            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
            }
        }
        else
        {
            Debug.Log("You need somehting to bury in order to bury it.");
        }
    }
}