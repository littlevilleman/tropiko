using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class ArcadeMatchScreen : UIScreen
    {
        private IArcadeMatch match;
        [SerializeField] private TMP_Text levelLabel;
        [SerializeField] private Image levelProgressBar;
        [SerializeField] private ScoreWidgetPool scorePool;
        [SerializeField] private Transform scorePanel;

        protected override void OnDisplay(params object[] parameters)
        {
            match = parameters[0] as IArcadeMatch;
            match.Players[0].OnReceiveScore += OnReceiveScore;
            match.Players[0].OnLevelUp += OnLevelUp;

            levelLabel.text = $"Level {match.Level}";
        }

        private void OnReceiveScore(IPlayer player, long score)
        {
            Debug.Log("SCORE " + player.Score);
            levelProgressBar.fillAmount = match.Progress;
            ScoreWidget scoreWidget = scorePool.Pull(scorePanel);
            scoreWidget.Display(Vector3.zero, score);
        }

        private void OnLevelUp(int level)
        {
            levelLabel.text = $"Level {level}";
        }

        protected override void OnClose()
        {
            match.Players[0].OnLevelUp -= OnLevelUp;
            match = null;
        }
    }
}
