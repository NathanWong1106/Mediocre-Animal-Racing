using UnityEngine;
using Racing.Game.Storage;

namespace Racing.Game.Management
{
    public static class GameManager
    {
        public static Data Data;
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
