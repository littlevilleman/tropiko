using Core;
using TMPro;
using UnityEngine;

namespace Client
{
    public class ArcadeMatchScreen : UIScreen
    {
        private IArcadeMatch match;
        [SerializeField] private TMP_Text levelLabel;

        protected override void OnDisplay(params object[] parameters)
        {
            match = parameters[0] as IArcadeMatch;
            match.OnLevelUp += OnLevelUp;

            levelLabel.text = $"Level {match.CurrentLevel}";
        }

        private void OnLevelUp(int level)
        {
            levelLabel.text = $"Level {level}";
        }

        protected override void OnClose()
        {

        }
    }
}
