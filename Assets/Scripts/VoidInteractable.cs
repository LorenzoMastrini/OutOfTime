using UnityEngine;



// This class do nothing, useful just to display informations in the UI
public class VoidInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string infoMessage;

    // Not interactable
    public bool hasBeenUsed { get; set; } = true;
    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Do nothing.";
    }



    // Do nothing
    public void Interact() {}
}
