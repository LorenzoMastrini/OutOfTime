using UnityEngine;



public class FutureCarMovement : PlayerOnMovingPlatform
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float distanceUntilDestroyed = 10f;
    [SerializeField] private Direction moveDirection = Direction.x;

    private Vector3 moveVector;
    private Vector3 startPosition;
    private Vector3 deltaMovement;



    private void Start()
    {
        startPosition = transform.position;

        // Set movement vector and rotation based on the specified direction
        switch (moveDirection)
        {
            case Direction.x:
                moveVector = Vector3.right;
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;

            case Direction.minus_x:
                moveVector = Vector3.left;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;

            case Direction.z:
                moveVector = Vector3.forward;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case Direction.minus_z:
                moveVector = Vector3.back;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            default:
                moveVector = Vector3.up;
                break;
        }
    }



    private void FixedUpdate() {
        deltaMovement = moveVector * speed * Time.fixedDeltaTime;

        transform.position += deltaMovement;

        if (Vector3.Distance(startPosition, transform.position) >= distanceUntilDestroyed)
        {
            Destroy(gameObject);
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (_playerOnPlatform != null)
        {
            _playerOnPlatform.ForceMove(deltaMovement);
        }
    }



    public void SetMoveDirection(Direction newDirection)
    {
        moveDirection = newDirection;
    }



    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}