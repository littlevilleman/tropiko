using Core;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public class BoardBehavior : MonoBehaviour, IPoolable<BoardBehavior>
    {
        [SerializeField] private PiecePreviewBehavior nextPieceBhv;
        [SerializeField] private PieceBehavior currentPieceBhv;

        public IBoard Board { get; private set; }

        public event RecyclePoolable<BoardBehavior> onRecycle;
        private PiecePool piecePool;

        public void Setup(IBoard boardSetup, PiecePool piecePoolSetup)
        {
            piecePool = piecePoolSetup;
            Board = boardSetup;
            Board.OnOverflow += OnDefeat;
            Board.OnTakePiece += OnTakePiece;

            gameObject.SetActive(true);
        }

        private void OnTakePiece(IPiece piece, IToken[,] nextPiece)
        {
            //PieceBehavior pieceBhv = piecePool.Pull(transform);
            //pieceBhv.Setup(piece);
            //pieceBhv.onRecycle += piecePool.Recycle;
            //
            currentPieceBhv.Setup(piece);
            nextPieceBhv.Setup(nextPiece);
        }

        private void Update()
        {
            Board.Update(Time.deltaTime, GetPushFactor());

            if (Input.GetKeyDown(KeyCode.E))
                Board.RotatePiece(false);

            if (Input.GetKeyDown(KeyCode.Q))
                Board.RotatePiece(true);

            if (Input.GetKeyDown(KeyCode.A))
                Board.MovePiece(true);

            if (Input.GetKeyDown(KeyCode.D))
                Board.MovePiece(false);
        }

        private bool GetPushFactor()
        {
            return Input.GetKey(KeyCode.S);
        }

        private void OnOverflow()
        {
              
        }

        private void OnDefeat()
        {
            //isDefeat = true;
            Board.OnOverflow -= OnDefeat;
            Board.OnTakePiece -= OnTakePiece;
            onRecycle?.Invoke(this);
            //Board = null;
        }
    }

}