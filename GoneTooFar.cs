using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoneTooFar : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
