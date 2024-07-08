using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPieceHandler
    {
        public event TakePiece OnTakePiece;

        public bool TryUpdate<T>(IBoard board, MatchContext<T> context) where T : IMatchMode;
        public void MovePiece(IBoard board, bool left = false);
        public void RotatePiece(IBoard board, bool left = false);
        public void PushPiece(bool push);
        public void SwitchPiece<T>(IBoard board, MatchContext<T> context) where T : IMatchMode;
        public void LocatePiece(IBoard board);
        public void Dispose();
    }

    public class PieceHandler : IPieceHandler
    {
        public event TakePiece OnTakePiece;

        private IPiece currentPiece;
        private IToken[,] nextPiecePreview;

        public bool TryUpdate<T>(IBoard board, MatchContext<T> context) where T : IMatchMode
        {
            if (currentPiece != null)
            {
                currentPiece.Update(board, context);
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

        public void SwitchPiece<T>(IBoard board, MatchContext<T> context) where T : IMatchMode
        {
            currentPiece = context.pieceFactory.Build(board, nextPiecePreview);
            nextPiecePreview = context.pieceFactory.GetPiecePreview(context.config, context.level);

            if(currentPiece != null)
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