using UnityEngine;

namespace Core.Map
{
    public interface ITokenMap
    {
        public Vector2Int Size { get; }
        public IToken GetToken(int x, int y);
        public bool GetToken(int x, int y, out IToken token);
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

        public bool GetToken(int x, int y, out IToken token)
        {
            token = GetToken(x, y);
            return token != null;
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
    }
}