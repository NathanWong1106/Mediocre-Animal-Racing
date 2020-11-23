using Racing.Game;
using Racing.Map;
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

        public void SetPositionIndicator(int position)
        {
            view.PositionIndicator.SetText(GetPositionString(position));
        }

        public void SetLapIndicator(int laps)
        {
            view.LapIndicator.SetText(GetLapString(laps));
        }

        public void SetCountdownTimer(int seconds)
        {
            view.CountdownTimer.SetText(seconds.ToString());
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
