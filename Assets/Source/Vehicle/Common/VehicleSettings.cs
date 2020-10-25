using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Vehicle.Common
{
    public static class VehicleSettings
    {
        public static float maxMotorTorque = 400f; // maximum torque the motor can apply to a wheel
        public static float maxSteerAngle = 30f; // maximum steer angle a wheel can have
        public static float maxBrakeTorque = 400f; // maximum torque the barke can apply to a wheel
        public static float turnLerp = 0.5f; //Amount of time to lerp to maxSteerAngle
        public static float jumpForce = 12000f;
        public static float rideHeight = 1f;
    }
}
