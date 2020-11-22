using Racing.User;
using Racing.Map;
using Racing.Map.Tracking;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.Game
{
    [RequireComponent(typeof(TrackInstance))]
    public class RaceManager : MonoBehaviour
    {
        public List<Player> Players { get; set; }
        public Queue<Player> Finishers { get; set; } = new Queue<Player>();
        public int Laps { get; set; } = 5;

        private void Start()
        {
            Players = FindObjectsOfType<Player>().ToList();
        }

        private void FixedUpdate()
        {
            PositionTracker.DetermineUpdatePositions(Players);
        }
    }
}
