using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Racing.Map.Tracking;
using Racing.Vehicle.Common;

namespace Racing.User
{
    public class Player : MonoBehaviour
    {
        public List<Checkpoint> checkpoints { get; set; } = new List<Checkpoint>();
        public int lapNumber { get; set; } = 0;
    }
}
