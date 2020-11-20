using System;
using System.Collections;
using System.Collections.Generic;
using Racing.User;
using UnityEngine;

namespace Racing.Map.Tracking
{
    /// <summary>
    /// Tag for checkpoint triggers
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        private Track track;
        public bool IsFinish = false;

        private void Start()
        {
            track = FindObjectOfType<Track>();
        }

        public void AddCheckpoint(Player player)
        {
            if(track.Checkpoints.Count == player.Checkpoints.Count && IsFinish)
            {
                player.Checkpoints.Clear();
                player.LapNumber++;
                //track.finishers.Enqueue(player);
            }

            player.Checkpoints.Add(this);
            player.TargetCheckpointIndex = (player.TargetCheckpointIndex == track.Checkpoints.Count - 1) ? 0 : player.TargetCheckpointIndex + 1;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                if (track.Checkpoints[player.TargetCheckpointIndex]  == this)
                {
                    AddCheckpoint(player);
                }
            }
        }
    }  
}
