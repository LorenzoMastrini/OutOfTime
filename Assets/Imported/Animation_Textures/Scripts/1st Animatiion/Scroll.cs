using UnityEngine;

 public class Scroll : MonoBehaviour
 {
    public float ScrollX =  0.5f;
    public float ScrollY = 0.5f;
    


    void Update()
    {
        float OffsetY = Time.time * ScrollY;
        float OffsetX = Time.time * ScrollX;
        GetComponent<Renderer> (). material.mainTextureOffset = new Vector2 (OffsetX,OffsetY);
    }
 }