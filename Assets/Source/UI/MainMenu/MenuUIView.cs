using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Racing.UI.MainMenu
{
    [RequireComponent(typeof(MenuUIController))]
    public class MenuUIView : MonoBehaviour, IUserInterfaceView
    {
        public GameObject MainView;
        public GameObject CustomView;
        public List<GameObject> MenuViews;

        public Button MainExitButton;
        public Button MainCustomRace;
        public Button CustomStartRace;
        public Slider CustomTotalPlayers;
        public Slider CustomStartingPosition;

        public TextMeshProUGUI TotalPlayersIndicator;
        public TextMeshProUGUI StartingPositionIndicator;

        private void Start()
        {
            UserInterface.CurrentView = this;
            MenuViews = new List<GameObject>() { MainView, CustomView };
        }
    }
}