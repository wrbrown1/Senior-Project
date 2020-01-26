using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jogSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSmoothing = 0.2f; //time in seconds it takes for the player to rotate toward the input vector
    public float movementSpeedSmoothing = 0.2f; //time in seconds it takes for the player to achieve movement equal to the input vector

    Transform cameraTransform;

    Vector2 inputVector;
    Vector2 inputDirection;

    bool isRunning;

    float movementSpeed;
    float targetMovementSpeed;
    float currentRotationVelocity;
    float currentSpeedVelocity;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        GetInput();
        UpdateTranslation();
        UpdateRotation();
    }
    private void GetInput()
    {
        inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputDirection = inputVector.normalized;
    }

    private void UpdateTranslation()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning)
        {
            targetMovementSpeed = runSpeed * inputVector.magnitude;
        }
        else
        {
            targetMovementSpeed = jogSpeed * inputVector.magnitude;
        }
        //"SmoothDamp/SmoothDampAngle" method arguments: (starting point, point to move to, cached float, time in seconds that the smoothing will take)
        movementSpeed = Mathf.SmoothDamp(movementSpeed, targetMovementSpeed, ref currentSpeedVelocity, movementSpeedSmoothing);
        transform.Translate(Time.deltaTime * transform.forward * movementSpeed, Space.World);
    }

    private void UpdateRotation()
    {
        if (inputVector != Vector2.zero)
        {
            float targetRotation = Mathf.Rad2Deg * Mathf.Atan2(inputDirection.x, inputDirection.y) + cameraTransform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref currentRotationVelocity, rotationSmoothing);
        }
    }

    //public float movementSpeed = 10f;
    //public float rotationSpeed = 10f;
    //CameraController cameraController;
    //public Transform camera;

    //private void Start()
    //{
    //    cameraController = gameObject.GetComponentInChildren<CameraController>();
    //}

    //void Update()
    //{
    //    Movement(GetInputVector(), cameraController.mouseHorizontal, cameraController.mouseVertical);
    //}

    //private Vector3 GetInputVector()
    //{
    //    float xAxis = Input.GetAxis("Horizontal"); //"a" and "d" keys
    //    float yAxis = 0f; //Possible use for jumping. Jumping not yet implimented.
    //    float zAxis = Input.GetAxis("Vertical"); //"w" and "s" keys
    //    return new Vector3(xAxis, yAxis, zAxis);
    //}

    //void Movement(Vector3 inputVector, float mouseXLocation, float mouseYLocation)
    //{
    //    Translation(inputVector);
    //    Rotation(inputVector, mouseXLocation, mouseYLocation);
    //}

    //private void Translation(Vector3 inputVector)
    //{
    //    Vector3 playerMovementVector = inputVector * movementSpeed * Time.deltaTime;
    //    transform.Translate(playerMovementVector, Space.Self);
    //}

    //private void Rotation(Vector3 inputVector, float mouseXLocation, float mouseYLocation)
    //{
    //    if(inputVector.x > 0){

    //    }else if(inputVector.x < 0)
    //    {
    //        Debug.Log("Moving to the left");
    //    }
    //}
}
