using UnityEngine;

public class BlowUp : MonoBehaviour, IInteractable
{
    public bool hasBeenUsed { get; set; } = false;
    public string info { get; set; }

    [SerializeField] private GameObject objectToBlow;
    [SerializeField] private GameObject objectToRemove; // Optional
    [SerializeField] private GameObject objectToAdd; // Optional

    [SerializeField] string infoMessage;
    [SerializeField] string infoMessageAfterInteraction;

    [SerializeField] AudioClip sound;
    [SerializeField] GameObject[] FX;


    void Start()
    {
        info = infoMessage != null ? infoMessage : "Blow up";
    }



    public void Interact()
    {
        hasBeenUsed = true;

        foreach (GameObject obj in FX)
        {
            obj.SetActive(true);
        }

        if (sound != null)
        {
            SoundManager.instance.PlaySound(sound, transform, 1, 1);
        }

        if(infoMessageAfterInteraction != null)
        {
            info = infoMessageAfterInteraction;
        }

        // Destroy the object to blow
        Destroy(objectToBlow);

        if(objectToRemove != null)
        {
            // Remove old object
            Destroy(objectToRemove);
        }
        if(objectToAdd != null)
        {
            // Substitute with this
            objectToAdd.SetActive(true);
        }
    }
}