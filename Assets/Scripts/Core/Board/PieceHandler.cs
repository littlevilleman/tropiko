using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPieceHandler
    {
        public event TakePiece OnTakePiece;

        public bool TryUpdate(IBoard board, float time, float speed, int level, float collisionTime);
        public void MovePiece(IBoard board, bool left = false);
        public void RotatePiece(IBoard board, bool left = false);
        public void PushPiece(bool push);
        public void SwitchPiece(IBoard board, int level);
        public void LocatePiece(IBoard board);
        public void Dispose();
    }

    public class PieceHandler : IPieceHandler
    {
        public event TakePiece OnTakePiece;

        private IPieceFactory pieceFactory;
        private IPiece currentPiece;
        private IToken[,] nextPiecePreview;

        public PieceHandler(IPieceFactory factorySetup)
        {
            pieceFactory = factorySetup;
            nextPiecePreview = pieceFactory.GetPiecePreview(0);
        }

        public bool TryUpdate(IBoard board, float time, float speed, int level, float collisionTime)
        {
            if (currentPiece != null)
            {
                currentPiece.Update(board, time, speed, collisionTime);
                return true;
            }

            return false;
        }

        public void MovePiece(IBoard board, bool left = false) 
        {
            currentPiece?.Move(board, left);
            Debug.Log($"Piece - Move - {currentPiece}");
        }

        public void RotatePiece(IBoard board, bool left = false)
        {
            currentPiece?.Rotate(board, left);
            Debug.Log($"Piece - Rotate - {currentPiece}");
        }

        public void PushPiece(bool push)
        {
            currentPiece?.Push(push);
            Debug.Log($"Piece - Push - {currentPiece}");
        }

        public void SwitchPiece(IBoard board, int level)
        {
            currentPiece = pieceFactory.Build(board, nextPiecePreview);
            nextPiecePreview = pieceFactory.GetPiecePreview(level);
            OnTakePiece?.Invoke(currentPiece, nextPiecePreview);
            Debug.Log($"Piece - Switch - {currentPiece} - {nextPiecePreview}");
        }

        public void LocatePiece(IBoard board)
        {
            currentPiece.Locate(board);
            currentPiece = null;
            Debug.Log("Piece - Locate - " + this);
        }

        public void Dispose()
        {
            currentPiece.Dispose();
        }
    }

}