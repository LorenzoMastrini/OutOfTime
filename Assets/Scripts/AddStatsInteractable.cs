using UnityEngine;

public class AddStatsInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] int timeCost = 1; // Time required to perform the action

    [Header("Stats to update")]
    [SerializeField] int body = 0;
    [SerializeField] int study = 0;
    [SerializeField] int culture = 0;

    [SerializeField] string infoMessage;
    [SerializeField] string infoMessageAfterUsage;

    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;
    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Add stats";
    }



    public void Interact()
    {
        if(!hasBeenUsed)
        {
            // Prevent multiple interactions
            hasBeenUsed = true;

            SoundManager.instance.PlaySound(sound, transform, 1, 1);

            // Add Body
            GameManager.instance.playerStats.AddBody(body);
            Debug.Log($"Current body stat: {GameManager.instance.playerStats.GetBody()}");

            // Study
            GameManager.instance.playerStats.AddStudy(study);
            Debug.Log($"Current study stat: {GameManager.instance.playerStats.GetStudy()}");

            // Culture
            GameManager.instance.playerStats.AddCulture(culture);
            Debug.Log($"Current culture stat: {GameManager.instance.playerStats.GetCulture()}");

            GameManager.instance.DecreaseTime(timeCost);

            info = infoMessageAfterUsage;
        }
    }
}
