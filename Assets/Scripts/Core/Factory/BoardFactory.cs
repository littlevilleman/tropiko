using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardFactory
    {
        public event BuildBoard OnBuildBoard;
        public IPieceFactory pieceFactory { get; }
        public IBoard Build();
    }
    public class BoardFactory : IBoardFactory
    {
        public event BuildBoard OnBuildBoard;

        public IPieceFactory pieceFactory { get; private set; }

        private Vector2Int boardSizeSetup = new Vector2Int(6, 13);

        public BoardFactory(IPieceFactory pieceFactorySetup)
        {
            pieceFactory = pieceFactorySetup;
        }

        public IBoard Build()
        {
            IBoard board = new Board(boardSizeSetup, pieceFactory);
            OnBuildBoard?.Invoke(board);
            return board;
        }
    }
}