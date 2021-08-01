using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotBand : MonoBehaviour
{
    public Transform endPoint;
    public LineRenderer lineRenderer;

    public void Awake()
    {
        if (lineRenderer == null) return;
        if (endPoint == null) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    public void LateUpdate()
    {
        if (endPoint == null) return;
        if (lineRenderer == null) return;

        lineRenderer.SetPosition(1, endPoint.position);
    }
}
