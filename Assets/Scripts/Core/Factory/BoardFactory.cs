using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardFactory
    {
        public event BuildBoard OnBuildBoard;
        public IPieceFactory PieceFactory { get; }
        public IBoard Build(Vector2Int size);
    }
    public class BoardFactory : IBoardFactory
    {
        public event BuildBoard OnBuildBoard;

        public IPieceFactory PieceFactory { get; private set; }

        public BoardFactory(IPieceFactory pieceFactorySetup)
        {
            PieceFactory = pieceFactorySetup;
        }

        public IBoard Build(Vector2Int size)
        {
            IBoard board = new Board(size, PieceFactory);
            OnBuildBoard?.Invoke(board);
            return board;
        }
    }
}