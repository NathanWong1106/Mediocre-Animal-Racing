using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Racing.UI.InGame
{
    [RequireComponent(typeof(InGameUIController))]
    public class InGameUIView : MonoBehaviour, IUserInterfaceView
    {
        //Set in Unity Editor
        public GameObject HUD;
        public GameObject Menu;
        public TextMeshProUGUI HUDPositionIndicator;
        public TextMeshProUGUI HUDLapIndicator;
        public TextMeshProUGUI HUDMessageDisplay;
        public Button MenuRestart;
        public Button MenuMainMenu;
        public Button MenuExitGame;
        public Button MenuContinue;

        private void Start()
        {
            UserInterface.CurrentView = this;
        }
    }
}
