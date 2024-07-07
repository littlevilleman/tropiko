using Core.Map;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPiece : IPieceMap
    {
        public event CollidePiece OnCollide;
        public event LocatePiece OnLocate;
        public event DisposePiece OnDispose;
        public float CollisionTime { get; }
        public void Update(IBoard board, float time, float speed = 1f, float collisionTime = .5f);
    }

    public enum EPieceState
    {
        Control, Collide, Located
    }

    public class Piece : PieceMap, IPiece
    {
        public event CollidePiece OnCollide;
        public event LocatePiece OnLocate;
        public event DisposePiece OnDispose;
        public float CollisionTime { get; private set; } = .5f;
        private float Speed = 1f;
        private EPieceState State = EPieceState.Control;

        public Piece(IToken[,] tokensSetup)
        {
            Size = new Vector2Int(1, 3);
            Tokens = tokensSetup;
        }

        public override void Update(IBoard board, float time, float speed = 1f, float collisionTime = .5f)
        {
            if (State == EPieceState.Located)
                return;

            if (Collide(board))
                return;

            Speed = speed;
            CollisionTime = collisionTime;
            State = EPieceState.Control;
            base.Update(board, time, IsPush ? 20F : Speed);
        }

        private bool Collide(IBoard board)
        {
            if (MapUtils.IsColisionLocation(board, Location + Vector2Int.down))
            {
                if (State == EPieceState.Control)
                    OnCollide?.Invoke(this);

                if (CollisionTime <= 0f)
                    board.LocatePiece(this);

                CollisionTime -= Time.deltaTime;
                State = EPieceState.Collide;

                return true;
            }

            return false;
        }

        public override void Locate(IBoardMap board, bool falling = false)
        {
            base.Locate(board, falling);
            State = EPieceState.Located;
            OnLocate?.Invoke(this);
            Debug.Log("Piece - Locate - " + this);
        }

        public override void Move(IBoardMap board, bool left = false)
        {
            if (State == EPieceState.Located)
                return;

            base.Move(board, left);
            Debug.Log("Piece - Move - " + this);
        }

        public override void Rotate(IBoardMap board, bool left = false)
        {
            if (State == EPieceState.Located)
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