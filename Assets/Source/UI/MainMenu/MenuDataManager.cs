using Racing.Game.Storage;
using Racing.Util.Serialization;
using System;
using UnityEngine;

namespace Racing.UI.MainMenu
{
    public class MenuDataManager : MonoBehaviour
    {
        private MenuUIView view;

        public event Action OnDataModified;

        private void Start()
        {
            view = GetComponent<MenuUIView>();
            ReevaluteSavedTabs();
        }

        public void ReevaluteSavedTabs()
        {
            DataTab.ReevaluateTabs(view.SavedDataComponents.transform, view.SavedCustomPrefab);
            
            if (OnDataModified != null) 
                OnDataModified.Invoke();
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
            ReevaluteSavedTabs();
        }

        public void RemoveRaceConfig(Data data)
        {
            Data toRemove = Data.CustomRaceData.Find(d => d == data);
            Data.CustomRaceData.Remove(toRemove);

            Serializer.SerializeToPlayerPrefs(Data.CustomDataKey, Data.CustomRaceData);
            ReevaluteSavedTabs();
        }
    }
}
