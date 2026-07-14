using UnityEngine;

public class SpawnerInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector3 position;
    [SerializeField] private Quaternion rotation;

    // Optional
    [SerializeField] private GameObject objectToAddInScene = null;
    [SerializeField] private GameObject objectToRemove = null;

    [SerializeField] string infoMessage;

    [SerializeField] AudioClip sound;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; }



    void Start()
    {
        info = infoMessage != null ? infoMessage : "Add stats";
    }



    public void Interact()
    {
        hasBeenUsed = true;

        SoundManager.instance.PlaySound(sound, transform, 1, 1);
        Spawn();

        RemoveObject();
        AddObjectInScene();
    }



    void Spawn()
    {
        GameObject newObject = Instantiate(objectToSpawn, position, rotation);
    }



    void RemoveObject()
    {
        if (objectToRemove != null)
        {
            Destroy(objectToRemove);
        }
    }



    void AddObjectInScene()
    {
        if (objectToAddInScene != null)
        {
            objectToAddInScene.SetActive(true);
        }
    }
}
