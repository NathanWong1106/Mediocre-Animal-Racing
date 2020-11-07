using Racing.Map.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Racing.User;
using System;

namespace Racing.Map
{
    /// <summary>
    /// Stores track checkpoints and race data
    /// </summary>
    public class Track : MonoBehaviour
    {
        public List<Checkpoint> checkpoints { get; set; } = new List<Checkpoint>();
        public Queue<Player> finishers { get; set; } = new Queue<Player>();
        public Checkpoint finishLine { get; set; }
        public int laps { get; set; } = 5;

        void Start()
        {
            checkpoints = transform.GetComponentsInChildren<Checkpoint>().ToList();
            finishLine = checkpoints.Find(c => c.isFinish);

            if (checkpoints.Count() == 0 || finishLine == null)
            {
                throw new System.Exception("~Track: No checkpoints or finish line found");
            }

            InitPlayers();
        }

        private void InitPlayers()
        {
            foreach (var player in FindObjectsOfType<Player>())
            {
                player.target = finishLine;
            }
        }
    }
}
