using System.Collections.Generic;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IMatchBuilderDispatcher
    {
        public BuildPlayer OnBuildPlayer { get; }
        public BuildBoard OnBuildBoard { get; }
        public BuildToken OnBuildToken { get; }
    }

    public interface IMatchConfig
    {
        public Vector2Int BoardSize { get; }
    }

    public interface IArcadeMatchConfig : IMatchConfig
    {
        public int GetPlayerLevel(long score);
        public float GetProgress(long score);
        public float GetScoreToNextLevel(long score);
        public float GetSpeed(long score);
        public float GetCollisionTime(long score);
        public ITokenConfig GenerateRandomToken(long score);
        public float GetTombs(long score);
    }

    public interface IMultiplayerMatchConfig : IMatchConfig
    {
    }

    public interface ICampaignMatchConfig : IMatchConfig
    {
        public ICampaignStageConfig GetStage(int stage);
    }

    public interface IArcadeLevelConfig
    {

    }

    public interface IArcadeLevelTokenConfig
    {

    }

    public class MatchConfiguration
    {
    }
}
