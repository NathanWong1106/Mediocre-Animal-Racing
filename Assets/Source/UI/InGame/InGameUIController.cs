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
        private RaceManager raceManager;

        private void Start()
        {
            view = GetComponent<InGameUIView>();
            raceManager = Track.Current.GetComponent<RaceManager>();
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

        private string GetPositionString(int position)
        {
            return $"Position {position}/{raceManager.Players.Count}";
        }

        private string GetLapString(int laps)
        {
            return $"Lap {laps}/{raceManager.Laps}";
        }
    }
}
