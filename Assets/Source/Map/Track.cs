using Racing.Map.Tracking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Racing.User;

namespace Racing.Map
{
    /// <summary>
    /// Stores track checkpoints and race data
    /// </summary>
    public class Track : MonoBehaviour
    {
        public List<Checkpoint> checkpoints { get; set; } = new List<Checkpoint>();
        public Queue<Player> finishers { get; set; } = new Queue<Player>();
        public FinishLine finishLine { get; set; }
        public int laps { get; set; } = 5;

        void Start()
        {
            checkpoints = transform.GetComponentsInChildren<Checkpoint>().ToList();
            finishLine = FindObjectOfType<FinishLine>();

            if(checkpoints.Count() == 0 || finishLine == null)
            {
                throw new System.Exception("~Track: No checkpoints or finish line found");
            }

            finishLine.onFinishCrossed += (p) => OnFinishCrossed(p);
        }

        //TODO: Make sure the player checkpoints are in the same order as the checkpoint positions
        private void OnFinishCrossed(Player player)
        {
            var intersection = player.checkpoints.Intersect(checkpoints);

            if(intersection.Count() == checkpoints.Count())
            {
                player.lapNumber++;
                Debug.Log(player.lapNumber);
            }

            if(player.lapNumber == laps)
            {
                finishers.Enqueue(player);
            }

            player.checkpoints.Clear();
        }
    }
}
