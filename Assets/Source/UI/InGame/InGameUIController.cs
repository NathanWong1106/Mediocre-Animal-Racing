using Racing.User;
using Racing.Map;
using Racing.Util;
using Racing.Game.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Racing.UI.InGame
{
    [RequireComponent(typeof(InGameUIView))]
    public class InGameUIController : MonoBehaviour, IUserInterfaceController
    {
        private InGameUIView view;

        private void Start()
        {
            view = GetComponent<InGameUIView>();
            UserInterface.CurrentController = this;

            SetLapIndicator(1);
        }

        private void Update()
        {
            //Toggle in-game menu
            if (InputManager.Escape)
                ToggleMenu();
        }

        public void ToggleMenu()
        {
            GameManager.ToggleTimeScale();
            view.Menu.SetActive(!view.Menu.activeSelf);
            view.HUD.SetActive(!view.HUD.activeSelf);
        }

        public void RestartLevel()
        {
            SceneLoader.ReloadScene();
        }

        public void SetPositionIndicator(int position)
        {
            view.HUDPositionIndicator.SetText(GetPositionString(position));
        }

        public void SetLapIndicator(int laps)
        {
            view.HUDLapIndicator.SetText(GetLapString(laps));
        }

        public void SetCountdownTimer(float seconds)
        {
            view.HUDMessageDisplay.SetText(seconds.ToString());
            if (seconds == 0)
            {
                view.HUDMessageDisplay.enabled = false;
            }
        }

        public void OnPlayerFinish(Player player)
        {
            view.HUDMessageDisplay.enabled = true;
            view.HUDMessageDisplay.SetText($"Finished in {RaceScene.CurrentGameManager.Finishers.IndexOf(player) + 1}");
            view.HUDLapIndicator.SetText("Finished!");
        }

        private string GetPositionString(int position)
        {
            return $"Position {position}/{RaceScene.CurrentGameManager.Players.Count}";
        }

        private string GetLapString(int laps)
        {
            return $"Lap {laps}/{RaceScene.CurrentGameManager.Laps}";
        }
    }
}
