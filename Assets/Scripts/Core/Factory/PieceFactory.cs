
using UnityEngine;

namespace Core
{
    public interface IPieceFactory
    {
        public IMatchConfig<IArcadeMatch> Config { get; set; }

        public ITokenFactory TokenFactory { get; }
        public IPiece Build(IBoard board, IToken[,] tokens);
        public IToken[,] GetPiecePreview(IBoard board);
        public IToken[,] GetPiecePreview(int level);
    }

    public class PieceFactory : IPieceFactory
    {
        public ITokenFactory TokenFactory { get; private set; }
        public IMatchConfig<IArcadeMatch> Config { get; set; }

        public PieceFactory(ITokenFactory tokenFactorySetup)
        {
            TokenFactory = tokenFactorySetup;
        }

        public IPiece Build(IBoard board, IToken[,] tokens)
        {
            if(tokens == null)
                return null;

            for (int i = 0; i < tokens.Length; i++)
                tokens[0, i] = TokenFactory.Build(board, tokens[0, i]);

            return new Piece(tokens);
        }

        public IToken[,] GetPiecePreview(IBoard board)
        {
            IToken[,] pieceTokens = new IToken[1, 3];

            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = TokenFactory.GetRandomToken(board, new Vector2Int(2, 12) + y * Vector2Int.up);

            return pieceTokens;
        }

        public IToken[,] GetPiecePreview(int level)
        {
            IToken[,] pieceTokens = new IToken[1, 3];

            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = TokenFactory.GetRandomToken(Config, level, new Vector2Int(2, 12) + y * Vector2Int.up);

            return pieceTokens;
        }
    }
}