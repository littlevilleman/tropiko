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
        [SerializeField] private SpriteRenderer boardBorder;
        [SerializeField] private SpriteRenderer grid;

        public IBoard Board { get; private set; }

        public event RecyclePoolable<BoardBehavior> onRecycle;
        //private PiecePool piecePool;

        public void Setup(IBoard boardSetup/*, PiecePool piecePoolSetup*/)
        {
            //piecePool = piecePoolSetup;
            Board = boardSetup;
            Board.OnOverflow += OnOverflow;
            Board.OnTakePiece += OnTakePiece;
            Board.ComboDispatcher.OnDispatch += OnDispatchCombo;

            grid.DOColor(AnimationUtils.GetFadeColor(grid.color, .75f), 2f).SetLoops(-1, LoopType.Yoyo);

            gameObject.SetActive(true);
        }

        private void OnDispatchCombo(List<IToken> token, int comboIndex)
        {
            GameManager.Instance.Audio.PlaySound(ESound.Combo_01 + Mathf.Clamp(comboIndex, 0, 3));
            GameManager.Instance.Camera.Shake();
            Color color = GameManager.Instance.Config.GetTokenConfig(token[0].Type).color;
            boardBorder.DOColor(color, .125f).SetLoops(2, LoopType.Yoyo);

        }

        private void OnTakePiece(IPiece piece, IToken[,] nextPiece)
        {
            GameManager.Instance.Audio.PlaySound(ESound.GeneratePiece);
            currentPieceBhv.Setup(piece);
            nextPieceBhv.Setup(nextPiece);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Rotate(false);

            if (Input.GetKeyDown(KeyCode.Q))
                Rotate(true);

            if (Input.GetKeyDown(KeyCode.A))
                Board.MovePiece(true);

            if (Input.GetKeyDown(KeyCode.D))
                Board.MovePiece(false);

            Board.PushPiece(Input.GetKey(KeyCode.S));
        }

        private void Rotate(bool left)
        {
            Board.RotatePiece(left);
            GameManager.Instance.Audio.PlaySound(ESound.RotatePiece);
        }

        private void OnOverflow()
        {
            Board.OnOverflow -= OnOverflow;
            Board.OnTakePiece -= OnTakePiece;
            Board.ComboDispatcher.OnDispatch -= OnDispatchCombo;
            Board = null;
            onRecycle?.Invoke(this);
        }
    }

}