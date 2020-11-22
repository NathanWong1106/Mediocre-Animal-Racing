using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="https://docs.unity3d.com/Manual/WheelColliderTutorial.html"/>
/// </summary>

namespace Racing.Vehicles.Components
{
    /// <summary>
    /// Contains relevant fields of a vehicle axle
    /// </summary>
    [System.Serializable]
    public class Axle
    {
        public WheelCollider Left;
        public WheelCollider Right;
        
        public bool Motor; // attached to a motor?
        public bool Steering; // steer angle applicable?
        public bool Braking; // brake applicable?

        [HideInInspector] public float TargetLerpAngle = 0;
        [HideInInspector] public float PreviousTargetLerpAngle = 0;
        [HideInInspector] public float InitialLerpAngle = 0;
        [HideInInspector] public bool IsLerping = false;
        [HideInInspector] public float TimeSinceStartLerp = 0;

        public static void InitAxleLerp(Axle axle, float initAngle)
        {
            axle.TargetLerpAngle = initAngle;
            axle.PreviousTargetLerpAngle = initAngle;
            axle.InitialLerpAngle = initAngle;
        }

        public static void UpdateVisuals(Axle axle)
        {
            ApplyLocalPositionToVisuals(axle.Right);
            ApplyLocalPositionToVisuals(axle.Left);
        }

        /// <summary>
        /// Applies Wheel Collider rotation and position to their visual elements
        /// </summary>
        /// <param name="collider">Wheel Collider that the visual is attached to</param>
        private static void ApplyLocalPositionToVisuals(WheelCollider collider)
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
}
