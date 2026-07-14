using System.Collections;
using UnityEngine;

public class BackAndForthMovement : PlayerOnMovingPlatform, IInteractable
{
    private const float WAIT_UNTIL_FIRST_MOVEMENT = 3f;
    private Vector3 startPosition;
    private int directionSign = 1;
    private bool isStopped = true;
    private float waitTime = 2f;
    private float timer = 0f; // Avoid being stuck inthe waiting phase when activating and deactivating the boat (changing timeline)
    [SerializeField] Direction direction = Direction.x;
    [SerializeField]  int maxDistance = 2;
    [SerializeField] float speed = 1f;
    [SerializeField] Inventory playerInventory;
    [SerializeField] GameObject requiredObject;

    public bool hasBeenUsed { get; set; } = false;

    public string info { get; set; } = "This boat is completely out of fuel in these times of crisis.";


    // When user interact, start the movement
    public void Interact()
    {
        if (playerInventory.AlreadyInInventory(requiredObject))
        {
            info = "The movement of this boat is so strange...";
            hasBeenUsed = true;
            playerInventory.UseItem();
            playerInventory.RemoveEquippedItem();

            AudioSource sound = GetComponent<AudioSource>();
            sound.Play();

            // Move after 3 seconds to let the user jump on the ferry
            GameManager.ExecuteAfterDelay(WAIT_UNTIL_FIRST_MOVEMENT, () =>
                isStopped = false
            );
        }
        else
        {
            Debug.Log("You need a battery to power the ferry.");
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        // 1 if moving on positive direction of the axis, -1 otherwise
        directionSign =
            direction == Direction.x || direction == Direction.z ? 1 : -1;
    }



    // Update is called once per frame
    void Update()
    {
        if (timer < 0)
        {
            directionSign *= -1;
            startPosition = transform.position;
            isStopped = false;
        }

        if (isStopped)
            return;

        Vector3 previousPosition = transform.position;

        if (direction == Direction.x || direction == Direction.minus_x) {
            MoveOnXAxis();
        }

        if (direction == Direction.z || direction == Direction.minus_z)
        {
            MoveOnZAxis();
        }

        // Movement traveled by the object in this frame
        Vector3 deltaMovement = transform.position - previousPosition;

        // Move the player with the object
        _playerOnPlatform?.ForceMove(deltaMovement);


        // Calculate traveled distance
        float distance = Vector3.Distance(startPosition, transform.position);

        // Wait for 2 seconds and switch direction
        if (distance >= maxDistance)
        {
            StartCoroutine(ChangeDirection());
        }
    }



    // Movement back and forth on the X axis
    void MoveOnXAxis()
    {
        transform.position += new Vector3(speed * directionSign, 0, 0) * Time.deltaTime;
    }

    

    // Movement back and forth on the Z axis
    void MoveOnZAxis()
    {
        transform.position += new Vector3(0, 0, speed * directionSign) * Time.deltaTime;
    }



    // Wait for 2 seconds, change direction and reset start position
    IEnumerator ChangeDirection()
    {
        isStopped = true;

        // Wait for 2 seconds
        yield return new WaitForSeconds(waitTime);

        directionSign *= -1; // Change direction
        startPosition = transform.position; // Reset

        isStopped = false;
    }
}