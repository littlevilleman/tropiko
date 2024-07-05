namespace Core
{
    public interface IMatchConfig<T> where T : IMatch
    {
        public int GetLevel(long score);
        public ITokenConfig GetRandomToken(int level);
        public float GetLevelSpeed(int level);
    }

    public interface IMatchFactory<T> where T : IMatch
    {
        public T Build(IMatchConfig<T> config);
    }

    public class ArcadeMatchFactory : IMatchFactory<IArcadeMatch>
    {
        private IPlayerFactory playerFactory;
        private IBoardFactory boardFactory;
        private ITokenFactory tokenFactory;

        public ArcadeMatchFactory(IMatchBuilder builderDispatcher)
        {
            tokenFactory = new TokenFactory();
            boardFactory = new BoardFactory(new PieceFactory(tokenFactory));
            playerFactory = new PlayerFactory(boardFactory);

            playerFactory.OnBuildPlayer += builderDispatcher.OnBuildPlayer;
            boardFactory.OnBuildBoard += builderDispatcher.OnBuildBoard;
            tokenFactory.OnBuildToken += builderDispatcher.OnBuildToken;
        }

        public IArcadeMatch Build(IMatchConfig<IArcadeMatch> config)
        {
            boardFactory.pieceFactory.Config = config;
            return new ArcadeMatch(config, playerFactory.Build("Player01"));
        }
    }
}