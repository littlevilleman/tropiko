using Core.Map;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ILocatable
    {
        public Vector2 Position { get; }
        public Vector2Int Location { get; }
        public void Locate(IBoardMap board, bool falling = false);
    }

    public interface ITokenConfig
    {
        public ETokenType Type { get; }
        public IComboStrategy GetComboStrategy();
    }

    public interface IToken : ILocatable
    {
        public event LocateToken OnLocate;
        public event BreakToken OnBreak;
        public event DisposeToken OnDispose;
        public ETokenType Type { get; }
        public void Update(IBoardMap board, Vector2 position);
        public void Fall(IBoardMap board);
        public void Break(IBoardMap board);
        public ComboResultContext Perform(IBoardMap board);
        public void Dispose();
    }

    public class Token : IToken
    {
        public event LocateToken OnLocate;
        public event BreakToken OnBreak;
        public event DisposeToken OnDispose;

        public ETokenType Type { get; protected set; }
        public Vector2 Position { get; protected set; }
        public Vector2Int Location => new Vector2Int(Mathf.CeilToInt(Position.x), Mathf.CeilToInt(Position.y));
        public override string ToString() => $"{Type} - {Location}";
        private IComboStrategy comboStrategy;

        public Token(ETokenType typeSetup, Vector2 positionSetup, IComboStrategy comboStrategySetup)
        {
            Type = typeSetup;
            Position = positionSetup;
            comboStrategy = comboStrategySetup;
        }

        public void Update(IBoardMap board, Vector2 position)
        {
            Position = TokenMap.ClampPosition(board, position);
        }

        public void Locate(IBoardMap board, bool falling = false)
        {
            Update(board, falling ? GetFallLocation(board) : Location);
            board.LocateToken(this);
            OnLocate?.Invoke(this, falling);
            Debug.Log("Token - Locate - " + this);
        }

        public ComboResultContext Perform(IBoardMap board)
        {
            return comboStrategy.Perform(this, board);
        }

        public void Break(IBoardMap board)
        {
            board.RemoveToken(Location);
            OnBreak?.Invoke(this);
            Debug.Log("Token - Break - " + this);
        }

        public void Fall(IBoardMap board)
        {
            board.RemoveToken(Location);
            Locate(board, true);
            Debug.Log("Token - Fall - " + this);
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }

        protected Vector2Int GetFallLocation(IBoardMap board)
        {
            Vector2Int fallLocation = Location;
            while (fallLocation.y > 0)
            {
                if (board.GetToken(fallLocation.x, fallLocation.y - 1) != null)
                    break;

                fallLocation += Vector2Int.down;
            }

            return fallLocation;
        }
    }

    public enum ETokenType
    {
        SKULL, WATER, LEAF, BUG, DIAMOND, CROWN, BOMB
    }
}
