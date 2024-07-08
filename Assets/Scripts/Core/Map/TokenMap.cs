using System.Collections.Generic;
using UnityEngine;

namespace Core.Map
{
    public interface ITokenMap
    {
        public Vector2Int Size { get; }
        public IToken GetToken(int x, int y);
        public IToken[] GetRandomTokens(int count, List<IToken> excluding);
        public bool GetToken(int x, int y, out IToken token);
        public void Dispose();
    }

    public interface IBoardMap : ITokenMap
    {
        public void LocateToken(IToken token);
        public void TryLocatePiece(IPiece piece);
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

        public virtual void RemoveToken(Vector2Int location)
        {
            Tokens[location.x, location.y] = null;
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

        public IToken[] GetRandomTokens(int count)
        {
            return GetRandomTokens(count, new List<IToken>());
        }

        public IToken[] GetRandomTokens(int count, List<IToken> excluding)
        {
            List<IToken> tokens = GetTokens(excluding);
            IToken[] draft = new IToken[count];
            int i = 0;

            while (i < count && tokens.Count > 0)
            {
                IToken token = tokens[Random.Range(0, tokens.Count)];
                draft[i] = token;
                tokens.Remove(token);
                i++;
            }

            return draft;
        }

        protected List<IToken> GetTokens(List<IToken> excluding)
        {
            List<IToken> tokens = new List<IToken>();
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    if (GetToken(x, y, out IToken token) && !excluding.Contains(token))
                        tokens.Add(token);

            return tokens;
        }
    }
}