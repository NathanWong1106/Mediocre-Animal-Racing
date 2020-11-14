using System;
using System.Collections;
using System.Collections.Generic;
using Racing.Util;
using Racing.Vehicle.Common;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

/// <summary>
/// Contains methods pertaining to player control over vehicles
/// </summary>

[RequireComponent(typeof(Vehicle))]
public class VehicleController : MonoBehaviour
{
    Vehicle vehicle;
    public bool isGrounded = true;
    public InputType inputType;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void Update()
    {
        if(inputType == InputType.Player)
        {
            GroundCheck();
            Jump();
        }
        ReevaluteVisuals();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void ReevaluteVisuals()
    {
        foreach(Axle axle in vehicle.axles)
        {
            ApplyLocalPositionToVisuals(axle.right);
            ApplyLocalPositionToVisuals(axle.left);
        }
    }

    /// <summary>
    /// Checks if the vehicle is grounded
    /// </summary>
    private void GroundCheck()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), VehicleSettings.rideHeight))
            isGrounded = true;
        else
            isGrounded = false;
    }

    /// <summary>
    /// Checks for valid jump input and applies an upward force on the vehicle rigidbody
    /// </summary>
    private void Jump()
    {
        if (InputHelper.Jump && isGrounded)
        {
            vehicle.rigidbody.AddForce(Vector3.up * VehicleSettings.jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Calls movement handlers to update vehicle control
    /// <see cref="https://docs.unity3d.com/Manual/WheelColliderTutorial.html"/>
    /// </summary>
    private void UpdateMovement()
    {
        vehicle.rigidbody.AddForce(Vector3.down * 300);

        if (inputType == InputType.Player)
        {
            ApplyGroundedMovement();

            if (!isGrounded)
                ApplyAirMovement();
        }
/*        else
        {
            //Not implemented yet - AI movement
            throw new NotImplementedException();
        }*/
    }

    /// <summary>
    /// Sets wheel steering, motor torque, and brake torque based on user input.
    /// </summary>
    private void ApplyGroundedMovement()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(vehicle.rigidbody.velocity);

        foreach (Axle axle in vehicle.axles)
        {
            if (axle.steering)
            {
                axle.targetLerpAngle = VehicleSettings.maxSteerAngle * InputHelper.Horizontal;
            }
            if (axle.motor)
            {
                axle.left.motorTorque = VehicleSettings.maxMotorTorque * InputHelper.Vertical;
                axle.right.motorTorque = VehicleSettings.maxMotorTorque * InputHelper.Vertical;
            }
            if (axle.braking)
            {
                // 0.01f accounts for small amount of velocity still carried by the rigidbody (at this point motorTorque is able to simulate braking)
                if (localVelocity.z > 0.01f && InputHelper.Vertical < 0)
                {
                    axle.left.brakeTorque = VehicleSettings.maxBrakeTorque;
                    axle.right.brakeTorque = VehicleSettings.maxBrakeTorque;
                }
                else if (localVelocity.z < -0.01f && InputHelper.Vertical > 0)
                {
                    axle.left.brakeTorque = VehicleSettings.maxBrakeTorque;
                    axle.right.brakeTorque = VehicleSettings.maxBrakeTorque;
                }
                else
                {
                    axle.left.brakeTorque = 0f;
                    axle.right.brakeTorque = 0f;
                }
            }
        }
    }

    /// <summary>
    /// Applies torque in the y-axis based on user input
    /// </summary>
    private void ApplyAirMovement()
    {
        vehicle.rigidbody.AddTorque(Vector3.up * VehicleSettings.airTurnTorque * InputHelper.Horizontal);
    }

    /// <summary>
    /// Applies Wheel Collider rotation and position to their visual elements
    /// </summary>
    /// <param name="collider">Wheel Collider that the visual is attached to</param>
    private void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;

        Vector3 euler = visualWheel.transform.rotation.eulerAngles;

        //visual wheel rotation fix - sets y rotation to 90
        visualWheel.transform.rotation = Quaternion.Euler(euler.x, euler.y, 90);
    }
}
