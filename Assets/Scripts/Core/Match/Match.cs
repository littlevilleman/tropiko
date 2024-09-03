using System.Collections.Generic;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface ITokenGenerator
    {
        public IToken[,] GenerateRandomPiece();
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

    public abstract class MatchContext
    {
        public IMatchBuilder builder;
        public ITokenGenerator tokenGenerator;
        public IArcadeMatchConfig config;

        public float deltaTime = 0f;
        public float matchTime = 0f;
        public float collisionTime = .5f;
        public float speed = 1f;
        public float tombs = 0f;
        public int level = 0;
    }

    public abstract class Match : IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;
        public event PauseMatch OnPause;

        protected abstract void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex);
        protected abstract void OnDefeatPlayer(IPlayer player);
        protected abstract MatchContext GetContext(IPlayer player, float deltaTime);

        public IPlayer[] Players { get; protected set; }
        protected IMatchBuilder Builder { get; set; }
        protected float matchTime = 0f;

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