using Unity.VisualScripting;
using UnityEngine;

public class UpAndDownMovement : PlayerOnMovingPlatform
{
    private Vector3 startPosition;
    private int directionSign = 1;
    private bool isWaiting = false; // Starts immediately
    private float waitTime = 2f;
    private float timer = 0;

    [SerializeField] Direction direction = Direction.y;
    [SerializeField]  float maxDistance = 2;
    [SerializeField] float speed = 1f;

    [SerializeField] AudioSource sound;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        // 1 if moving on positive direction of the axis, -1 otherwise
        directionSign =
            direction == Direction.x || direction == Direction.y || direction == Direction.z ? 1 : -1;

        sound = GetComponent<AudioSource>();
        sound.Play();
    }



    // Update is called once per frame
    void Update()
    {
        if (isWaiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                timer = 0f;
                directionSign *= -1; // Switch direction
                startPosition = transform.position;

                sound.Play();

                isWaiting = false;
            }

            return;
        }

        Vector3 previousPosition = transform.position;

        if (direction == Direction.x || direction == Direction.minus_x) {
            MoveOnXAxis();
        }

        if (direction == Direction.y || direction == Direction.minus_y)
        {
            MoveOnYAxis();
        }

        if (direction == Direction.z || direction == Direction.minus_z)
        {
            MoveOnZAxis();
        }

        // Movement traveled by the object in this frame
        Vector3 deltaMovement = transform.position - previousPosition;

        // Move the player with the object
        _playerOnPlatform?.ForceMove(deltaMovement);
        _playerOnPlatform?.SetVerticalVelocity(0);

        // Calculate traveled distance
        float distance = Vector3.Distance(startPosition, transform.position);

        // Wait for 2 seconds
        if (distance >= maxDistance)
        {
            timer = 0; // Reset timer
            isWaiting = true; // Stop the platform
        }
    }



    // Movement back and forth on the X axis
    void MoveOnXAxis()
    {
        transform.position += new Vector3(speed * directionSign, 0, 0) * Time.deltaTime;
    }



    // Movement up and down on the Y axis
    void MoveOnYAxis()
    {
        transform.position += new Vector3(0, speed * directionSign, 0) * Time.deltaTime;
    }



    // Movement back and forth on the Z axis
    void MoveOnZAxis()
    {
        transform.position += new Vector3(0, 0, speed * directionSign) * Time.deltaTime;
    }
}