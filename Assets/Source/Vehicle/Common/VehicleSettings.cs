using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Vehicle.Common
{
    /// <summary>
    /// Contains all variables pertaining to the vehicle
    /// </summary>
    public static class VehicleSettings
    {
        /// <summary>
        /// Maximum torque the motor can apply to a wheel
        /// </summary>
        public static readonly float maxMotorTorque = 600f;

        /// <summary>
        /// Maximum steer angle a wheel can have
        /// </summary>
        public static readonly float maxSteerAngle = 25f;

        /// <summary>
        /// Maximum torque the brake can apply to a wheel
        /// </summary>
        public static readonly float maxBrakeTorque = 15000f;

        /// <summary>
        /// Lerp multiplier to maxSteerAngle
        /// </summary>
        public static readonly float turnLerp = 25f; 

        /// <summary>
        /// Amount of upward force to jump with
        /// </summary>
        public static readonly float jumpForce = 18000f;

        /// <summary>
        /// Torque applied to vehicle mid-air
        /// </summary>
        public static readonly float airTurnTorque = 4000f;

        /// <summary>
        /// Distance between the floor of the vehicle and the bottom of its wheels
        /// </summary>
        public static readonly float rideHeight = 1f;
    }
}
