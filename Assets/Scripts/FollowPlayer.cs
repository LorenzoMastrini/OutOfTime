using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Riferimento al giocatore")]
    public CharacterController playerController;

    [Header("Velocità di inseguimento")]
    public float followSpeed = 1f;

    private void LateUpdate()
    {
        // Do nothing if game is paused
        if (PauseManager.instance.paused != PauseState.None)
            return;

        if (playerController == null)
            return;

        Vector3 targetPosition = playerController.transform.position;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );


        if(Vector3.Distance(targetPosition, transform.position) < 1)
        {
            GameManager.instance.DecreaseTime(1);
        }
    }
}