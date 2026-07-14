using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private int duration = 5;
    [SerializeField] private ElevatorDoor doorLeft;
    [SerializeField] private ElevatorDoor doorRight;

    [SerializeField] AudioClip elevatorDing;

    private void OnTriggerEnter(Collider other)
    {
        SoundManager.instance.PlaySound(elevatorDing, transform, 1, 1);

        GameManager.ExecuteAfterDelay(duration, () =>
        {
            OpenDoors();
        });
    }



    private void OpenDoors()
    {
        doorLeft.Move();
        doorRight.Move();
    }
}