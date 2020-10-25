using System;
using System.Collections;
using System.Collections.Generic;
using Racing.Util;
using Racing.Vehicle.Common;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

public class VehicleController : MonoBehaviour
{
    Vehicle vehicle;

    private void Awake()
    {
        vehicle = GetComponent<Vehicle>();
    }

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        if (InputHelper.Jump && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), VehicleSettings.rideHeight))
        {
            vehicle.rigidbody.AddForce(Vector3.up * VehicleSettings.jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    /// <summary>
    /// Sets wheel steering, motor torque, and brake torque based on user input
    /// <see cref="https://docs.unity3d.com/Manual/WheelColliderTutorial.html"/>
    /// </summary>
    private void UpdateMovement()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(vehicle.rigidbody.velocity);

        foreach (Axle axle in vehicle.axles)
        {
            if (axle.steering)
            {
                axle.left.steerAngle = Mathf.Lerp(axle.left.steerAngle, VehicleSettings.maxSteerAngle * InputHelper.Horizontal, VehicleSettings.turnLerp);
                axle.right.steerAngle = Mathf.Lerp(axle.right.steerAngle, VehicleSettings.maxSteerAngle * InputHelper.Horizontal, VehicleSettings.turnLerp);
            }
            if (axle.motor)
            {
                axle.left.motorTorque = VehicleSettings.maxMotorTorque * InputHelper.Vertical;
                axle.right.motorTorque = VehicleSettings.maxMotorTorque * InputHelper.Vertical;
            }
            if (axle.braking)
            {
                if(localVelocity.z > 0 && InputHelper.Vertical < 0)
                {
                    axle.left.brakeTorque = VehicleSettings.maxBrakeTorque;
                    axle.right.brakeTorque = VehicleSettings.maxBrakeTorque;
                }
                else if (localVelocity.z < 0 && InputHelper.Vertical > 0)
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
            ApplyLocalPositionToVisuals(axle.left);
            ApplyLocalPositionToVisuals(axle.right);   
        }

        vehicle.rigidbody.AddForce(Vector3.down * 300);
    }

    /// <summary>
    /// Applies Wheel Collider rotation and position to their visual elements
    /// </summary>
    /// <param name="collider"></param>
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
