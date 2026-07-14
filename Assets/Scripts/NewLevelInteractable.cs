using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelInteractable : MonoBehaviour, IInteractable
{
    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; } = "Complete level";

    public void Interact()
    {
        // Load next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}