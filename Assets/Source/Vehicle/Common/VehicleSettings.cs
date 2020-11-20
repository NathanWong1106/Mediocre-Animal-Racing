using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Vehicles.Common
{
    /// <summary>
    /// Contains all variables pertaining to the vehicle
    /// </summary>
    public static class VehicleSettings
    {
        /// <summary>
        /// Maximum torque the motor can apply to a wheel
        /// </summary>
        public static readonly float MaxMotorTorque = 900f;

        /// <summary>
        /// Maximum steer angle a wheel can have
        /// </summary>
        public static readonly float MaxSteerAngle = 25f;

        /// <summary>
        /// Maximum torque the brake can apply to a wheel
        /// </summary>
        public static readonly float MaxBrakeTorque = 15000f;

        /// <summary>
        /// Lerp multiplier to maxSteerAngle
        /// </summary>
        public static readonly float TurnLerpMultiplier = 6f; 

        /// <summary>
        /// Amount of upward force to jump with
        /// </summary>
        public static readonly float JumpForce = 18000f;

        /// <summary>
        /// Torque applied to vehicle mid-air
        /// </summary>
        public static readonly float AirTurnTorque = 4000f;

        /// <summary>
        /// Distance between the floor of the vehicle and the bottom of its wheels
        /// </summary>
        public static readonly float RideHeight = 1f;
    }
}
