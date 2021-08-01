using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyDamper : MonoBehaviour
{
    public float dampWaitLength = 10f;
    public float dampAmount = 0.9f;

    private float dampTime = -1;
    private bool canDamp = false;
    private Rigidbody[] rigidbodies = new Rigidbody[0];

    public bool allSleeping = false;

    public void ReadyDamp()
    {
        rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
        dampTime = Time.time + dampWaitLength;
        canDamp = true;
        allSleeping = false;

        StartCoroutine(CheckSleepingRigidbodies());

    }

    public void FixedUpdate()
    {
        if (!canDamp || dampTime > Time.time) return;

        foreach(Rigidbody next in rigidbodies)
        {
            if(next != null && !next.isKinematic && !next.IsSleeping())
            {
                next.angularVelocity *= dampAmount;
                next.velocity *= dampAmount; 
            }
        }
    }

    private IEnumerator CheckSleepingRigidbodies()
    {
        bool sleepCheck = false;

        while (!sleepCheck)
        {
            sleepCheck = true;

            foreach(Rigidbody next in rigidbodies)
            {
                if(next != null && !next.isKinematic && !next.IsSleeping())
                {
                    sleepCheck = false;
                    yield return null;
                    break;
                }
            }
        }
        allSleeping = true;
        canDamp = false;
    }

    public void AddBodiesToCheck(Rigidbody[] toAdd)
    {
        Rigidbody[] temp = rigidbodies;
        rigidbodies = new Rigidbody[temp.Length + toAdd.Length];

        for(int i = 0; i < temp.Length; i++)
        {
            rigidbodies[i] = temp[i];
        }

        for(int i = 0; i < toAdd.Length; i++)
        {
            rigidbodies[i + temp.Length] = toAdd[i];
        }
    }
}
