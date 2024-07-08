using UnityEngine;

namespace Core
{
    public interface IPieceFactory
    {
        public ITokenFactory TokenFactory { get; }
        public IPiece Build(IBoard board, IToken[,] tokens);
        public IToken[,] GetPiecePreview<T>(IMatchConfig<T> config, int level) where T : IMatchMode;
    }

    public class PieceFactory : IPieceFactory
    {
        public ITokenFactory TokenFactory { get; private set; }

        public PieceFactory(ITokenFactory tokenFactorySetup)
        {
            TokenFactory = tokenFactorySetup;
        }

        public IPiece Build(IBoard board, IToken[,] tokens)
        {
            if(tokens == null)
                return null;

            for (int i = 0; i < tokens.Length; i++)
                tokens[0, i] = TokenFactory.Build(board, tokens[0, i], new Vector2Int(board.Size.x / 2, board.Size.y - 1 + i));

            return new Piece(tokens);
        }

        public IToken[,] GetPiecePreview<T>(IMatchConfig<T> config, int level) where T :IMatchMode
        {
            IToken[,] pieceTokens = new IToken[1, 3];

            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = TokenFactory.GetRandomToken(config, level);

            return pieceTokens;
        }
    }
}