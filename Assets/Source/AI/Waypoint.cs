using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.AI
{
    [System.Serializable]
    public class Waypoint : MonoBehaviour
    {
        public Waypoint nextWaypoint;
        public float radius;
    }
}
