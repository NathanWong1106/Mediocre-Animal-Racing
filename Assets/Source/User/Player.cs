using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Map.Tracking;
using Racing.Vehicles.Common;
using System;

namespace Racing.User
{
    public class Player : MonoBehaviour
    {
        /// <summary>
        /// List of all checkpoints the player has passed through in one lap
        /// </summary>
        public List<Checkpoint> Checkpoints { get; set; } = new List<Checkpoint>();
        [NonSerialized] public int TargetCheckpointIndex = 0;
        public int LapNumber = 0;
    }
}
