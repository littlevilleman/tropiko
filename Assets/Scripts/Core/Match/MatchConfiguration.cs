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
        public ITokenConfig GenerateToken(int level);
        public int GetPlayerLevel(long score);
        public float GetSpeed(int level);
        public float GetCollisionTime(long score);
        float GetScoreToLevel(int level);
    }

    public interface IArcadeMatchConfig : IMatchConfig
    {
    }

    public interface IMultiplayerMatchConfig : IMatchConfig
    {
    }

    public interface ICampaignMatchConfig : IMatchConfig
    {
    }

    public class MatchConfiguration
    {
    }
}
