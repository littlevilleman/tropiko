using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class HomeScreen : UIScreen
    {
        [SerializeField] private Button singlePlayerButton;
        [SerializeField] private Button multiPlayerButton;

        protected override void OnDisplay(params object[] parameters)
        {
            singlePlayerButton.onClick.AddListener(OnClickSinglePlayerButton);
            multiPlayerButton.onClick.AddListener(OnClickMultiPlayerButton);
        }

        private void OnClickMultiPlayerButton()
        {
            GameManager.Instance.Match.Launch<IMultiplayerMatchMode>();
        }

        private void OnClickSinglePlayerButton()
        {
            GameManager.Instance.Match.Launch<IArcadeMatchMode>();
        }

        protected override void OnClose()
        {
            singlePlayerButton.onClick.RemoveListener(OnClickSinglePlayerButton);
            multiPlayerButton.onClick.RemoveListener(OnClickMultiPlayerButton);
        }
    }
}