using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    private Vector3 startPosition;
    private float maxDistance = 0.7537f;
    private float speed = 1.5f;

    [SerializeField] Direction direction;
    

    void Start()
    {
        startPosition = transform.position;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private IEnumerator MoveCoroutine()
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



    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }
}
