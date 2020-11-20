using Racing.Map.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Racing.User;
using Racing.AI;
using System;

namespace Racing.Map
{
    /// <summary>
    /// Stores track variables
    /// </summary>
    public class Track : MonoBehaviour
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

        void Start()
        {
            Checkpoints = transform.Find("Checkpoints").GetComponentsInChildren<Checkpoint>().ToList();
            Waypoints = transform.Find("Waypoints").GetComponentsInChildren<Waypoint>().ToList();
            //FinishLine = Checkpoints.Find(c => c.IsFinish);
/*
            if (Checkpoints.Count() == 0 || FinishLine == null)
            {
                throw new System.Exception("~Track: No checkpoints or finish line found");
            }*/

            InitPlayers();
        }

        private void InitPlayers()
        {
/*            foreach (var player in FindObjectsOfType<Player>())
            {
                player.target = FinishLine;
            }*/
        }
    }
}
