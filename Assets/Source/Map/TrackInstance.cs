using Racing.AI;
using Racing.Map.Tracking;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Racing.Map
{
    public class TrackInstance : MonoBehaviour
    {
        /// <summary>
        /// Ordered checkpoints on the track
        /// <para>In the Unity Editor make sure to organize the checkpoints in descending order (top to bottom, first to last)</para>
        /// </summary>
        public List<Checkpoint> Checkpoints { get; set; }

        /// <summary>
        /// Ordered waypoints for AI on the track
        /// <para>In the Unity Editor make sure to organize the checkpoints in descending order (top to bottom, first to last)</para>
        /// </summary>
        public List<Waypoint> Waypoints { get; set; }
        public Checkpoint FinishLine { get; set; }

        void Awake()
        {
            Checkpoints = transform.Find("Checkpoints").GetComponentsInChildren<Checkpoint>().ToList();
            Waypoints = transform.Find("Waypoints").GetComponentsInChildren<Waypoint>().ToList();
            FinishLine = Checkpoints.Find(c => c.IsFinish);
            RaceScene.CurrentTrack = this;
        }
    }
}