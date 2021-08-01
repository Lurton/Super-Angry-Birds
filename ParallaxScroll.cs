using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    public Material[] materials = new Material[0];
    public float[] speeds = new float[0];

    private Vector3 lastPosition = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPosition = Camera.main.transform.position;
        float move = newPosition.z - lastPosition.z;

        for(int i = 0; i < materials.Length; i++)
        {
            Vector2 offset = materials[i].mainTextureOffset;
            offset.x -= move * speeds[i];
            materials[i].mainTextureOffset = offset;
        }

        lastPosition = newPosition;
    }
}
