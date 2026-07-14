using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioSource soundObject;

    public void Awake()
    {
        if (instance == null) {
            instance = this;
        }
    }

    public void PlaySound(AudioClip clip, Transform spawnTransform, float volume, float pitch)
    {
        // Spawn game object
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        // Assign audio clip
        audioSource.clip = clip;
        audioSource.pitch = pitch;

        // Assign volume
        audioSource.volume = volume;

        // Play audio clip
        audioSource.Play();

        // Get audio clip duration
        float clipLength = audioSource.clip.length;

        // Destroy game object
        Destroy(audioSource, clipLength);
    }
}