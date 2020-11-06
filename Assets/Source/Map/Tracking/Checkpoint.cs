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
        public void AddCheckpoint(Player player)
        {
            if (player.checkpoints.Contains(this))
                return;

            player.checkpoints.Add(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                AddCheckpoint(player);
            }
        }
    }
}
