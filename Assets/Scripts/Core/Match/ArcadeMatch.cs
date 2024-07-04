using System;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IArcadeMatch : IMatch
    {
        public event ArcadeMatchLevelUp OnLevelUp;
        public int CurrentLevel { get; }
        public float Speed { get; }
    }

    public class ArcadeMatch : Match, IArcadeMatch
    {
        public event ArcadeMatchLevelUp OnLevelUp;
        private IMatchConfig<IArcadeMatch> config;

        public int CurrentLevel => config.GetLevel(Players[0].Score);
        public float Speed => config.GetLevelSpeed(CurrentLevel);

        public ArcadeMatch(IMatchConfig<IArcadeMatch> configSetup, IPlayer playerSetup)
        {
            config = configSetup;
            Players = new IPlayer[1] { playerSetup };

            foreach (IPlayer player in Players)
            {
                player.OnDefeat += OnDefeatPlayer;
                player.OnReceiveScore += OnReceiveScore;
            }
        }

        private void OnReceiveScore(IPlayer player, long score)
        {
            int previousLevel = config.GetLevel(player.Score - score);
            if (previousLevel != CurrentLevel)
                OnLevelUp?.Invoke(CurrentLevel);
        }

        public override void Update(float deltaTime)
        {
            Debug.Log("SCORE " + Players[0].Score + "LEVEL " + CurrentLevel + " SPEED " + Speed);
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Board.Update(deltaTime, Speed);
            }
        }
    }
}