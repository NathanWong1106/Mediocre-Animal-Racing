using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Map.Tracking;
using Racing.Vehicles.Components;
using System;
using Racing.Map;

namespace Racing.User
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// List of all checkpoints the player has passed through in one lap
        /// </summary>
        public List<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
        public int TargetCheckpointIndex { get; set; } = 0;
        public int PreviousCheckpointIndex { get; set; }
        public int LapNumber { get; set; } = 0;
        public int Position { get; set; } = 0;


        public override string ToString()
        {
            return $"{transform.parent.name} || {LapNumber} || {TargetCheckpointIndex} || {Vector3.Distance(transform.position, Track.Current.Checkpoints[TargetCheckpointIndex].transform.position)}";
        }

        /// <summary>
        /// Returns a modified TargetCheckpointIndex for determination of track position
        /// </summary>
        public int TargetIndexForPosition()
        {
            if(PreviousCheckpointIndex == Track.Current.Checkpoints.Count - 1 && TargetCheckpointIndex == 0)
            {
                return Track.Current.Checkpoints.Count;
            }

            return TargetCheckpointIndex;
        }
    }
}
