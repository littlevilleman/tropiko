using Core.Map;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ILocatable
    {
        public Vector2 Position { get; }
        public Vector2Int Location { get; }
        public void Locate(IBoardMap board, Vector2Int location, bool falling = false);
    }

    public interface IToken : ILocatable
    {
        public event LocateToken OnLocate;
        public event BreakToken OnBreak;
        public event DisposeToken OnDispose;

        public ETokenType Type { get; }
        public void Update(IBoard board, Vector2 position, Vector2Int direction);
        public void Fall(IBoard board);
        public void Break(IBoard board);
        public void Dispose();
    }

    public class Token : IToken
    {
        public event LocateToken OnLocate;
        public event BreakToken OnBreak;
        public event DisposeToken OnDispose;

        public ETokenType Type { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2Int Location => new Vector2Int(Mathf.CeilToInt(Position.x), Mathf.CeilToInt(Position.y));
        public override string ToString() => $"{Type} - {Location}";

        public Token(ETokenType typeSetup, Vector2 positionSetup)
        {
            Type = typeSetup;
            Position = positionSetup;
        }

        public void Update(IBoard board, Vector2 position, Vector2Int direction)
        {
            Position = ClampPosition(board, position + direction);
        }

        public void Break(IBoard board)
        {
            board.RemoveToken(Location);
            OnBreak?.Invoke(this);
            Debug.Log("Token - Break - " + this);
        }

        public void Locate(IBoardMap board, Vector2Int position, bool falling = false)
        {
            Position = position;
            board.LocateToken(this);
            OnLocate?.Invoke(this, falling);
            Debug.Log("Token - Locate - " + this);
        }

        public void Fall(IBoard board)
        {
            Vector2Int fallLocation = GetFallPosition(board, Location);
            if (Location == fallLocation)
                return;

            board.RemoveToken(Location);
            Locate(board, fallLocation, true);
            Debug.Log("Token - Fall - " + this);
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        protected Vector2Int GetFallPosition(IBoardMap board, Vector2Int location)
        {
            while (location.y > 0)
            {
                if (board.GetToken(location.x, location.y - 1) != null)
                    break;

                location += Vector2Int.down;
            }

            return location;
        }

        protected Vector2 ClampPosition(IBoardMap board, Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, 0, board.Size.x - 1);
            pos.y = Mathf.Clamp(pos.y, 0, board.Size.y + 3);

            return pos;
        }
    }

    public enum ETokenType
    {
        SKULL, WATER, LEAF, BUG, DIAMOND, CROWN
    }
}
