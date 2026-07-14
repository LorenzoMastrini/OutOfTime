public interface IInteractable
{
    public bool hasBeenUsed { get; set; }

    public string info { get; set; }

    public void Interact();
}