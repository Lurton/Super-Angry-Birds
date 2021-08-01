using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float health = 50;
    protected bool didSpecial = false;

    // Update is called once per frame
    public void Update()
    {
        if (didSpecial)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;
        if (GetComponent<Rigidbody>() == null || GetComponent<Rigidbody>().isKinematic)
            return;

        DoSpecial();
    }

    protected virtual void DoSpecial()
    {
        didSpecial = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        health -= collision.relativeVelocity.magnitude;
        if (health < 0)
            Destroy(gameObject);
    }
}
