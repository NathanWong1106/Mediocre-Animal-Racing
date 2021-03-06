﻿using Racing.User;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Racing.Map.Tracking
{
    public static class PositionTracker
    {
        /// <summary>
        /// Updates the player list with player positions in the race
        /// </summary>
        /// <remarks>Sorts players by lap number --> target checkpoint --> distance to target checkpoint</remarks>
        /// <param name="players">List of players in the race</param>
        public static void DetermineUpdatePositions(List<Player> players)
        {
            // Descending: Greatest --> Smallest
            players = players.OrderByDescending(p => p.LapNumber)
                .ThenByDescending(p => p.TargetIndexForPosition())
                .ThenBy(p => Vector3.Distance(p.transform.position, RaceScene.CurrentTrack.Checkpoints[p.TargetCheckpointIndex].transform.position))
                .ToList();

            for (int i = 1; i <= players.Count; i++)
                players[i - 1].Position = i;
        }
    }
}
