using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public GameObject currentHitObject;
    public float maxDistance;
    public float rayLineDamping;
    public float sphereRadius;
    public float adjustmentSmoothing;
    public float currentHitDistance;
    public float cameraZoom;
    public float maxHieghtAdjustment;
    public float minHieghtAdjustment;
    public float whenHeightSmooths;
    public float hieghtSmoothing;
    public LayerMask sphereLayerMask;
    public LayerMask lineLayerMask;


    private Vector3 origin;
    private Vector3 direction;
    private float currentFloat;
    private float currentHieghtChange;
    private CameraController cameraController;

    private void Start()
    {
        cameraController = gameObject.GetComponent<CameraController>();
    }

    private void LateUpdate()
    {
        SphereRayCast();
        LineRayCast();
        CameraCollisionAdjustment();
        maxDistance = Mathf.Clamp(maxDistance, cameraController.cameraMinDistance, cameraController.cameraMaxDistance);
    }

    private void LineRayCast()
    {
        RaycastHit hit;
        origin = cameraController.focus.transform.position;
        direction = -transform.forward;
        maxDistance = Vector3.Distance(origin, transform.position) * rayLineDamping;

        if (Physics.Raycast(origin, direction, out hit, maxDistance, lineLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
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

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, sphereLayerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
    }

    private void CameraCollisionAdjustment()
    {
        AdjustCameraHieght();
        Mathf.Clamp(maxDistance, cameraController.cameraMinDistance, cameraController.cameraMaxDistance);
        if (currentHitObject != null)
        {
            cameraController.cameraDistanceFromPlayer = Mathf.SmoothDamp(cameraController.cameraDistanceFromPlayer, currentHitDistance, ref currentFloat, adjustmentSmoothing);
        }
        else
        {
            cameraController.cameraDistanceFromPlayer = Mathf.SmoothDamp(cameraController.cameraMaxDistance, currentHitDistance, ref currentFloat, adjustmentSmoothing);
        }
    }

    private void AdjustCameraHieght()
    {
        if (cameraController.cameraDistanceFromPlayer < cameraController.cameraMaxDistance * whenHeightSmooths)
        {
            cameraController.cameraHieght = Mathf.SmoothDamp(cameraController.cameraHieght, maxHieghtAdjustment, ref currentHieghtChange, hieghtSmoothing);
        }
        else
        {
            cameraController.cameraHieght = Mathf.SmoothDamp(cameraController.cameraHieght, minHieghtAdjustment, ref currentHieghtChange, hieghtSmoothing);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, (origin + direction * currentHitDistance) + new Vector3(0f, -sphereRadius, 0f), Color.red);
        Debug.DrawLine(origin, (origin + direction * currentHitDistance) + new Vector3(0f, sphereRadius, 0f), Color.red);
        Debug.DrawLine(origin, (origin + direction * currentHitDistance) + new Vector3(-sphereRadius, 0f, 0f), Color.red);
        Debug.DrawLine(origin, (origin + direction * currentHitDistance) + new Vector3(sphereRadius, 0f, 0f), Color.red);
        Debug.DrawLine(origin, origin + direction * currentHitDistance, Color.red);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
    //public float minDistance = 1f;
    //public float maxDistance = 4f;
    //public float smooth = 10f;
    //Vector3 dollyDirection;
    //public Vector3 dollyDirectionAdjusted;
    //public float distance;

    //private void Awake()
    //{
    //    dollyDirection = transform.localPosition.normalized;
    //    distance = transform.localPosition.magnitude;
    //}

    //void Update()
    //{
    //    Vector3 desiredCamerPosition = transform.parent.TransformPoint(dollyDirection * maxDistance);
    //    RaycastHit hit;
    //    if(Physics.Linecast(transform.parent.position, desiredCamerPosition, out hit))
    //    {
    //        distance = Mathf.Clamp((hit.distance * .8f), minDistance, maxDistance);
    //    }
    //    else
    //    {
    //        distance = maxDistance;
    //    }
    //    transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDirection * distance, Time.deltaTime * smooth);
    //}
}
