using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Vehicle.Common;
using Racing.Map;
using Racing.Util;

namespace Racing.AI
{
    [RequireComponent(typeof(Vehicle.Common.Vehicle))]
    public class AIMovement : MonoBehaviour
    {

        private float Cautiousness = 4f;
        private float InverseTrackingAccuracy = 20f;
        private float InverseBrakePriority = 1f;
        private float ThrottleMultiplier = 1.1f;
        public bool RandomizeParameters = true;

        public bool debug = false;

        private Track track;
        private Vehicle.Common.Vehicle vehicle;
        private GameObject tracker;

        [System.NonSerialized]
        public int TargetIndex = 0;

        private void Start()
        {
            GetComponent<VehicleController>().inputType = InputType.AI;
            vehicle = GetComponent<Vehicle.Common.Vehicle>();
            track = FindObjectOfType<Track>();

            tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            DestroyImmediate(tracker.GetComponent<Collider>());
            DestroyImmediate(tracker.GetComponent<MeshRenderer>());
            tracker.transform.position = track.waypoints[TargetIndex].transform.position;

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
            if (Mathf.Abs(transform.InverseTransformDirection(vehicle.rigidbody.velocity).z) < 10f)
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
                tracker.transform.LookAt(track.waypoints[TargetIndex].transform);
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

            steerAngle = VehicleSettings.maxSteerAngle * steerMultiplier;

            //Determine throttle
            motorTorque = (1 - brakePriority) * VehicleSettings.maxMotorTorque * ThrottleMultiplier;
            brakeTorque = brakePriority * (VehicleSettings.maxBrakeTorque / InverseBrakePriority);


            UpdateAxles(steerAngle, motorTorque, brakeTorque);
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
                        float deltaSpeed = transform.InverseTransformDirection(vehicle.rigidbody.velocity).z - transform.InverseTransformDirection(hit.transform.GetComponent<Rigidbody>().velocity).z;

                        if (deltaSpeed > 1.5)
                        {
                            float newBrakePriority = deltaSpeed / distance;

                            if (newBrakePriority > brakePriority)
                            {
                                brakePriority = newBrakePriority;
                            }
                        }
                    }
                    else if (i >= 10 && i <= 170)
                    {
                        float newRightPressure = -((Cautiousness - distance) / Cautiousness);

                        if (newRightPressure < rightPressure)
                        {
                            rightPressure = newRightPressure;
                        }
                    }
                    else if (i <= -10 && i >= -170)
                    {
                        float newLeftPressure = (Cautiousness - distance) / Cautiousness;

                        if (newLeftPressure > leftPressure)
                        {
                            leftPressure = newLeftPressure;
                        }
                    }
                }
            }

            brakePriority /= 2;
            brakePriority = Mathf.Clamp(brakePriority, 0, 1);
        }

        /// <summary>
        /// Updates axle parameters with the given paramaters
        /// </summary>
        private void UpdateAxles(float steerAngle, float motorTorque, float brakeTorque)
        {
            foreach (var axle in vehicle.axles)
            {
                if (axle.steering)
                {
                    axle.targetLerpAngle = steerAngle;
                }

                if (axle.motor)
                {
                    axle.right.motorTorque = motorTorque;
                    axle.left.motorTorque = motorTorque;
                }

                if (axle.braking)
                {
                    axle.right.brakeTorque = brakeTorque;
                    axle.left.brakeTorque = brakeTorque;
                }
            }
        }

        /// <summary>
        /// Increments the target index given the tracker position
        /// </summary>
        private void ReevaluteTargetIndex()
        {
            if (Vector3.Distance(track.waypoints[TargetIndex].transform.position, tracker.transform.position) < 1)
            {
                TargetIndex = (TargetIndex == track.waypoints.Count - 1) ? 0 : TargetIndex + 1;
            }
        }
    }
}
