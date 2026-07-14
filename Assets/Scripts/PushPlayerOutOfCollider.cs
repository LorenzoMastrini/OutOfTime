using UnityEngine;

public class PushPlayerOutOfCollider : MonoBehaviour
{
    [SerializeField] Vector3 pushDirection;
    [SerializeField] FirstPersonMovement player;

    void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<FirstPersonMovement>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        player.ForceMove(pushDirection);
    }

    private void OnTriggerStay(Collider other)
    {
        player.ForceMove(pushDirection);
    }
}