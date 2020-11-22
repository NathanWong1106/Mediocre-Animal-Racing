using System;
using System.Collections;
using System.Collections.Generic;
using Racing.Vehicles.Components;
using Racing.Util;
using UnityEngine;

namespace Racing.Vehicles.Control
{
    /// <summary>
    /// Updates fields in VehicleUpdater based on Player Input
    /// </summary>
    [RequireComponent(typeof(Vehicle), typeof(VehicleUpdater))]
    public class VehicleController : MonoBehaviour
    {
        private Vehicle vehicle;
        private VehicleUpdater vehicleUpdater;
        public bool IsGrounded = true;

        private void Start()
        {
            vehicle = GetComponent<Vehicle>();
            vehicleUpdater = GetComponent<VehicleUpdater>();
        }

        private void Update()
        {
            GroundCheck();
            Jump();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        /// <summary>
        /// Checks if the vehicle is grounded
        /// </summary>
        private void GroundCheck()
        {
            IsGrounded = vehicle.Axles.TrueForAll(a => a.Right.isGrounded && a.Left.isGrounded);
        }

        /// <summary>
        /// Checks for valid jump input and applies an upward force on the vehicle rigidbody
        /// </summary>
        private void Jump()
        {
            if (InputHelper.Jump && IsGrounded)
            {
                vehicle.Rigidbody.AddForce(Vector3.up * VehicleSettings.JumpForce, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Calls movement handlers to update vehicle control
        /// <see cref="https://docs.unity3d.com/Manual/WheelColliderTutorial.html"/>
        /// </summary>
        private void UpdateMovement()
        {
            vehicle.Rigidbody.AddForce(Vector3.down * 300);

            ApplyGroundedMovement();

            if (!IsGrounded)
                ApplyAirMovement();
        }

        /// <summary>
        /// Sets wheel steering, motor torque, and brake torque based on user input.
        /// </summary>
        private void ApplyGroundedMovement()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(vehicle.Rigidbody.velocity);

            vehicleUpdater.TargetLerpAngle = VehicleSettings.MaxSteerAngle * InputHelper.Horizontal;
            vehicleUpdater.MotorTorque = VehicleSettings.MaxMotorTorque * InputHelper.Vertical;

            // 0.01f accounts for small amount of velocity still carried by the rigidbody (at this point motorTorque is able to simulate braking)
            if (localVelocity.z > 0.01f && InputHelper.Vertical < 0)
            {
                vehicleUpdater.BrakeTorque = VehicleSettings.MaxBrakeTorque;
            }
            else if (localVelocity.z < -0.01f && InputHelper.Vertical > 0)
            {
                vehicleUpdater.BrakeTorque = VehicleSettings.MaxBrakeTorque;
            }
            else
            {
                vehicleUpdater.BrakeTorque = 0f;
            }
        }

        /// <summary>
        /// Applies torque in the y-axis based on user input
        /// </summary>
        private void ApplyAirMovement()
        {
            vehicle.Rigidbody.AddTorque(Vector3.up * VehicleSettings.AirTurnTorque * InputHelper.Horizontal);
        }
    }
}
