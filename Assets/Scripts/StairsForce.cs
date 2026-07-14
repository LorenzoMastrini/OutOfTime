using UnityEngine;

public class StairsForce : MonoBehaviour, IInteractable
{
    private FirstPersonMovement movement;

    [SerializeField] int requiredStrength = 10;
    [SerializeField] float slowMultiplier = 0.25f; // 25% slower

    [Header("Push Back")]
    [SerializeField] float minBackwardForce = 1f;
    [SerializeField] float maxBackwardForce = 2f;

    [Header("Stair Limits")]
    [SerializeField] Transform stairBottom;
    [SerializeField] Transform stairTop;

    public bool hasBeenUsed { get; set; } = true; // Stairs are not interactable (true so that interaction suggestion is not shown)
    public string info { get; set; } = "These stairs look extremely tiring to climb...";

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        movement = other.GetComponent<FirstPersonMovement>();

        if (GameManager.instance.playerStats.GetBody() < requiredStrength)
        {   
            // Slow the player
            movement.moveSpeedMultiplier = slowMultiplier;

            // Direzione scale
            Vector3 stairDirection =
                (stairTop.position - stairBottom.position).normalized;

            // Progressione del player sulle scale
            Vector3 playerOffset =
                other.transform.position - stairBottom.position;

            float stairLength =
                Vector3.Distance(stairBottom.position, stairTop.position);

            float progress =
                Vector3.Dot(playerOffset, stairDirection) / stairLength;

            progress = Mathf.Clamp01(progress);

            // Più sale -> più viene respinto
            float currentForce = Mathf.Lerp(
                minBackwardForce,
                maxBackwardForce,
                progress
            );

            // Push contrario alla salita
            Vector3 pushDirection = -stairDirection;
            pushDirection.y = 0f;

            movement.ForceMove(
                pushDirection.normalized *
                currentForce *
                Time.deltaTime
            );
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        movement = other.GetComponent<FirstPersonMovement>();

        movement.moveSpeedMultiplier = 1f;
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        if (movement != null)
        {
            movement.moveSpeedMultiplier = 1f;
        }
    }
}
