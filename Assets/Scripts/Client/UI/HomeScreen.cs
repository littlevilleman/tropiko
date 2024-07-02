using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class HomeScreen : UIScreen
    {
        [SerializeField] private Button singlePlayerButton;

        protected override void OnDisplay(params object[] parameters)
        {
            singlePlayerButton.onClick.AddListener(OnClickSinglePlayerButton);
        }

        private void OnClickSinglePlayerButton()
        {
            GameManager.Instance.LaunchMatch(EMatchMode.Single);
        }

        protected override void OnClose()
        {
            singlePlayerButton.onClick.RemoveListener(OnClickSinglePlayerButton);
        }
    }
}