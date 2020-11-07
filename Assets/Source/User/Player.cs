using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Map.Tracking;
using Racing.Vehicle.Common;
using System;

namespace Racing.User
{
    public class Player : MonoBehaviour
    {
        public List<Checkpoint> checkpoints { get; set; } = new List<Checkpoint>();
        [NonSerialized] public Checkpoint target;
        public int lapNumber { get; set; } = 0;
    }
}
