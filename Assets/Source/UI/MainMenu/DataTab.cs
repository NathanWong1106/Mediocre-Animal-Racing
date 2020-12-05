using Racing.UI;
using Racing.Game.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Racing.UI.MainMenu
{
    /// <summary>
    /// Contains the components of a Custom Race Data Tab
    /// </summary>
    public class DataTab : MonoBehaviour
    {
        public static List<DataTab> tabs = new List<DataTab>();
        
        private Data data;
        
        public Data Data {
            get
            {
                return data;
            }
            set
            {
                data = value;
                UpdateVisuals();
                UpdateListeners();
            }
        }
        public Button Import { get; set; }
        public Button Delete { get; set; }
        private TextMeshProUGUI text { get; set; }

        private void Awake()
        {
            Import = transform.Find("Import").GetComponent<Button>();
            Delete = transform.Find("Delete").GetComponent<Button>();
            text = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            tabs.Add(this);
        }

        private void UpdateVisuals()
        {
            text.text = data.Name;
        }

        private void UpdateListeners()
        {
            Import.onClick.RemoveAllListeners();
            Delete.onClick.RemoveAllListeners();

            //add listeners
            Import.onClick.AddListener(() => UserInterface.GetControllerAsType<MenuUIController>().SetCustomFromPlayerPref(Data));
            Delete.onClick.AddListener(() => UserInterface.GetControllerAsType<MenuUIController>().RemoveRaceConfig(Data));
        }

        public static void ReevaluateTabs(Transform parent, GameObject prefab)
        {
            ClearTabs();

            foreach(Data data in Data.CustomRaceData)
            {
                DataTab tab = Instantiate(prefab, parent).GetComponent<DataTab>();
                tab.Data = data;
            }
        }

        public static void ClearTabs()
        {
            foreach(DataTab dt in tabs)
            {
                Destroy(dt.gameObject);
            }
            tabs.Clear();
        }
    }
}
