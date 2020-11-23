using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Vehicles.Components;
using Racing.Map;
using Racing.Util;

namespace Racing.AI
{
    [RequireComponent(typeof(Vehicle), typeof(VehicleUpdater))]
    public class AIMovement : MonoBehaviour
    {
        private float Cautiousness = 4f;
        private float InverseAvoidancePriority = 1.3f;
        private float InverseTrackingAccuracy = 20f;
        private float InverseBrakePriority = 3f;
        private float ThrottleMultiplier = 1.1f;
        public bool RandomizeParameters = true;

        public bool debug = false;

        private Vehicle vehicle;
        private VehicleUpdater vehicleUpdater;
        private GameObject tracker;

        [System.NonSerialized]
        public int TargetIndex = 0;

        private void Start()
        {
            vehicle = GetComponent<Vehicle>();
            vehicleUpdater = GetComponent<VehicleUpdater>();

            tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            DestroyImmediate(tracker.GetComponent<Collider>());
            DestroyImmediate(tracker.GetComponent<MeshRenderer>());

            tracker.transform.position = RaceScene.CurrentTrack.Waypoints[TargetIndex].transform.position;

            if (RandomizeParameters)
                ApplyRandomParameters();
        }

        private void Update()
        {
            TranslateTracker();
            EvaluateMovement(CalulateCrossProduct());
            ReevaluteTargetIndex();
            Ghost();
        }

        /// <summary>
        /// Applies a random modifier to AI parameters (makes it a bit more dynamic ya know?)
        /// </summary>
        private void ApplyRandomParameters()
        {
            Cautiousness *= UnityEngine.Random.Range(0.95f, 1.05f);
            ThrottleMultiplier *= UnityEngine.Random.Range(0.95f, 1.05f);
            InverseBrakePriority *= UnityEngine.Random.Range(0.95f, 1.05f);
        }

        //TODO: add transparency effect on ghost active
        private void Ghost()
        {
            //TODO: some arbitrary value - change when finalized
            if (Mathf.Abs(transform.InverseTransformDirection(vehicle.Rigidbody.velocity).z) < 10f)
            {
                vehicle.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                vehicle.GetComponent<BoxCollider>().enabled = true;
            }
        }

        /// <summary>
        /// Translates tracker along the predefined waypoint path
        /// </summary>
        private void TranslateTracker()
        {
            if (Vector3.Distance(tracker.transform.position, transform.position) < InverseTrackingAccuracy)
            {
                tracker.transform.LookAt(RaceScene.CurrentTrack.Waypoints[TargetIndex].transform);
                tracker.transform.Translate(0, 0, (20f) * Time.deltaTime);
            }

            //Debug.DrawLine(transform.position, tracker.transform.position);
        }

        /// <summary>
        /// Returns the cross product between the tracker and vehicle positions
        /// </summary>
        private float CalulateCrossProduct()
        {
            Vector3 forward = transform.forward;
            Vector3 diff = tracker.transform.position - transform.position;

            //the cross product of a vector tells us which direction to steer towards the target
            return Vector3.Cross(forward, diff).y;
        }

        /// <summary>
        /// Decides what action/direction to take given the cross product
        /// </summary>
        private void EvaluateMovement(float cross)
        {
            float rightPressure = 0;
            float leftPressure = 0;
            float brakePriority = 0;
            AvoidPossibleCollisions(ref leftPressure, ref rightPressure, ref brakePriority);

            //Vehicle parameters
            float steerAngle = 0;
            float motorTorque = 0;
            float brakeTorque = 0;


            float trackGravity = 0;

            if (cross < -4)
            {
                trackGravity = -1;
            }
            else if (cross > 4)
            {
                trackGravity = 1;
            }

            float steerMultiplier = rightPressure + leftPressure + trackGravity;

            steerMultiplier = Mathf.Clamp(steerMultiplier, -1, 1);

            //Less avoidance on straights
            if (cross > -4 && cross < 4)
            {
                steerMultiplier /= 2;
            }

            steerAngle = VehicleSettings.MaxSteerAngle * steerMultiplier;

            //Determine throttle
            if(brakePriority >= 0.5f)
            {
                motorTorque = 0;
            }
            else
            {
                motorTorque = (1 - brakePriority) * VehicleSettings.MaxMotorTorque * ThrottleMultiplier;
            }
            brakeTorque = brakePriority * (VehicleSettings.MaxBrakeTorque / InverseBrakePriority);


            //UpdateAxles(steerAngle, motorTorque, brakeTorque);
            vehicleUpdater.TargetLerpAngle = steerAngle;
            vehicleUpdater.MotorTorque = motorTorque;
            vehicleUpdater.BrakeTorque = brakeTorque;
        }

        /// <summary>
        /// Returns an angle relative to the forward transform of the vehicle
        /// </summary>
        private Vector3 GetRaycastAngle(float angle)
        {
            return Quaternion.Euler(0, angle, 0) * transform.forward;
        }

        /// <summary>
        /// Produces a series of raycasts returning the "pressure" from other cars around the current vehicle
        /// </summary>
        private void AvoidPossibleCollisions(ref float leftPressure, ref float rightPressure, ref float brakePriority)
        {
            int vehicleLayerMask = LayerMask.GetMask(new string[] { "Vehicle" });
            RaycastHit hit;

            for (float i = -170; i <= 170; i += 15)
            {
                float raycastDistanceAdd = 0;
                if (i < 45 && i > -45)
                    raycastDistanceAdd = 2;

                if (debug)
                    Debug.DrawRay(transform.position, GetRaycastAngle(i) * (Cautiousness + raycastDistanceAdd));
                if (Physics.Raycast(transform.position, GetRaycastAngle(i), out hit, Cautiousness + raycastDistanceAdd, vehicleLayerMask))
                {
                    float distance = hit.distance;

                    if (i < 45 && i > -45)
                    {
                        float deltaSpeed = transform.InverseTransformDirection(vehicle.Rigidbody.velocity).z - transform.InverseTransformDirection(hit.transform.GetComponent<Rigidbody>().velocity).z;

                        if (deltaSpeed > 1.5) //TODO: some arbitrary number - use a variable
                        {
                            float newBrakePriority = deltaSpeed / distance;
                            brakePriority = newBrakePriority > brakePriority ? newBrakePriority : brakePriority;
                        }
                    }

                    // Simple linear equation representing the inverse percentage of distance in Cautiousness
                    else if (i >= 10 && i <= 170)
                    {
                        float newRightPressure = -((Cautiousness - distance) / Cautiousness);
                        rightPressure = newRightPressure < rightPressure ? newRightPressure : rightPressure;
                    }
                    else if (i <= -10 && i >= -170)
                    {
                        float newLeftPressure = (Cautiousness - distance) / Cautiousness;
                        leftPressure = newLeftPressure > leftPressure ? newLeftPressure : leftPressure;
                    }
                }
            }

            leftPressure /= InverseAvoidancePriority;
            rightPressure /= InverseAvoidancePriority;
            brakePriority /= InverseBrakePriority;
            brakePriority = Mathf.Clamp(brakePriority, 0, 1);
        }

        /// <summary>
        /// Increments the target index given the tracker position
        /// </summary>
        private void ReevaluteTargetIndex()
        {
            if (Vector3.Distance(RaceScene.CurrentTrack.Waypoints[TargetIndex].transform.position, tracker.transform.position) < 1)
            {
                TargetIndex = (TargetIndex == RaceScene.CurrentTrack.Waypoints.Count - 1) ? 0 : TargetIndex + 1;
            }
        }
    }
}
