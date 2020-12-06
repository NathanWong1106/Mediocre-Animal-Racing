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

        // Awake() does not get called on a cold start of the build. This is a fix... idk anymore
        /// <summary>
        /// Method must be called when a new DataTab is instantiated
        /// </summary>
        private void Init(Data data)
        {
            Import = transform.Find("Import").GetComponent<Button>();
            Delete = transform.Find("Delete").GetComponent<Button>();
            text = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            Data = data;
            tabs.Add(this);
        }

        private void UpdateVisuals()
        {
            text.text = Data.Name;
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
                var tab = Instantiate(prefab, parent);
                tab.GetComponent<DataTab>().Init(data);
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

        private void OnDestroy()
        {
            tabs.Remove(this);
        }
    }
}
