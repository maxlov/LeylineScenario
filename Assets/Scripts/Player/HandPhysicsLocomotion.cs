using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.XR.Interaction.Toolkit;

public class HandPhysicsLocomotion : MonoBehaviour
{
    [Header("PID")]
    [SerializeField]
    private float frequency = 50f;
    [SerializeField] private float damping = 1f;
    [SerializeField] private float rotfrequency = 100f;
    [SerializeField] private float rotDamping = 0.9f;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private Transform target;
    
    [Space]
    [Header("Springs")]
    [SerializeField] private float climbForce = 1000f;
    [SerializeField] private float climbDrag = 500f;

    [SerializeField]
    [Tooltip("Select action for grabbing onto walls.")]
    private InputActionProperty selectAction;
    
    private Vector3 _previousPosition;
    private Rigidbody _rigidbody;
    private bool _isColliding;
    private bool _isGrabbingSurface;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = float.PositiveInfinity;
        SnapHandsToTarget();
    }

    public void SnapHandsToTarget()
    {
        var hand = transform;
        hand.position = target.position;
        hand.rotation = target.rotation;
        _previousPosition = hand.position;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    
    private void FixedUpdate()
    {
        PidMovement();
        PidRotation();

        if (_isColliding || _isGrabbingSurface) HookesLaw();
    }

    private void PidMovement()
    {
        float kp = (6f * frequency) *  (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRigidbody.velocity - _rigidbody.velocity) * kdg;
        _rigidbody.AddForce(force, ForceMode.Acceleration);
    }

    private void PidRotation()
    {
        float kp = (6f * rotfrequency) *  (6f * rotfrequency) * 0.25f;
        float kd = 4.5f * rotfrequency * rotDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);

        if (q.w < 0)
        {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }

        q.ToAngleAxis(out float angle, out Vector3 axis);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = axis * (ksg * angle) + -_rigidbody.angularVelocity * kdg;
        _rigidbody.AddTorque(torque, ForceMode.Acceleration);
    }

    private void HookesLaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * climbForce;
        float drag = GetDrag();

        playerRigidbody.AddForce(force, ForceMode.Acceleration);
        playerRigidbody.AddForce(-playerRigidbody.velocity * (drag * climbDrag), ForceMode.Acceleration);
    }

    private float GetDrag()
    {
        Vector3 handVelocity = (target.localPosition - _previousPosition) /  Time.fixedDeltaTime;
        float drag = 1 / handVelocity.magnitude + 0.01f;
        drag = Mathf.Clamp(drag, 0.03f, 1);
        _previousPosition = transform.position;
        return drag;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isColliding = true;
    }

    private void OnCollisionExit(Collision other)
    {
        _isColliding = false;
    }

    private void OnSelect(InputAction.CallbackContext context)
    {
        if (!_isColliding)
            return;
        
        _isGrabbingSurface = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnLetGo(InputAction.CallbackContext context)
    {
        _isGrabbingSurface = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
    }
    
    private void OnEnable()
    {
        selectAction.action.performed += OnSelect;
        selectAction.action.canceled += OnLetGo;
    }

    private void OnDisable()
    {
        selectAction.action.performed -= OnSelect;
        selectAction.action.canceled -= OnLetGo;
    }
}
