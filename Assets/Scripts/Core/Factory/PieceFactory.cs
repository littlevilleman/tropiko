using UnityEngine;

namespace Core
{
    public interface IPieceFactory
    {
        public ITokenFactory TokenFactory { get; }
        public IPiece Build(IBoard board, IToken[,] tokens);
        public IToken[,] GetPiecePreview(int level);
    }

    public class PieceFactory<T> : IPieceFactory where T :IMatchMode
    {
        public ITokenFactory TokenFactory { get; private set; }
        private IMatchConfig<T> Config;

        public PieceFactory(ITokenFactory tokenFactorySetup, IMatchConfig<T> configSetup)
        {
            TokenFactory = tokenFactorySetup;
            Config = configSetup;
        }

        public IPiece Build(IBoard board, IToken[,] tokens)
        {
            if(tokens == null)
                return null;

            for (int i = 0; i < tokens.Length; i++)
                tokens[0, i] = TokenFactory.Build(board, tokens[0, i], new Vector2Int(board.Size.x / 2, board.Size.y - 1 + i));

            return new Piece(tokens);
        }

        public IToken[,] GetPiecePreview(int level)
        {
            IToken[,] pieceTokens = new IToken[1, 3];

            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = TokenFactory.GetRandomToken(Config, level);

            return pieceTokens;
        }
    }
}