using Core.Factory;

namespace Core
{
    public interface IMatchBuilder
    {
        public IPlayerFactory PlayerFactory { get; }
        public IBoardFactory BoardFactory { get; }
        public IPieceFactory PieceFactory { get; }
        public ITokenFactory TokenFactory { get; }

        public IPlayer BuildPlayer(IMatchConfig config, string name);
        public IPiece BuildPiece(IBoard board, IToken[,] nextPiecePreview);
        public IToken[,] GetPiecePreview(IMatchConfig config, int level);
    }

    public class MatchBuilder : IMatchBuilder
    {
        public IPlayerFactory PlayerFactory { get; private set; } = new PlayerFactory();
        public IBoardFactory BoardFactory { get; private set; } = new BoardFactory();
        public IPieceFactory PieceFactory { get; private set; } = new PieceFactory();
        public ITokenFactory TokenFactory { get; private set; } = new TokenFactory();

        public MatchBuilder(IMatchBuilderDispatcher dispatcher)
        {
            BoardFactory.OnBuildBoard += dispatcher.OnBuildBoard;
            TokenFactory.OnBuildToken += dispatcher.OnBuildToken;
            PlayerFactory.OnBuildPlayer += dispatcher.OnBuildPlayer;
        }

        public IPlayer BuildPlayer(IMatchConfig config, string name)
        {
            return PlayerFactory.Build(name, BoardFactory.Build(config.BoardSize));
        }

        public IToken[,] GetPiecePreview(IMatchConfig config, int level)
        {
            return PieceFactory.GetPiecePreview(config, level, TokenFactory);
        }

        public IPiece BuildPiece(IBoard board, IToken[,] piecePreview)
        {
            return PieceFactory.Build(board, piecePreview, TokenFactory);
        }
    }
}