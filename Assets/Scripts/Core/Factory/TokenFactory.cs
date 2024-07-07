using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, IComboStrategy strategy, Vector2 position);
        public IToken Build(IBoard board, IToken tokenSetup, Vector2Int position);
        public IToken GetRandomToken<T>(IMatchConfig<T> config, int level) where T : IMatchMode;
    }

    public class TokenFactory : ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, IComboStrategy strategy, Vector2 position)
        {
            IToken token = new Token(type, strategy, position);
            OnBuildToken?.Invoke(token, board);
            return token;
        }

        public IToken Build(IBoard board, IToken tokenSetup, Vector2Int position)
        {
            IComboStrategy strategy = tokenSetup.Type == ETokenType.BOMB ? new TypeComboStrategy() : new LineComboStrategy();
            return Build(board, tokenSetup.Type, strategy, position);
        }

        public IToken GetRandomToken<T>(IMatchConfig<T> config, int level) where T : IMatchMode
        {
            ITokenConfig tokenConfig = config.GetRandomToken(level);
            return new Token(tokenConfig.Type, tokenConfig.ComboStrategy, Vector2Int.zero);
        }
    }
}