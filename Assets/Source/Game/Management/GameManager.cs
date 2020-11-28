using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Racing.Game.Management
{
    public static class GameManager
    {
        public static GameState CurrentGameState { get; set; }

        public static void PauseTimeScale(bool paused)
        {
            Time.timeScale = paused ? 0 : 1;
            CurrentGameState = paused ? GameState.Paused : GameState.InGame;
        }

        public static void ToggleTimeScale()
        {
            Time.timeScale = IsPaused() ? 1 : 0;
            CurrentGameState = IsPaused() ? GameState.InGame : GameState.Paused;
        }

        public static bool IsPaused()
        {
            return CurrentGameState == GameState.Paused;
        }
        
        public static void ExitGame()
        {
            Application.Quit();
        }
    }
}
