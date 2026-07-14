using UnityEngine;

public class ActivateScript : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] string infoMessage;

    public bool hasBeenUsed { get; set; } = false;
    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Activate";
    }



    // When user interact, start the movement
    public void Interact()
    {
        hasBeenUsed = true;
        objectToActivate.GetComponent<UpAndDownMovement>().enabled = true;

        GetComponentInChildren<Animator>().SetBool("LeverUp", true);
    }
}
