using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{

    public GameObject currentHitObject;
    public float maxDistance;
    public float sphereRadius;
    public LayerMask layerMask;


    private Vector3 origin;
    private float currentFloat;
    private float currentHitDistance;
    private CameraController cameraController;
    private Vector3 direction;

    private void Start()
    {
        cameraController = gameObject.GetComponent<CameraController>();
    }

    private void Update()
    {
        SphereRayCast();
        LineRayCast();
    }

    private void LineRayCast()
    {
        RaycastHit hit;
        origin = cameraController.focus.transform.position;
        direction = -transform.forward;
        maxDistance = Vector3.Distance(origin, transform.position);

        if (Physics.Raycast(origin, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            Debug.Log(hit.point);
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
    }

    private void SphereRayCast()
    {
        RaycastHit hit;
        origin = cameraController.focus.transform.position;
        direction = -transform.forward;
        maxDistance = Vector3.Distance(origin, transform.position);

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            Debug.Log(hit.point);
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
    }

    private void CameraCollisionAdjustment(float distance)
    {
        if (currentHitObject != null)
        {

        }
    }

    private void SmoothBetweenTwoFloats(float current, float target, float smoothTime)
    {
        current = Mathf.SmoothDamp(current, target, ref currentFloat, smoothTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}
