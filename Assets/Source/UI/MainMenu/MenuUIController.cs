using Racing.Game.Management;
using Racing.Game.Storage;
using Racing.Util.Serialization;
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

            view.CustomTotalPlayers.onValueChanged.AddListener((p) => OnCustomPlayerCountChange(p));
            view.CustomStartingPosition.onValueChanged.AddListener((p) => OnCustomStartingPositionChange(p));
            ReevaluateViews(view.MainView);
        }

        public void ExitGame()
        {
            GameManager.ExitGame();
        }

        public void OnCustomPlayerCountChange(float players)
        {
            view.CustomStartingPosition.maxValue = players;
            view.TotalPlayersIndicator.SetText(players.ToString());
        }

        public void OnCustomStartingPositionChange(float position)
        {
            view.StartingPositionIndicator.SetText(position.ToString());
        }

        public void ReevaluateViews(GameObject visible)
        {
            foreach (GameObject view in view.MenuViews)
                view.SetActive(view == visible);
        }

        public void OnCustomRaceStart()
        {
            GameManager.Data = new Data()
            {
                AIPlayerCount = (int)view.CustomTotalPlayers.value - 1,
                PlayerCount = 1,
                PlayerStartPosition = (int)view.CustomStartingPosition.value
            };
            SceneLoader.LoadScene(Scenes.Debug);
        }

        public void SetCustomToDefault()
        {
            GameManager.Data = Data.Default;
            view.CustomTotalPlayers.value = GameManager.Data.PlayerCount + GameManager.Data.AIPlayerCount;
            view.CustomStartingPosition.value = GameManager.Data.PlayerStartPosition;
        }

        public void SetCustomFromPlayerPref(string name)
        {
            GameManager.Data = Data.CustomRaceData.Find(d => d.Name == name);
        }

        public void SaveRaceConfig(string name)
        {
            Data data = new Data()
            {
                AIPlayerCount = (int)view.CustomTotalPlayers.value - 1,
                PlayerCount = 1,
                PlayerStartPosition = (int)view.CustomStartingPosition.value,
                Name = name
            };

            if (Data.CustomRaceData.Contains(data))
                Data.CustomRaceData[Data.CustomRaceData.IndexOf(data)] = data;
            else
                Data.CustomRaceData.Add(data);

            Serializer.SerializeToPlayerPrefs(Data.CustomDataKey, Data.CustomRaceData);
            GameManager.Data = data;
        }

        public void RemoveRaceConfig(string name)
        {

        }
    }
}
