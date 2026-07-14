using UnityEngine;

public enum Direction
{
    x,
    minus_x,
    y,
    minus_y,
    z,
    minus_z
}

public class CarMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float speed = 10f;
    [SerializeField] float distanceUntilDestroyed = 10f;
    [SerializeField] Direction moveDirection = Direction.x;

    private Vector3 _moveVector;
    private Vector3 startPosition;



    private void Start()
    {
        startPosition = transform.position;

        // Set movement vector and rotation based on the specified direction
        switch (moveDirection)
        {
            case Direction.x:
                _moveVector = Vector3.right;
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;

            case Direction.minus_x:
                _moveVector = Vector3.left;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;

            case Direction.z:
                _moveVector = Vector3.forward;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;

            case Direction.minus_z:
                _moveVector = Vector3.back;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            default:
                _moveVector = Vector3.up;
                break;
        }
    }



    // Update is called once per frame
    void Update()
    {
        transform.position += _moveVector * speed * Time.deltaTime;

        // Calculate total distance traveled
        float distance = Vector3.Distance(startPosition, transform.position);

        // Destroy object when reaches max distance
        if (distance >= distanceUntilDestroyed)
        {
            Destroy(gameObject);
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
