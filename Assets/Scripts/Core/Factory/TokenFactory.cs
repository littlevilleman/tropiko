using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, IToken tokenSetup, Vector2Int position);
        public IToken GetRandomToken<T>(IMatchConfig<T> config, int level) where T : IMatchMode;
    }

    public class TokenFactory : ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, IComboStrategy comboSetup, IBreakStrategy breakSetup, IFallStrategy fallSetup, Vector2 position)
        {
            IToken token = new Token(type, comboSetup, breakSetup, fallSetup);
            token.SetPosition(board, position, false);
            OnBuildToken?.Invoke(token, board);
            return token;
        }

        public IToken Build(IBoard board, IToken tokenSetup, Vector2Int position)
        {
            IComboStrategy combo = tokenSetup.Type == ETokenType.BOMB ? new TypeComboStrategy() : new LineComboStrategy();
            IBreakStrategy breaks = tokenSetup.Type == ETokenType.TOMB ? new CountBreakStrategy(1) : new BasicBreakStrategy();
            IFallStrategy fall = new BasicFallStrategy();
            return Build(board, tokenSetup.Type, combo, breaks, fall, position);
        }

        public IToken GetRandomToken<T>(IMatchConfig<T> config, int level) where T : IMatchMode
        {
            ITokenConfig tokenConfig = config.GetRandomToken(level);
            return new Token(tokenConfig.Type, tokenConfig.Combo, tokenConfig.Break, tokenConfig.Fall);
        }
    }
}