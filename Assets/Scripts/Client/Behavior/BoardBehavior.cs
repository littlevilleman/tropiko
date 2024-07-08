using Core;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public class BoardBehavior : MonoBehaviour, IPoolable<BoardBehavior>
    {
        [SerializeField] private PiecePreviewBehavior nextPieceBhv;
        [SerializeField] private PieceBehavior currentPieceBhv;

        [SerializeField] private SpriteRenderer background;
        [SerializeField] private SpriteRenderer boardBorder01;
        [SerializeField] private SpriteRenderer boardBorder02;
        [SerializeField] private SpriteRenderer boardBorder03;
        [SerializeField] private SpriteRenderer grid;

        public IBoard Board { get; private set; }

        public event RecyclePoolable<BoardBehavior> onRecycle;

        public void Setup(IBoard boardSetup)
        {
            Board = boardSetup;
            Board.OnOverflow += OnOverflow;
            Board.PieceHandler.OnTakePiece += OnTakePiece;
            Board.OnDispatchCombo += OnDispatchCombo;

            grid.size = background.size = Board.Size;
            boardBorder01.size = boardBorder02.size = boardBorder03.size = Board.Size + Vector2Int.one * 2;
            nextPieceBhv.transform.localPosition = new Vector3(Board.Size.x + 2f, Board.Size.y - 2, 0);

            grid.DOColor(AnimationUtils.GetFadeColor(grid.color, .75f), 2f).SetLoops(-1, LoopType.Yoyo);

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Rotate(false);

            if (Input.GetKeyDown(KeyCode.Q))
                Rotate(true);

            if (Input.GetKeyDown(KeyCode.A))
                Board.PieceHandler.MovePiece(Board, true);

            if (Input.GetKeyDown(KeyCode.D))
                Board.PieceHandler.MovePiece(Board, false);

            Board.PieceHandler.PushPiece(Input.GetKey(KeyCode.S));
        }

        private void OnDispatchCombo(List<IToken> token, int comboIndex)
        {
            GameManager.Instance.Audio.PlaySound(ESound.Combo_01 + Mathf.Clamp(comboIndex, 0, 3));
            GameManager.Instance.Camera.Shake(comboIndex);
            Color color = GameManager.Instance.Config.GetTokenConfig(token[0].Type).color;
            boardBorder01.DOColor(color, .125f).SetLoops(2, LoopType.Yoyo);
        }

        private void OnTakePiece(IPiece piece, IToken[,] nextPiece)
        {
            GameManager.Instance.Audio.PlaySound(ESound.GeneratePiece);
            currentPieceBhv.Setup(piece);
            nextPieceBhv.Setup(nextPiece);
        }

        private void Rotate(bool left)
        {
            Board.PieceHandler.RotatePiece(Board, left);
            GameManager.Instance.Audio.PlaySound(ESound.RotatePiece);
        }

        private void OnOverflow()
        {
            Board.OnOverflow -= OnOverflow;
            Board.PieceHandler.OnTakePiece -= OnTakePiece;
            Board.OnDispatchCombo -= OnDispatchCombo;
            Board = null;
            onRecycle?.Invoke(this);
        }
    }

}