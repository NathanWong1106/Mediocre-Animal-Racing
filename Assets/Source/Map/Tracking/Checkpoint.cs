using System;
using System.Collections;
using System.Collections.Generic;
using Racing.User;
using UnityEngine;

namespace Racing.Map.Tracking
{
    /// <summary>
    /// Tag for checkpoint colliders
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        public bool isFinish = false;
        public event Action<Player> CrossedFinish;

        public void AddCheckpoint(Player player)
        {
            player.checkpoints.Add(this);
            Debug.Log(player.checkpoints);
        }

        private void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                AddCheckpoint(player);

                if (isFinish)
                {
                    CrossedFinish.Invoke(player);
                }
            }
        }
    }
}
