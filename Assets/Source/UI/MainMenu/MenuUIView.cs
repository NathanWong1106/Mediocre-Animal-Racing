using UnityEngine;
using UnityEngine.UI;

namespace Racing.UI.MainMenu
{
    [RequireComponent(typeof(MenuUIController))]
    public class MenuUIView : MonoBehaviour, IUserInterfaceView
    {
        public Button StartButton;
        public Button ExitButton;

        private void Start()
        {
            UserInterface.CurrentView = this;
        }
    }

}