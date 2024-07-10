using System.Collections.Generic;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IMatch
    {
        public event LaunchMatch OnLaunch;
        public event PauseMatch OnPause;
        public event CloseMatch OnClose;

        public IMatchConfig Config { get; }
        public IPlayer[] Players { get; }
        public void Launch();
        public void Update(float deltaTime);
        public void Pause(bool pause = true);
        public void Quit();
    }

    public abstract class MatchContext
    {
        public IMatchBuilder builder;
        public IMatchConfig config;
        public IPlayer player;
        public float deltaTime = 0f;
        public float matchTime = 0f;

        public abstract int Level { get; }
        public abstract float Speed { get; }
        public abstract float CollisionTime { get; }
        public abstract IToken[,] PiecePreview { get; }
    }

    public abstract class Match : IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;
        public event PauseMatch OnPause;

        public IMatchConfig Config { get; protected set; }
        public IPlayer[] Players { get; protected set; }
        protected IMatchBuilder Builder { get; set; }
        protected float matchTime = 0f;

        protected abstract void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex);
        protected abstract void OnDefeatPlayer(IPlayer player);
        protected abstract MatchContext GetContext(IPlayer player, float deltaTime);

        public virtual void Update(float deltaTime)
        {
            foreach (IPlayer player in Players)
                player.Update(GetContext(player, deltaTime));

            matchTime += deltaTime;
        }

        public void Launch()
        {
            OnLaunch?.Invoke(this);
            Debug.Log("Match - Launch - " + this);
        }

        public void Pause(bool pause = true)
        {
            OnPause?.Invoke(this);
            Debug.Log("Match - Pause - " + this);
        }

        public void Quit()
        {
            OnClose?.Invoke();
            Debug.Log("Match - Close - " + this);
        }
    }
}