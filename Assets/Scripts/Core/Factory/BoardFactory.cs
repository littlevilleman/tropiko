using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoardFactory
    {
        public event BuildBoard OnBuildBoard;
        public IBoard Build(Vector2Int size);
    }
    public class BoardFactory : IBoardFactory
    {
        public event BuildBoard OnBuildBoard;


        public BoardFactory()
        {
        }

        public IBoard Build(Vector2Int size)
        {
            IBoard board = new Board(size, new PieceHandler());
            OnBuildBoard?.Invoke(board);
            return board;
        }
    }
}