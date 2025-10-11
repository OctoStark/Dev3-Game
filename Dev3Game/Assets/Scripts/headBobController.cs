using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class HeadBobController : MonoBehaviour
{
  
    [SerializeField] private bool enableHeadBob = true;
    [SerializeField, Range(0f, 0.1f)] private float bobAmount = 0.05f;
    [SerializeField, Range(0f, 30f)] private float bobSpeed = 14f;
    [SerializeField] private float movementThreshold = 3.0f;
    [SerializeField, Range(0f, 10f)] private float rotationAmount = 2f;
    [SerializeField, Range(0f, 30f)] private float rotationSpeed = 14f;

   
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform bodyTransform;


    private Vector3 _startPos;
    private CharacterController _characterController;
    private float _bobTimer;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
           
            enabled = false;
            return;
        }

        _startPos = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (!enableHeadBob) return;

        bool isMoving = _characterController.velocity.magnitude > movementThreshold && _characterController.isGrounded;

        if (isMoving)
        {
            ApplyHeadBob();
        }
        else
        {
            ResetHeadBob();
        }
    }

    private void ApplyHeadBob()
    {
        float rotationOffset = Mathf.Sin(_bobTimer * 0.5f) * rotationAmount;
        Quaternion targetRotation = Quaternion.Euler(0f, rotationOffset, 0f);
        bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, targetRotation, Time.deltaTime * rotationSpeed);


        _bobTimer += Time.deltaTime * bobSpeed;

        float bobOffsetY = Mathf.Sin(_bobTimer) * bobAmount;
        float bobOffsetX = Mathf.Cos(_bobTimer * 0.5f) * bobAmount * 2;

        Vector3 bobPosition = _startPos + new Vector3(bobOffsetX, bobOffsetY, 0f);
        cameraTransform.localPosition = bobPosition;
    }

    private void ResetHeadBob()
    {
        _bobTimer = 0f;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _startPos, Time.deltaTime * bobSpeed);
        bodyTransform.localRotation = Quaternion.Lerp(bodyTransform.localRotation, Quaternion.identity, Time.deltaTime * rotationSpeed);

    }
}



