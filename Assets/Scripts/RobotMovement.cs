using System.Collections;
using UnityEngine;

public class RobotMovement : MonoBehaviour, IInteractable
{
    private Vector3 startPosition;

    [SerializeField] Direction direction = Direction.x;
    [SerializeField] int maxDistance = 2;
    [SerializeField] float speed = 1f;

    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject requiredObject;

    public bool hasBeenUsed { get; set; } = false;

    // ---------- TODO ----------
    public string info { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }



    // When user interact, start the movement
    public void Interact()
    {
        if (playerInventory.AlreadyInInventory(requiredObject))
        {
            hasBeenUsed = true; // Avoid multiple interactions

            StartCoroutine(Move());
        }
        else
        {
            Debug.Log("You need a chip to hack the robots.");
        }
    }



    IEnumerator Move()
    {
        float distance = Vector3.Distance(startPosition, transform.position);

        Vector3 directionVector = Vector3.right; // This value wil be updated but is required to avoid compiling errors

        switch (direction)
        {
            case Direction.x:
                directionVector = Vector3.right;
                break;

            case Direction.minus_x:
                directionVector = Vector3.left;
                break;
        }

        Vector3 targetPosition = startPosition + directionVector * maxDistance;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetPosition;

        yield return null;
    }
}
