using Racing.UI;
using Racing.UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

public class DataScrollResizer : MonoBehaviour
{
    private MenuUIView view;
    private RectTransform rectTransform;

    private void Start()
    {
        view = UserInterface.GetViewAsType<MenuUIView>();
        view.DataManager.OnDataModified += OnDataModified;
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnDataModified()
    {
        float height = (view.SavedCustomPrefab.GetComponent<RectTransform>().sizeDelta.y + view.SavedDataComponents.GetComponent<VerticalLayoutGroup>().spacing) * DataTab.tabs.Count;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
