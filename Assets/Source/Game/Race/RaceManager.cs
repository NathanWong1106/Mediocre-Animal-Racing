﻿using Racing.User;
using Racing.Map;
using Racing.Util;
using Racing.UI;
using Racing.AI;
using Racing.Map.Tracking;
using Racing.Game.Management;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Racing.UI.InGame;
using Racing.Vehicles.Control;

namespace Racing.Game.Race
{
    [RequireComponent(typeof(TrackInstance))]
    public class RaceManager : MonoBehaviour
    {
        private float secondsToStart = 3;
        public List<Player> Players { get; set; }
        public List<Player> Finishers { get; set; } = new List<Player>();
        public int Laps { get; set; } = 2;
        public bool RaceStarted { get; set; } = false;


        private void Awake()
        {
            Players = FindObjectsOfType<Player>().ToList();
            RaceScene.CurrentGameManager = this;
            GameManager.PauseTimeScale(false);
            InitiateRaceStart();
        }

        private void FixedUpdate()
        {
            PositionTracker.DetermineUpdatePositions(Players);
            
        }

        /// <summary>
        /// Initializes and starts a timer for race start
        /// </summary>
        private void InitiateRaceStart()
        {
            CustomUnityTimer timer = gameObject.AddComponent(typeof(CustomUnityTimer)) as CustomUnityTimer;
            timer.Initialize(Converter.SecondsToMilliseconds(secondsToStart), Converter.SecondsToMilliseconds(1), 
                (s) => UserInterface.GetControllerAsType<InGameUIController>().SetCountdownTimer(Converter.MillisecondsToSeconds(s)), StartRace);
            timer.StartTimer();
        }

        /// <summary>
        /// Enables movement components for players on race start
        /// </summary>
        private void StartRace()
        {
            RaceStarted = true;
            Player.AIPlayers.ForEach(p => p.GetComponent<AIMovement>().enabled = true);
            Player.HumanPlayers.ForEach(p => p.GetComponent<VehicleController>().enabled = true);
        }

        public void OnPlayerFinish(Player player)
        {
            player.Finished = true;
            Finishers.Add(player);
            
            if (player.InputType == InputType.Player)
                UserInterface.GetControllerAsType<InGameUIController>().OnPlayerFinish(player);
        }
    }
}
