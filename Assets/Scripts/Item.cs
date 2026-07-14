using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void Use()
    {
        Debug.Log("Uso item");
    }
}