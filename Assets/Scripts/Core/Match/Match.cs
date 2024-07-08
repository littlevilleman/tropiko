using UnityEngine;
using static Core.Events;

namespace Core
{
    public enum EMatchMode
    {
        Single, Multiplayer
    }

    public interface IMatchBuilder
    {
        public BuildPlayer OnBuildPlayer { get; }
        public BuildBoard OnBuildBoard { get; }
        public BuildToken OnBuildToken { get; }
    }

    public interface IMatchConfig<out T> where T : IMatchMode
    {
        public Vector2Int BoardSize { get; }
        public ITokenConfig GetRandomToken(int level);
        public int GetLevel(long score);
        public float GetLevelSpeed(int level);
    }

    public interface IArcadeMatchConfig : IMatchConfig<IArcadeMatchMode>
    {

    }

    public interface IMatch
    {
        public event LaunchMatch OnLaunch;
        public event PauseMatch OnPause;
        public event CloseMatch OnClose;

        public IPlayer[] Players { get; }
        public void Launch();
        public void Update(float deltaTime);
        public void Pause(bool pause = true);
        public void Quit();
    }

    public abstract class Match<T> : IMatch where T : IMatchMode
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;
        public event PauseMatch OnPause;
        public IMatchConfig<T> Config { get; protected set; }
        public IPlayer[] Players { get; protected set; }

        protected IPlayerFactory playerFactory;
        protected IBoardFactory boardFactory;
        protected IPieceFactory pieceFactory;
        protected ITokenFactory tokenFactory;

        protected abstract void OnReceiveScore(IPlayer player, long score);
        protected abstract void OnDefeatPlayer(IPlayer player);
        protected abstract MatchContext<T> GetContext(float deltaTime);

        public Match(IMatchBuilder builder, IMatchConfig<T> configSetup)
        {
            Config = configSetup;
            tokenFactory = new TokenFactory();
            pieceFactory = new PieceFactory(tokenFactory);
            boardFactory = new BoardFactory();
            playerFactory = new PlayerFactory();

            boardFactory.OnBuildBoard += builder.OnBuildBoard;
            tokenFactory.OnBuildToken += builder.OnBuildToken;
            playerFactory.OnBuildPlayer += builder.OnBuildPlayer;
        }

        public void Launch()
        {
            OnLaunch?.Invoke(this);
        }

        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < Players.Length; i++)
                Players[i].Board.Update(GetContext(deltaTime));
        }

        public void Pause(bool pause = true)
        {
        }

        public void Quit()
        {
            OnClose?.Invoke();
        }
    }
}
