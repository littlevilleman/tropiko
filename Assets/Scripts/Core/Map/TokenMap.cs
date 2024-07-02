using UnityEngine;

namespace Core.Map
{
    public interface ITokenMap
    {
        public Vector2Int Size { get; }
        public IToken GetToken(int x, int y);
        public void Dispose();
    }

    public interface IBoardMap : ITokenMap
    {
        public void LocateToken(IToken token);
        public void LocatePiece(IPiece piece);
        public void RemoveToken(Vector2Int location);
    }

    public interface IPieceMap : ITokenMap
    {
        public Vector2 Position { get; }
        public Vector2Int Location { get; }

        public void Locate(IBoardMap board);
        public void Move(IBoardMap board, bool left);
        public void Rotate(IBoardMap board, bool left);
    }

    public abstract class TokenMap : ITokenMap
    {
        public Vector2Int Size { get; protected set; }
        protected IToken[,] Tokens;

        public IToken GetToken(int x, int y)
        {
            if (x < 0 || x >= Size.x || y < 0 || y >= Size.y)
                return null;

            return Tokens[x, y];
        }

        public virtual void Dispose()
        {
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Tokens[x, y]?.Dispose();
        }

        protected bool IsOverflowLocation(IBoardMap board, Vector2Int location)
        {
            return IsValidLocation(board, location + Vector2Int.down) && location.y >= board.Size.y - 2;
        }

        protected bool IsValidLocation(IBoardMap map, Vector2Int location)
        {
            return location.y < 0 || map.GetToken(location.x, location.y) != null;
        }
    }
}
