using UnityEngine;

namespace Core.Factory
{
    public interface IPieceFactory
    {
        public IPiece Build(IBoard board, IToken[,] tokens, ITokenFactory tokenFactory);
        public IToken[,] GetPiecePreview(IMatchConfig config, int level, ITokenFactory tokenFactory);
    }

    public class PieceFactory : IPieceFactory
    {
        public IPiece Build(IBoard board, IToken[,] tokens, ITokenFactory tokenFactory)
        {
            if(tokens == null)
                return null;

            for (int i = 0; i < tokens.Length; i++)
                tokens[0, i] = tokenFactory.Build(board, tokens[0, i], new Vector2Int(board.Size.x / 2, board.Size.y - 1 + i));

            return new Piece(tokens);
        }

        public IToken[,] GetPiecePreview(IMatchConfig config, int level, ITokenFactory tokenFactory)
        {
            IToken[,] pieceTokens = new IToken[1, 3];

            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = tokenFactory.BuildTokenPreview(config.GenerateToken(level));

            return pieceTokens;
        }
    }
}