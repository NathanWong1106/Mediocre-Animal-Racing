using Racing.Map.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Racing.Map
{
    public class Track : MonoBehaviour
    {
        public List<Checkpoint> checkpoints { get; set; } = new List<Checkpoint>();
        public int laps { get; set; } = 5;

        void Start()
        {
            checkpoints = transform.GetComponentsInChildren<Checkpoint>().ToList();
        }
    }
}
