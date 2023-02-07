using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActionSnapTurn : MonoBehaviour
{
    [Header("Actions")]
    
    [SerializeField]
    [Tooltip("Used to read Snap Turn data from the left hand controller. Must be a Value Vector2 Control.")]
    private InputActionProperty leftHandSnapTurnAction;

    [SerializeField]
    [Tooltip("Used to read Snap Turn data from the right hand controller. Must be a Value Vector2 Control.")]
    private InputActionProperty rightHandSnapTurnAction;

    [Header("Turn")]
    [SerializeField]
    private float turnAmount = 45f;
    
    [SerializeField] private Transform head;
    
    private Rigidbody xRRigRigidbody;

    [Header("Physic Hands")] 
    [SerializeField]
    private List<HandPhysicsLocomotion> physicsHands;

    private void Awake()
    {
        xRRigRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        leftHandSnapTurnAction.action.performed += InputTurn;
        rightHandSnapTurnAction.action.performed += InputTurn;
    }

    private void OnDisable()
    {
        leftHandSnapTurnAction.action.performed -= InputTurn;
        rightHandSnapTurnAction.action.performed -= InputTurn;
    }

    private void InputTurn(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        
        if (input == Vector2.zero)
            return;

        input = input.normalized;
        var direction = input.x > 0 ? 1 : -1;
        
        Turn(direction * turnAmount);
    }
    
    private void Turn(float degrees)
    {
        var position = head.position;
        
        var q = Quaternion.AngleAxis(degrees, transform.up);
        xRRigRigidbody.MovePosition(q * (xRRigRigidbody.transform.position - position) + position);
        xRRigRigidbody.MoveRotation(q * xRRigRigidbody.transform.rotation);
        
        foreach (var hand in physicsHands)
           hand.SnapHandsToTarget();
    }
}