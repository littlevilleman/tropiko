using Core.Map;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPiece : IPieceMap
    {
        public event LocatePiece OnLocate;
        public event DisposePiece OnDispose;
        public bool IsPush { get; }
        public void Update(IBoard board, float time, bool push);
    }

    public enum ETokenState
    {
        Control, Located
    }

    public class Piece : PieceMap, IPiece
    {
        public event LocatePiece OnLocate;
        public event DisposePiece OnDispose;
        public bool IsPush { get; private set; }

        private ETokenState State = ETokenState.Control;
        private float Speed => IsPush ? 10F : 1f;
        public override string ToString() => $"{Location}";

        public Piece(IToken[,] tokensSetup)
        {
            Size = new Vector2Int(1, 3);
            Tokens = tokensSetup;
        }

        public void Update(IBoard board, float time, bool push)
        {
            if (State == ETokenState.Located)
                return;

            if (IsValidLocation(board, Location + Vector2Int.down))
            {
                State = ETokenState.Located;
                board.LocatePiece(this);
                return;
            }

            IsPush = push;
            State = ETokenState.Control;

            base.Update(board, time, Speed);
        }

        public override void Locate(IBoardMap board)
        {
            base.Locate(board);
            IsPush = false;
            OnLocate?.Invoke(this);
            Debug.Log("Piece - Locate - " + this);
        }

        public override void Move(IBoardMap board, bool left = false)
        {
            if (State != ETokenState.Control)
                return;
            
            base.Move(board, left);
            Debug.Log("Piece - Move - " + this);
        }

        public override void Rotate(IBoardMap board, bool left = false)
        {
            if (State != ETokenState.Control)
                return;
            
            base.Rotate(board, left);
            Debug.Log("Piece - Rotate - " + this);
        }

        public override void Dispose()
        {
            base.Dispose();
            OnDispose?.Invoke();
        }
    }
}