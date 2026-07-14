using UnityEngine;

public class GrabObjectInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Inventory inventory;
    [SerializeField] GameObject objectToGrab;

    [SerializeField] string infoMessage;
    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; }


    void Start()
    {
        info = infoMessage != null ? infoMessage : "Grabbable object";

        // If Inventory can't be assigned in the inspector, find it at runtime
        if (inventory == null)
        {
            inventory = FindAnyObjectByType<Inventory>(); // There is only one player
        }
    }


    public void Interact()
    {
        GetObject();

        if (sound != null)
        {
            SoundManager.instance.PlaySound(sound, transform, 1, 1);
        }
    }



    void GetObject()
    {
        if(inventory.AlreadyInInventory(objectToGrab))
        {
            Debug.Log("Oggetto già presente nell'inventario");
            return;
        }

        bool added = inventory.AddItem(objectToGrab);

        if (added)
        {
            inventory.EquipItem(objectToGrab);
        }
    }
}