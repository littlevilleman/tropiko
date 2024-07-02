
namespace Core
{
    public interface IPieceFactory
    {
        //public event BuildPiece OnBuildPiece;

        public ITokenFactory TokenFactory { get; }
        public IPiece Build(IBoard board, IToken[,] tokens);
        public IToken[,] GetPiecePreview(IBoard board);
    }

    public class PieceFactory : IPieceFactory
    {
        //public event BuildPiece OnBuildPiece;

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
                tokens[0, i] = TokenFactory.Build(board, tokens[0, i]);

            //OnBuildPiece?.Invoke(piece, board);
            return new Piece(tokens);
        }

        public IToken[,] GetPiecePreview(IBoard board)
        {
            IToken[,] pieceTokens = new IToken[1, 3];
        
            for (int x = 0; x < 1; x++)
                for (int y = 0; y < 3; y++)
                    pieceTokens[x, y] = TokenFactory.GetRandomToken(board, y);

            return pieceTokens;
        }
    }
}