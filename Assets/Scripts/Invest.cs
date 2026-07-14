using UnityEngine;

public class Invest : MonoBehaviour, IInteractable
{
    public const decimal MONEY_AFTER_INVESTING = 130730.60m;

    [SerializeField] Inventory playerInventory;
    [SerializeField] RequestMoney request;

    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; } =
        "You can invest your money in the stock market.";



    public void Interact()
    {
        hasBeenUsed = true;
        this.info = "You invested a small sum.";

        SoundManager.instance.PlaySound(sound, transform, 1, 1);
        
        // Modify info of the desk in timeline2
        request.info = "The effects of years and years of compound interest are being felt.";
    }
}