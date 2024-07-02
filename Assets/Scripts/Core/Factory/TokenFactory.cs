using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using static Core.Events;

namespace Core
{
    public interface ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, Vector2 position);
        public IToken Build(IBoard board, IToken tokenSetup);
        public IToken GetRandomToken(IBoard board, int i);
    }

    public class TokenFactory : ITokenFactory
    {
        public event BuildToken OnBuildToken;

        public IToken Build(IBoard board, ETokenType type, Vector2 position)
        {
            IToken token = new Token(type, position);
            OnBuildToken?.Invoke(token, board);
            return token;
        }

        public IToken Build(IBoard board, IToken tokenSetup)
        {
            return Build(board, tokenSetup.Type, tokenSetup.Position);
        }

        public IToken GetRandomToken(IBoard board, int i)
        {
            return new Token(GetRandomType(), new Vector2Int(2, 12) + i * Vector2Int.up);
        }

        private ETokenType GetRandomType()
        {
            Array values = Enum.GetValues(typeof(ETokenType));
            System.Random random = new System.Random();
            return (ETokenType)values.GetValue(random.Next(values.Length));
        }
    }
}
