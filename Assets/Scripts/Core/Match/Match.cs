using System.Linq;
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

    public abstract class Match : IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;
        public event PauseMatch OnPause;

        public IPlayer[] Players { get; protected set; }

        public void Launch()
        {
            OnLaunch?.Invoke(this);
        }

        public void Quit()
        {
            OnClose?.Invoke();
        }

        public void Pause(bool pause = true)
        {
        }

        protected void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.ToList().Where(x => x.IsDefeat == false).Count() == 0)
                Quit();
        }

        public virtual void Update(float deltaTime)
        {
            for (int i = 0; i < Players.Length; i++)
                Players[i].Board.Update(deltaTime, 1f);
        }
    }
}
