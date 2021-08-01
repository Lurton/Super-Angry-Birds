using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public SlingShot slingshot;

    public Transform rightPoint;
    public Transform leftPoint;
    public Transform topPoint;

    public float waitTime = 3f;
    public float headBackTime = -1;
    private Vector3 waitPosition;
    public float headBackDuration = 3f;

    public float dragScale = 0.075f;

    private bool followBird = false;
    private Vector3 followVelocity = Vector3.zero;
    public float followSmoothTime = 0.1f;

    public void Awake()
    {
        followBird = false;
        StartWait();
    }

    public void StartWait()
    {
        headBackTime = Time.time + waitTime;
        waitPosition = transform.position;
    }

    public void Update()
    {
        if (!slingshot.didFire)
        {
            if (slingshot.isAiming)
            {
                followBird = true;
                followVelocity = Vector3.zero;
            }
            else
            {
                followBird = false;
            }
        }

        if (followBird)
        {
            FollowBird();
            StartWait();
        }
        else if(Input.touchCount > 0)
        {
            DragCamera();
            StartWait();
        }

        if(!slingshot.didFire && headBackTime < Time.time)
        {
            BackToLeft();
        }
    }

    private void FollowBird()
    {
        if(slingshot.toFireBird == null)
        {
            followBird = false;
            return;
        }

        Vector3 targetPoint = slingshot.toFireBird.position;
        targetPoint.x = transform.position.x;

        transform.position = Vector3.SmoothDamp(transform.position, targetPoint, ref followVelocity, followSmoothTime);
        ClampPosition();
    }

    private void DragCamera()
    {
        transform.position -= new Vector3(0, 0, Input.GetTouch(0).deltaPosition.x * dragScale);
        ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 clamped = transform.position;
        clamped.z = Mathf.Clamp(clamped.z, leftPoint.position.z, rightPoint.position.z);
        clamped.y = Mathf.Clamp(clamped.y, leftPoint.position.y, topPoint.position.y);
        transform.position = clamped;
    }

    private void BackToLeft()
    {
        float progress = (Time.time - headBackTime) / headBackDuration;
        Vector3 newPosition = transform.position;
        newPosition.z = Mathf.SmoothStep(waitPosition.z, leftPoint.position.z, progress);
        newPosition.y = Mathf.SmoothStep(waitPosition.y, leftPoint.position.y, progress);
        transform.position = newPosition;
    }
}
