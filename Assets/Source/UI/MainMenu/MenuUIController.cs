using Racing.Game.Management;
using Racing.Game.Storage;
using UnityEngine;

namespace Racing.UI.MainMenu
{
    [RequireComponent(typeof(MenuUIView))]
    public class MenuUIController : MonoBehaviour, IUserInterfaceController
    {
        private MenuUIView view;

        void Start()
        {
            view = GetComponent<MenuUIView>();
            UserInterface.CurrentController = this;
        }

        public void StartGame()
        {
            SceneLoader.LoadScene(Scenes.Debug);
        }

        public void ExitGame()
        {
            GameManager.ExitGame();
        }
    }
}
