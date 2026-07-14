using UnityEngine;

public class WheelsAnimation : MonoBehaviour
{
    CarMovement parentMovementScript;

    private void Start()
    {
        parentMovementScript = transform.GetComponentInParent<CarMovement>();
    }
    // Update is called once per frame
    void Update()
    {
        if(parentMovementScript.enabled)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * 1000);
        }
    }
}
