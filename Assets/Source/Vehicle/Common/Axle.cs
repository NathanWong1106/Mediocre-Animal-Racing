using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="https://docs.unity3d.com/Manual/WheelColliderTutorial.html"/>
/// </summary>

namespace Racing.Vehicle.Common
{
    [System.Serializable]
    public class Axle
    {
        public WheelCollider left;
        public WheelCollider right;
        public bool motor; // attached to a motor?
        public bool steering; // steer angle applicable?
        public bool braking; // brake applicable?
        public bool handBrake; // handbrake applicable?
    }
}
