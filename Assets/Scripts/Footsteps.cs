using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Header("Grass")]
    [SerializeField] private AudioClip[] grassClips;

    [Header("Dirt")]
    [SerializeField] private AudioClip[] dirtClips;

    [Header("Rock")]
    [SerializeField] private AudioClip[] rockClips;

    [Header("Metal")]
    [SerializeField] private AudioClip[] metalClips;

    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.45f;
    [SerializeField] private float raycastDistance = 1.5f;
    [SerializeField] private LayerMask groundLayer;

    private CharacterController controller;
    private float stepTimer;
    private AudioClip lastClip;



    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }



    private void Update()
    {
        if (!IsMoving())
        {
            return;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }



    private bool IsMoving()
    {
        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        return horizontalVelocity.sqrMagnitude > 0.01f;
    }



    private void PlayFootstep()
    {
        AudioClip[] clips = GetSurfaceClips();

        if (clips == null || clips.Length == 0)
            return;

        AudioClip selectedClip;

        // Evita di ripetere lo stesso suono due volte di fila
        do
        {
            selectedClip = clips[Random.Range(0, clips.Length)];
        }
        while (clips.Length > 1 && selectedClip == lastClip);

        lastClip = selectedClip;

        // Piccola variazione per un effetto più naturale
        audioSource.pitch = Random.Range(0.95f, 1.05f);

        audioSource.PlayOneShot(selectedClip);
    }



    private AudioClip[] GetSurfaceClips()
    {
        Vector3 origin = transform.position + Vector3.up * 0.2f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
        {
            switch (hit.collider.tag)
            {
                case "Grass":
                    return grassClips;

                case "Dirt":
                    return dirtClips;

                case "Rock":
                    return rockClips;
                
                case "Metal":
                    return metalClips;
            }
        }

        return null;
    }
}