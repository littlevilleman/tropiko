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
            return location.y >= board.Size.y - 2;
        }

        public static bool IsColisionLocation(IBoardMap map, Vector2Int location)
        {
            return location.y < 0 || map.GetToken(location.x, location.y) != null;
        }

        public static Vector2 ClampPosition(IBoardMap board, Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, 0, board.Size.x - 1);
            pos.y = Mathf.Clamp(pos.y, 0, board.Size.y + 2);

            return pos;
        }
    }
}
