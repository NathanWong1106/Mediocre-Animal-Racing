using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Racing.UI.InGame
{
    [RequireComponent(typeof(InGameUIController))]
    public class InGameUIView : MonoBehaviour, IUserInterfaceView
    {
        //Set in Unity Editor
        public TextMeshProUGUI PositionIndicator;
        public TextMeshProUGUI LapIndicator;
        public TextMeshProUGUI CountdownTimer;

        private void Start()
        {
            UserInterface.CurrentView = this;
        }
    }
}
