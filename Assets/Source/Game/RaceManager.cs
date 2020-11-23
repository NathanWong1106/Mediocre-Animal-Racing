using Racing.User;
using Racing.Map;
using Racing.Util;
using Racing.UI;
using Racing.AI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Racing.UI.InGame;

namespace Racing.Game
{
    [RequireComponent(typeof(TrackInstance))]
    public class RaceManager : MonoBehaviour
    {
        public List<Player> Players { get; set; }
        public Queue<Player> Finishers { get; set; } = new Queue<Player>();
        public int Laps { get; set; } = 5;
        public bool RaceStarted { get; set; } = false;


        private void Start()
        {
            Players = FindObjectsOfType<Player>().ToList();
            RaceScene.CurrentGameManager = this;

            InitiateRaceStart();
        }

        private void FixedUpdate()
        {
            PositionTracker.DetermineUpdatePositions(Players);
        }

        private void InitiateRaceStart()
        {
            CustomUnityTimer timer = gameObject.AddComponent(typeof(CustomUnityTimer)) as CustomUnityTimer;
            timer.Initialize(3000, 1000, (s) => UserInterface.GetControllerAsType<InGameUIController>().SetCountdownTimer((int)Converter.MillisecondsToSeconds(s)), StartRace);
            timer.StartTimer();
        }

        private void StartRace()
        {
            RaceStarted = true;
            Player.AIPlayers.ForEach(p => p.GetComponent<AIMovement>().enabled = true);
        }
    }
}
