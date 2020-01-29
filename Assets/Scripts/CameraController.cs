using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focus;

    public float horizontalCameraSensitivity = 10f;
    public float verticalCameraSensitivity = 10f;
    public float cameraDistanceFromPlayer = 2f;
    public float cameraMinDistance = 1f;
    public float cameraMaxDistance = 6.5f;
    public float cameraHieght = 2f;
    public float cameraSmoothSpeed;
    public Vector2 pitchBoundary = new Vector2(-90f, 90f);

    Vector3 currentRotation;
    Vector3 targetRotation;
    Vector3 currentRotationVelocity;

    CameraCollisionController collisionController;

    float pitch;
    float yaw;

    private void LateUpdate()
    {
        GetInput();
        UpdateRotation();
        UpdateTranslation();
        cameraDistanceFromPlayer = Mathf.Clamp(cameraDistanceFromPlayer, cameraMinDistance, cameraMaxDistance);
    }

    private void GetInput()
    {
        yaw += Input.GetAxis("Mouse X") * horizontalCameraSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * verticalCameraSensitivity;
        pitch = Mathf.Clamp(pitch, pitchBoundary.x, pitchBoundary.y);
    }
    private void UpdateRotation()
    {
        targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref currentRotationVelocity, cameraSmoothSpeed);
        transform.eulerAngles = currentRotation;
    }
    private void UpdateTranslation()
    {
        transform.position = focus.position - cameraDistanceFromPlayer * transform.forward + new Vector3(0f, cameraHieght, 0f);
    }
}
