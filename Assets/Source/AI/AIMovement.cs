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

        private Track track;
        private Vehicle.Common.Vehicle vehicle;
        private GameObject tracker;
        private float maxTrackerDistance = 20f;
        public int TargetIndex = 0;

        private void Start()
        {
            GetComponent<VehicleController>().inputType = InputType.AI;
            vehicle = GetComponent<Vehicle.Common.Vehicle>();
            track = FindObjectOfType<Track>();
            tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            DestroyImmediate(tracker.GetComponent<Collider>());
            tracker.transform.position = track.waypoints[TargetIndex].transform.position;
        }

        private void Update()
        {
            TranslateTracker();
            ReevaluateTurnAngle(CalulateCrossProduct());
            ReevaluteTargetIndex();
        }

        private void TranslateTracker()
        {
            Debug.DrawLine(transform.position, tracker.transform.position, Color.red);
            if(Vector3.Distance(tracker.transform.position, transform.position) < maxTrackerDistance)
            {
                tracker.transform.LookAt(track.waypoints[TargetIndex].transform);
                tracker.transform.Translate(0, 0, (transform.InverseTransformDirection(vehicle.rigidbody.velocity).z + 0.5f) * Time.deltaTime);
            }
        }

        private float CalulateCrossProduct()
        {
            Vector3 forward = transform.forward;
            Vector3 diff = tracker.transform.position - transform.position;

            //the cross product of a vector tells us which direction the to steer towards the target
            return Vector3.Cross(forward, diff).y;
        }

        private void ReevaluateTurnAngle(float cross)
        {
            int layer = LayerMask.GetMask(new string[] { "Vehicle" });
            Vector3 dir = (tracker.transform.position - transform.position).normalized;
;
            RaycastHit hit;

            //TODO: improve later so the AI don't kill each other 
            if(Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity, layer))
            {
                if(Vector3.Distance(hit.transform.position, transform.position) < 5)
                {
                    UpdateAxles(0, 0);
                    return;
                }
            }

            float steerAngle;

            if (cross < -0.5)
            {
                steerAngle = -VehicleSettings.maxSteerAngle;
            }
            else if (cross > 0.5)
            {
                steerAngle = VehicleSettings.maxSteerAngle;
            }
            else
            {
                steerAngle = 0;
            }

            UpdateAxles(steerAngle, VehicleSettings.maxMotorTorque);
        }

        private void UpdateAxles(float steerAngle, float motorTorque)
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
            }
        }

        private void ReevaluteTargetIndex()
        {
            if (Vector3.Distance(track.waypoints[TargetIndex].transform.position, tracker.transform.position) < track.waypoints[TargetIndex].radius)
            {
                TargetIndex = (TargetIndex == track.waypoints.Count - 1) ? 0 : TargetIndex + 1;
            }
        }

    }

    
}
