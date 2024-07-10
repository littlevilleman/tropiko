using Core.Map;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ILocatable
    {
        public Vector2 Position { get; }
        public Vector2Int Location { get; }
        public void Locate(IBoardMap board);
    }

    public interface ITokenConfig
    {
        public ETokenType Type { get; }
        public IComboStrategy Combo { get; }
        public IBreakStrategy Break { get; }
        public IFallStrategy Fall { get; }
    }

    public interface IToken : ILocatable
    {
        public event LocateToken OnLocate;
        public event BreakToken OnBreak;
        public event FallToken OnFall;
        public event EntombToken OnEntomb;
        public event DisposeToken OnDispose;
        public ETokenType Type { get; }

        public void SetPosition(IBoardMap board, Vector2 position, bool clamp = true);
        public void Fall(IBoardMap board);
        public void Break(IBoardMap board);
        public void Entomb(IBoardMap board, int count);
        public void Untomb(IBoardMap board);
        public ComboResultContext GetCombo(IBoardMap board);
        public void Dispose();
        public string ToString() => $"{Type} - {Location}";
    }

    public class Token : IToken
    {
        public event LocateToken OnLocate;
        public event FallToken OnFall; 
        public event BreakToken OnBreak;
        public event EntombToken OnEntomb;
        public event DisposeToken OnDispose;

        public ETokenType Type => breakStrategy is CountBreakStrategy ? ETokenType.TOMB : type;
        public Vector2 Position { get; protected set; }
        public Vector2Int Location => new Vector2Int(Mathf.CeilToInt(Position.x), Mathf.CeilToInt(Position.y));

        private ETokenType type;
        private IComboStrategy comboStrategy;
        private IBreakStrategy breakStrategy;
        private IFallStrategy fallStrategy;

        public Token(ETokenType typeSetup, IComboStrategy comboStrategySetup, IBreakStrategy breakStrategySetup, IFallStrategy fallStrategySetup)
        {
            type = typeSetup;
            comboStrategy = comboStrategySetup;
            breakStrategy = breakStrategySetup;
            fallStrategy = fallStrategySetup;
        }

        public void SetPosition(IBoardMap board, Vector2 position, bool clamp = true)
        {
            Position = clamp ? MapUtils.ClampPosition(board, position) : position;
        }

        public void Locate(IBoardMap board)
        {
            board.LocateToken(this);
            OnLocate?.Invoke(this);
            Debug.Log("Token - Locate - " + this);
        }

        public void Break(IBoardMap board)
        {
            breakStrategy.Break(board, this);
            OnBreak?.Invoke(this, breakStrategy.Counter);
            Debug.Log("Token - Break - " + this);
        }

        public void Fall(IBoardMap board)
        {
            fallStrategy.Fall(board, this);
            OnFall?.Invoke(this);
            Debug.Log("Token - Fall - " + this);
        }

        public void Entomb(IBoardMap board, int count)
        {
            comboStrategy = new NonComboStrategy();
            breakStrategy = new CountBreakStrategy(count);
            OnEntomb?.Invoke(board, this, count);
            Debug.Log("Token - Entomb - " + this);
        }

        public void Untomb(IBoardMap board)
        {
            comboStrategy = new LineComboStrategy();
            breakStrategy = new BasicBreakStrategy();
            Debug.Log("Token - Untomb - " + this);
        }

        public ComboResultContext GetCombo(IBoardMap board)
        {
            return comboStrategy.Perform(this, board);
        }

        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }

    public enum ETokenType
    {
        SKULL, WATER, LEAF, BUG, DIAMOND, CROWN, BOMB, TOMB, NONE
    }
}