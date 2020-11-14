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
    [RequireComponent(typeof(VehicleController))]
    public class AIMovement : MonoBehaviour
    {

        private float Cautiousness = 2.5f;
        private float InverseTrackingAccuracy= 20f;
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

        private void TranslateTracker()
        {
            if(Vector3.Distance(tracker.transform.position, transform.position) < InverseTrackingAccuracy)
            {
                tracker.transform.LookAt(track.waypoints[TargetIndex].transform);
                tracker.transform.Translate(0, 0, (transform.InverseTransformDirection(vehicle.rigidbody.velocity).z + 0.5f) * Time.deltaTime);
            }
        }

        private float CalulateCrossProduct()
        {
            Vector3 forward = transform.forward;
            Vector3 diff = tracker.transform.position - transform.position;

            //the cross product of a vector tells us which direction to steer towards the target
            return Vector3.Cross(forward, diff).y;
        }

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
            if (cross < -4)
            {
                if (leftPossible)
                {
                    steerAngle = -VehicleSettings.maxSteerAngle;
                }
                else if (!leftPossible && rightPossible)
                {
                    steerAngle = VehicleSettings.maxSteerAngle / 7;
                }
            }
            else if (cross > 4 && rightPossible)
            {
                if (rightPossible)
                {
                    steerAngle = VehicleSettings.maxSteerAngle;
                }
                else if (!rightPossible && leftPossible)
                {
                    steerAngle = -VehicleSettings.maxSteerAngle / 6;
                }
            }

            //Determine throttle
            if (toTarget)
            {
                motorTorque = VehicleSettings.maxMotorTorque;
            }
            else
            {
                brakeTorque = VehicleSettings.maxBrakeTorque / 2;
            }

            UpdateAxles(steerAngle, motorTorque, brakeTorque);
        }

        private Vector3 GetRaycastAngle(float angle)
        {
            return Quaternion.Euler(0, angle, 0) * transform.forward;
        }

        private void AvoidPossibleCollisions(ref bool rightPossible, ref bool leftPossible, ref bool toTarget)
        {
            int vehicleLayerMask = LayerMask.GetMask(new string[] { "Vehicle" });
            bool rightHit, leftHit, toTargetHit = false;

            rightHit = Physics.Raycast(transform.position, GetRaycastAngle(90), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(30), Cautiousness + 1f, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(135), Cautiousness + 1f, vehicleLayerMask);

            leftHit = Physics.Raycast(transform.position, GetRaycastAngle(-90), Cautiousness, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(-30), Cautiousness + 1f, vehicleLayerMask) ||
                Physics.Raycast(transform.position, GetRaycastAngle(-135), Cautiousness + 1f, vehicleLayerMask);

            RaycastHit hit;
            if(Physics.Raycast(transform.position, GetRaycastAngle(0), out hit, Cautiousness + 1f, vehicleLayerMask))
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

            rightPossible = !rightHit;
            leftPossible = !leftHit;
            toTarget = !toTargetHit;
        }

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

        private void ReevaluteTargetIndex()
        {
            if (Vector3.Distance(track.waypoints[TargetIndex].transform.position, tracker.transform.position) < 1)
            {
                TargetIndex = (TargetIndex == track.waypoints.Count - 1) ? 0 : TargetIndex + 1;
            }
        }
    }
}
