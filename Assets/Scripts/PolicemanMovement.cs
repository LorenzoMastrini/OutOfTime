using System.Collections;
using UnityEngine;

public class PolicemanMovement : MonoBehaviour
{
    private Vector3 startPosition;

    [SerializeField] Direction direction = Direction.x;
    [SerializeField] int maxDistance = 2;
    [SerializeField] float speed = 1f;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }


    public void StartMovement()
    {
        animator.SetBool("isMoving", true);
        StartCoroutine(Move());
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
        animator.SetBool("isMoving", false);

        yield return null;
    }
}
