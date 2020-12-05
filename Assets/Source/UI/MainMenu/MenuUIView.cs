using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Racing.UI.MainMenu
{
    [RequireComponent(typeof(MenuUIController), typeof(MenuDataManager))]
    public class MenuUIView : MonoBehaviour, IUserInterfaceView
    {
        [HideInInspector]
        public MenuUIController Controller;

        [HideInInspector]
        public MenuDataManager DataManager;

        public GameObject MainView;
        public GameObject CustomView;
        public GameObject SavedCustomPrefab;
        public GameObject SavedDataComponents;

        [HideInInspector] 
        public List<GameObject> MenuViews;

        public Button MainExitButton;
        public Button MainCustomRace;
        public Button CustomStartRace;
        public Button CustomSaveConfig;
        public Slider CustomTotalPlayers;
        public Slider CustomStartingPosition;

        public TextMeshProUGUI TotalPlayersIndicator;
        public TextMeshProUGUI StartingPositionIndicator;

        public TMP_InputField CustomSaveConfigInputField;

        private void Start()
        {
            UserInterface.CurrentView = this;
            MenuViews = new List<GameObject>() { MainView, CustomView };

            Controller = GetComponent<MenuUIController>();
            DataManager = GetComponent<MenuDataManager>();
        }
    }
}