using UnityEngine;



public class ChangeTimelineWarning : MonoBehaviour
{
    [SerializeField] int warningTimelineIndex;



    private void OnTriggerEnter(Collider other)
    {
        FirstPersonMovement player = other.GetComponent<FirstPersonMovement>();
        
        if (player != null)
        {
            player.SetWarning(warningTimelineIndex - 1, true);
        }
    }



    private void OnTriggerExit(Collider other)
    {
        FirstPersonMovement player = other.GetComponent<FirstPersonMovement>();

        if (player != null)
        {
            player.ResetWarning(warningTimelineIndex - 1);
        }
    }
}