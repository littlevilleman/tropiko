using System;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, IComboStrategy strategy, Vector2 position);
        public IToken Build(IBoard board, IToken tokenSetup);
        public IToken GetRandomToken(IBoard board, Vector2 position);
        public IToken GetRandomToken(IMatchConfig<IArcadeMatch> config, int level, Vector2 position);
    }

    public class TokenFactory : ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, IComboStrategy strategy, Vector2 position)
        {
            IToken token = new Token(type, position, strategy);
            OnBuildToken?.Invoke(token, board);
            return token;
        }

        public IToken Build(IBoard board, IToken tokenSetup)
        {
            IComboStrategy strategy = tokenSetup.Type == ETokenType.BOMB ? new TypeComboStrategy() : new LineComboStrategy();
            return Build(board, tokenSetup.Type, strategy, tokenSetup.Position);
        }

        public IToken GetRandomToken(IBoard board, Vector2 position)
        {
            return new Token(GetRandomType(), position, new LineComboStrategy());
        }

        public IToken GetRandomToken(IMatchConfig<IArcadeMatch> config, int level, Vector2 position)
        {
            ITokenConfig tokenConfig = config.GetRandomToken(level);
            Debug.Log("STRATEGY " + tokenConfig.GetComboStrategy());
            return new Token(tokenConfig.Type, position, tokenConfig.GetComboStrategy());
        }

        private ETokenType GetRandomType()
        {
            Array values = Enum.GetValues(typeof(ETokenType));
            System.Random random = new System.Random();
            return (ETokenType)values.GetValue(random.Next(values.Length));
        }
    }
}
