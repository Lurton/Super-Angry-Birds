using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public RigidbodyDamper rigidbodyDamper;
    
    public GameObject[] levelBirds = new GameObject[0];
    private GameObject[] currentBirds;
    private int nextIndex = 0;
    public Transform waitPoint;

    public Transform toFireBird;
    public bool didFire = false;
    public bool isAiming = false;

    public Transform pouch;
    public Transform focalPoint;
    public Transform pouchBirdPoint;

    public float maxRange = 3;

    public float maxFireStrength = 25;
    public float minFireStrength = 5;

    public void Awake()
    {
        currentBirds = new GameObject[levelBirds.Length];
        for(int i = 0; i < levelBirds.Length; i++)
        {
            GameObject nextBird = Instantiate(levelBirds[i]) as GameObject;
            nextBird.GetComponent<Rigidbody>().isKinematic = true;
            currentBirds[i] = nextBird;
        }

        ReadyNextBird();
        SetWaitingPositions();
    }

    public void ReadyNextBird()
    {
        if(currentBirds.Length <= nextIndex)
        {
            LevelTracker.OutOfBirds();
            return;
        }

        if(currentBirds[nextIndex] == null)
        {
            nextIndex++;
            ReadyNextBird();
            return;
        }

        toFireBird = currentBirds[nextIndex].transform;
        nextIndex++;

        toFireBird.parent = pouchBirdPoint;
        toFireBird.localPosition = Vector3.zero;
        toFireBird.localRotation = Quaternion.identity;

        didFire = false;
        isAiming = false;
    }

    public void SetWaitingPositions()
    {
        for(int i = nextIndex; i < currentBirds.Length; i++)
        {
            if (currentBirds[i] == null) continue;
            Vector3 offset = Vector3.forward * (i - nextIndex) * 2;
            currentBirds[i].transform.position = waitPoint.position - offset;
        }
    }

    public void Update()
    {
        if (didFire)
        {
            if (rigidbodyDamper.allSleeping)
            {
                ReadyNextBird();
                SetWaitingPositions();
            }
            return;
        }
        else if (isAiming)
        {
            DoAiming();
        }
        else
        {
            if (Input.touchCount <= 0) return;
            Vector3 touchPoint = GetTouchPoint();
            isAiming = Vector3.Distance(touchPoint, focalPoint.position) < maxRange / 2;
        }
    }

    private void DoAiming()
    {
        if(Input.touchCount <= 0)
        {
            FireBird();
            return;
        }

        Vector3 touchPoint = GetTouchPoint();

        pouch.position = touchPoint;
        pouch.LookAt(focalPoint);

        float distance = Vector3.Distance(focalPoint.position, pouch.position);
        if(distance > maxRange)
        {
            pouch.position = focalPoint.position - (pouch.forward * maxRange);
        }
    }

    private Vector3 GetTouchPoint()
    {
        Ray touchRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        Vector3 touchPoint = touchRay.origin;
        touchPoint.x = 0;
        return touchPoint;
    }

    private void FireBird()
    {
        didFire = true;

        Vector3 direction = (focalPoint.position - pouch.position).normalized;
        float distance = Vector3.Distance(focalPoint.position, pouch.position);
        float power = distance <= 0 ? 0 : distance / maxRange;
        power *= maxFireStrength;
        power = Mathf.Clamp(power, minFireStrength, maxFireStrength);

        toFireBird.parent = null;
        toFireBird.GetComponent<Rigidbody>().isKinematic = false;
        toFireBird.GetComponent<Rigidbody>().AddForce(direction * power, ForceMode.Impulse);

        pouch.position = focalPoint.position;
       rigidbodyDamper.ReadyDamp();
    }
}