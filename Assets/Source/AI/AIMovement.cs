using Racing.AI;
using Racing.Map.Tracking;
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

        private float Cautiousness = 3f;
        private float InverseTrackingAccuracy= 30f;
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
            //DestroyImmediate(tracker.GetComponent<MeshRenderer>());
            tracker.transform.position = track.waypoints[TargetIndex].transform.position;
        }

        private void Update()
        {
            TranslateTracker();
            EvaluateMovement(CalulateCrossProduct());
            ReevaluteTargetIndex();
            Ghost();
        }

        //TODO: add transparency effect on ghost active
        private void Ghost()
        {
            if(Mathf.Abs(transform.InverseTransformDirection(vehicle.rigidbody.velocity).z) < 10f)
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
            if(Vector3.Distance(tracker.transform.position, transform.position) < InverseTrackingAccuracy)
            {
                tracker.transform.LookAt(track.waypoints[TargetIndex].transform);
                tracker.transform.Translate(0, 0, (20f) * Time.deltaTime);
            }
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
            //Collision avoidance
            bool rightPossible = false;
            bool leftPossible = false;
            bool toTarget = false;
            AvoidPossibleCollisions(ref rightPossible, ref leftPossible, ref toTarget);

            //Vehicle parameters
            float steerAngle = 0;
            float motorTorque = 0;
            float brakeTorque = 0;

            //Determine steerAngle
            if (!leftPossible && rightPossible)
            {
                steerAngle = VehicleSettings.maxSteerAngle / 15;
            }

            else if (!rightPossible && leftPossible)
            {
                steerAngle = -VehicleSettings.maxSteerAngle / 15;
            }

            if (cross < -4)
            {
                if (leftPossible)
                {
                    steerAngle = -VehicleSettings.maxSteerAngle;
                }
            }
            else if (cross > 4 && rightPossible)
            {
                if (rightPossible)
                {
                    steerAngle = VehicleSettings.maxSteerAngle;
                }
            }

            //Determine throttle
            if (toTarget)
            {
                motorTorque = VehicleSettings.maxMotorTorque * 1.2f;
            }
            else
            {
                brakeTorque = VehicleSettings.maxBrakeTorque / 2;
            }

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
        /// Produces a series of raycasts returning whether a specified direction should be taken
        /// </summary>
        private void AvoidPossibleCollisions(ref bool rightPossible, ref bool leftPossible, ref bool toTarget)
        {
            int vehicleLayerMask = LayerMask.GetMask(new string[] { "Vehicle" });
            bool rightHit, leftHit, toTargetHit = false;

            rightHit = Physics.Raycast(transform.position, GetRaycastAngle(90), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(30), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(135), Cautiousness + 2, vehicleLayerMask);

            leftHit = Physics.Raycast(transform.position, GetRaycastAngle(-90), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(-30), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(-135), Cautiousness + 2, vehicleLayerMask);

            RaycastHit hit;
            if(Physics.Raycast(transform.position, GetRaycastAngle(0), out hit, Cautiousness + 5f, vehicleLayerMask))
            {
                if(transform.InverseTransformDirection(hit.transform.GetComponent<Vehicle.Common.Vehicle>().rigidbody.velocity).z - transform.InverseTransformDirection(vehicle.rigidbody.velocity).z < 0)
                {
                    toTargetHit = true;
                }
                else
                {
                    toTargetHit = false;
                }
            }
            else if (Physics.Raycast(transform.position, GetRaycastAngle(15), out hit, Cautiousness + 2f, vehicleLayerMask))
            {
                if (transform.InverseTransformDirection(hit.transform.GetComponent<Vehicle.Common.Vehicle>().rigidbody.velocity).z - transform.InverseTransformDirection(vehicle.rigidbody.velocity).z < 0)
                {
                    toTargetHit = true;
                }
                else
                {
                    toTargetHit = false;
                }
            }
            else if (Physics.Raycast(transform.position, GetRaycastAngle(-15), out hit, Cautiousness + 2f, vehicleLayerMask))
            {
                if (transform.InverseTransformDirection(hit.transform.GetComponent<Vehicle.Common.Vehicle>().rigidbody.velocity).z - transform.InverseTransformDirection(vehicle.rigidbody.velocity).z < 0)
                {
                    toTargetHit = true;
                }
                else
                {
                    toTargetHit = false;
                }
            }

            rightPossible = !rightHit;
            leftPossible = !leftHit;
            toTarget = !toTargetHit;
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
