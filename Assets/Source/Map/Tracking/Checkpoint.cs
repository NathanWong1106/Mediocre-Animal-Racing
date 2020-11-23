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
        public bool IsFinish = false;

        public void AddCheckpoint(Player player)
        {
            if(RaceScene.CurrentTrack.Checkpoints.Count == player.Checkpoints.Count && IsFinish)
            {
                player.Checkpoints.Clear();
                player.LapNumber++;
                //track.finishers.Enqueue(player);
            }

            player.Checkpoints.Add(this);
            player.PreviousCheckpointIndex = player.TargetCheckpointIndex;
            player.TargetCheckpointIndex = (player.TargetCheckpointIndex == RaceScene.CurrentTrack.Checkpoints.Count - 1) ? 0 : player.TargetCheckpointIndex + 1;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                if (RaceScene.CurrentTrack.Checkpoints[player.TargetCheckpointIndex]  == this)
                {
                    AddCheckpoint(player);
                }
            }
        }
    }  
}
