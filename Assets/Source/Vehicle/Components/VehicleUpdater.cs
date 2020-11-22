using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Vehicles.Components
{
    /// <summary>
    /// Updates vehicle axles with the provided fields
    /// </summary>
    [RequireComponent(typeof(Vehicle))]
    public class VehicleUpdater : MonoBehaviour
    {
        private Vehicle vehicle;
        public float TargetLerpAngle { get; set; } = 0;
        public float BrakeTorque { get; set; } = 0;
        public float MotorTorque { get; set; } = 0;

        private void Start()
        {
            vehicle = GetComponent<Vehicle>();
        }

        private void Update()
        {
            RecalculateMovementParameters();
            UpdateAxleLerp();
            ApplyLocalPositionToAxleVisuals();
        }

        /// <summary>
        /// Updates visuals for each axle on the vehicle
        /// </summary>
        private void ApplyLocalPositionToAxleVisuals()
        {
            foreach(var axle in vehicle.Axles)
            {
                Axle.UpdateVisuals(axle);
            }
        }

        /// <summary>
        /// Applies public fields to vehicle axle parameters
        /// </summary>
        private void RecalculateMovementParameters()
        {
            vehicle.Rigidbody.AddForce(Vector3.down * 300); //temp downforce?
            foreach (var axle in vehicle.Axles)
            {
                if (axle.Steering)
                {
                    axle.TargetLerpAngle = TargetLerpAngle;
                }

                if (axle.Motor)
                {
                    axle.Right.motorTorque = MotorTorque;
                    axle.Left.motorTorque = MotorTorque;
                }

                if (axle.Braking)
                {
                    axle.Right.brakeTorque = BrakeTorque;
                    axle.Left.brakeTorque = BrakeTorque;
                }
            }
        }

        /// <summary>
        /// Handles wheel lerp for each exle of the vehicle
        /// </summary>
        private void UpdateAxleLerp()
        {
            foreach (var axle in vehicle.Axles)
            {
                RecalculateLerp(axle);
                if (axle.IsLerping)
                {
                    axle.TimeSinceStartLerp += Time.deltaTime * VehicleSettings.TurnLerpMultiplier;
                    LerpToSteerAngle(axle);
                }
            }
        }

        /// <summary>
        /// Recalculates wheel lerp variables based on the current targetLerpAngle of an axis
        /// </summary>
        /// <param name="axle"></param>
        private void RecalculateLerp(Axle axle)
        {
            if (axle.PreviousTargetLerpAngle != axle.TargetLerpAngle) // User has changed directions
            {
                axle.IsLerping = true;
                axle.TimeSinceStartLerp = 0;
                axle.InitialLerpAngle = axle.Right.steerAngle;
                axle.PreviousTargetLerpAngle = axle.TargetLerpAngle;
            }
            else if (axle.Right.steerAngle == axle.TargetLerpAngle) // Maximum steer angle has been reached
            {
                axle.IsLerping = false;
            }
            else // Axle is in the middle of lerping
            {
                axle.IsLerping = true;
            }
        }

        /// <summary>
        /// Sets the axle steer angle to an angle relative to the lerp timeframe
        /// </summary>
        /// <param name="axle"></param>
        private void LerpToSteerAngle(Axle axle)
        {
            float angle = Mathf.Lerp(axle.InitialLerpAngle, axle.TargetLerpAngle, axle.TimeSinceStartLerp);
            axle.Right.steerAngle = angle;
            axle.Left.steerAngle = angle;
        }

    }
}
