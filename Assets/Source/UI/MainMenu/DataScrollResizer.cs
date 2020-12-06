using Racing.UI;
using Racing.UI.MainMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float height = (view.SavedCustomPrefab.GetComponent<RectTransform>().sizeDelta.y + 20f) * DataTab.tabs.Count;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
