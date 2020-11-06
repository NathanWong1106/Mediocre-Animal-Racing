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
        public float lerpSpeed;
        public bool motor; // attached to a motor?
        public bool steering; // steer angle applicable?
        public bool braking; // brake applicable?

        [NonSerialized] public float targetLerpAngle;
        [NonSerialized] public float lastTargetLerpAngle;
        [NonSerialized] public float startLerpAngle;
        [NonSerialized] public bool isLerping = false;
        [NonSerialized] public float time = 0;

        public static void InitAxleLerp(Axle axle, float initAngle)
        {
            axle.targetLerpAngle = initAngle;
            axle.lastTargetLerpAngle = initAngle;
            axle.startLerpAngle = initAngle;
        }
    }
}
