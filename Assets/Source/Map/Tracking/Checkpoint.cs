using System;
using System.Collections;
using System.Collections.Generic;
using Racing.User;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Racing.Map.Tracking
{
    /// <summary>
    /// Tag for checkpoint triggers
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        private Track track;
        public Checkpoint nextCheckpoint;
        public bool isFinish = false;

        private void Start()
        {
            track = FindObjectOfType<Track>();
        }

        public void AddCheckpoint(Player player)
        {
            if(track.checkpoints.Count == player.checkpoints.Count && isFinish)
            {
                player.checkpoints.Clear();
                player.lapNumber++;
                //track.finishers.Enqueue(player);
                Debug.Log(player.lapNumber);
            }

            player.checkpoints.Add(this);
            player.target = nextCheckpoint;
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                if (player.target == this)
                {
                    AddCheckpoint(player);
                }
            }
        }
    }  
}
