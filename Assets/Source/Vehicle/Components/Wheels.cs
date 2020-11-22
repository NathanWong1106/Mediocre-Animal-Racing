/*using Racing.Vehicle.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Deprecated. Check Racing.Vehicle.Common.VehicleUpdater.cs
/// </summary>
[RequireComponent(typeof(Vehicle))]
public class Wheels : MonoBehaviour
{
    Vehicle vehicle;

    private void Start()
    {
        vehicle = GetComponent<Vehicle>();
        InitAxles();
    }

    private void Update()
    {
        foreach(Axle axle in vehicle.axles)
        {
            RecalculateLerp(axle);
            if (axle.isLerping)
            {
                axle.time += Time.deltaTime * VehicleSettings.turnLerp;
                LerpToSteerAngle(axle);
            }
        }
    }

    private void InitAxles()
    {
        foreach(Axle axle in vehicle.axles)
        {
            Axle.InitAxleLerp(axle, axle.right.steerAngle);
        }
    }

    /// <summary>
    /// Sets the axle steer angle to an angle relative to the lerp timeframe
    /// </summary>
    /// <param name="axle"></param>
    private void LerpToSteerAngle(Axle axle)
    {
        float angle = Mathf.Lerp(axle.startLerpAngle, axle.targetLerpAngle, axle.time);
        axle.right.steerAngle = angle;
        axle.left.steerAngle = angle;
    }

    /// <summary>
    /// Recalculates wheel lerp variables based on the current targetLerpAngle of an axis
    /// </summary>
    /// <param name="axle"></param>
    private void RecalculateLerp(Axle axle)
    {
        if(axle.lastTargetLerpAngle != axle.targetLerpAngle) // User has changed directions
        {
            axle.isLerping = true;
            axle.time = 0;
            axle.startLerpAngle = axle.right.steerAngle;
            axle.lastTargetLerpAngle = axle.targetLerpAngle;
        }
        else if (axle.right.steerAngle == axle.targetLerpAngle) // Maximum steer angle has been reached
        {
            axle.isLerping = false;
        }
        else // Axle is in the middle of lerping
        {
            axle.isLerping = true;
        }
    }
}*/
