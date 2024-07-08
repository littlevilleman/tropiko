using Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Client
{
    public class MatchPlayerWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreLabel;

        public IPlayer player { get; private set; }
        private ScoreWidgetPool scoreWidgetPool;

        public void Display(IPlayer playerSetup, ScoreWidgetPool scoreWidgetPoolSetup)
        {
            scoreWidgetPool = scoreWidgetPoolSetup;
            player = playerSetup;
            player.OnReceiveScore += OnReceiveScore;
            player.Board.OnDispatchCombo += OnDispatchCombo;
            player.Board.OnOverflow += OnOverflow;
            player.Board.PieceHandler.OnTakePiece += OnTakePiece;

            gameObject.SetActive(true);
        }

        private void OnReceiveScore(IPlayer player, long score)
        {
            scoreLabel.text = player.Score.ToString();
        }

        private void OnDispatchCombo(List<IToken> tokens, int comboIndex)
        {
            ScoreWidget widget = scoreWidgetPool.Pull(GetComponent<RectTransform>());
            widget.onRecycle += scoreWidgetPool.Recycle;
            widget.Display(scoreLabel.transform.position + Vector3.up * 14f + Vector3.right * 56f, 150, ECombination.Combox1);
        }

        private void OnTakePiece(IPiece piece, IToken[,] nextPiece)
        {
        }

        private void OnOverflow()
        {
            GameManager.Instance.Audio.PlaySound(ESound.Defeat);
        }

        public void Hide()
        {
            if (player == null)
                return;

            player.OnReceiveScore -= OnReceiveScore;
            player.Board.OnDispatchCombo -= OnDispatchCombo;
            player.Board.OnOverflow -= OnOverflow;
            player.Board.PieceHandler.OnTakePiece -= OnTakePiece;
        }
    }
}