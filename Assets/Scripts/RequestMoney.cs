using Unity.VisualScripting;
using UnityEngine;

public class RequestMoney : MonoBehaviour, IInteractable
{
    [SerializeField] FirstPersonMovement player;
    [SerializeField] Invest investment;
    [SerializeField] PolicemanMovement policeman;

    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; } =
        "You are broke. To visit the square you must pay.";



    // When user interact, start the movement
    public void Interact()
    {
        if (investment.hasBeenUsed)
        {
            hasBeenUsed = true; // Avoid multiple interactions
            SoundManager.instance.PlaySound(sound, transform, 1, 1);
            policeman.StartMovement();
        }
    }

    void OnEnable()
    {
        // When returning to the present, if the investment event panel has not been shown yet, trigger the event
        if (investment.hasBeenUsed && !EventsManager.Instance.IsInvestmentEventDone())
        {
            EventsManager.Instance.InvokeInvestmentEvent();
        }
    }
}