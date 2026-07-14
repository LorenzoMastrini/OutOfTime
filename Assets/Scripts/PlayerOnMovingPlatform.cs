using UnityEngine;

public class PlayerOnMovingPlatform : MonoBehaviour
{
    private string playerTag = "Player";
    protected FirstPersonMovement _playerOnPlatform;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerOnPlatform = other.GetComponent<FirstPersonMovement>();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            _playerOnPlatform = null;
        }
    }
}
