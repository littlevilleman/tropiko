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

    public interface IMatchConfig<in T> where T : IMatchMode
    {
        public int GetLevel(long score);
        public ITokenConfig GetRandomToken(int level);
        public float GetLevelSpeed(int level);

        public Vector2Int BoardSize { get; }
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

        public Match(IMatchBuilder builder, IMatchConfig<T> configSetup)
        {
            Config = configSetup;
            tokenFactory = new TokenFactory();
            pieceFactory = new PieceFactory<T>(tokenFactory, Config);
            boardFactory = new BoardFactory(pieceFactory);
            playerFactory = new PlayerFactory(boardFactory);

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
                Players[i].Board.Update(deltaTime, 1f);
        }

        public void Quit()
        {
            OnClose?.Invoke();
        }

        public void Pause(bool pause = true)
        {
        }
    }
}
