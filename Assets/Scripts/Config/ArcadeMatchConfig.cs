using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/ArcadeMatchConfig", order = 1)]
    public class ArcadeMatchConfig : MatchConfig, IArcadeMatchConfig
    {
        public List<ArcadeLevelConfig> levelsConfig;

        public Vector2Int BoardSize => levelsConfig[0].boardSize;

        public int GetPlayerLevel(long score)
        {
            return levelsConfig.FindLastIndex(x => x.scoreToComplete <= score);
        }

        public float GetScoreToLevel(int level)
        {
            if (level >= levelsConfig.Count)
                return levelsConfig.Count - 1;

            return levelsConfig[level].scoreToComplete;
        }

        public float GetSpeed(int level)
        {
            return Mathf.Clamp(levelsConfig[level].speedFactor, 1f, 20f);
        }

        public ITokenConfig GenerateToken(int level)
        {
            ArcadeMatchTokenConfig tokenConfig = ProbabilityDispatcher.LaunchProbability(levelsConfig[level].tokensConfig, level, levelsConfig.Count);
            return tokenConfig.token;
        }
        public float GetCollisionTime(long score)
        {
            return levelsConfig[GetPlayerLevel(score)].collisionTime;
        }
    }

    [Serializable]
    public class ArcadeLevelConfig
    {
        public List<ArcadeMatchTokenConfig> tokensConfig;
        public Vector2Int boardSize = new Vector2Int(6, 13);
        public long scoreToComplete = 1000000;
        public float speedFactor = 1f;
        public float collisionTime = .5f;
        public float tombsFactor = 0f;
    }

    public class ProbabilityItem<T>
    {
        public float probability;
        public T item;
    }

    public static class ProbabilityDispatcher
    {
        public static ArcadeMatchTokenConfig LaunchProbability(List<ArcadeMatchTokenConfig> tokensConfig, int currentLevel, int maxLevel)
        {
            List<ProbabilityItem <ArcadeMatchTokenConfig>> items = new List<ProbabilityItem<ArcadeMatchTokenConfig>>();
            foreach (ArcadeMatchTokenConfig config in tokensConfig)
            {
                float probability = UnityEngine.Random.Range(0f, 1f) * config.Evaluate(currentLevel, maxLevel);
                items.Add(new ProbabilityItem<ArcadeMatchTokenConfig>() { item = config, probability = probability});
            }

            return items.OrderByDescending(x => x.probability).First().item;
        }
    }


    [Serializable]
    public class ArcadeMatchTokenConfig
    {
        public TokenConfig token;
        public AnimationCurve spawnByLevel;

        public float Evaluate(int level, int maxLevels)
        {
            return spawnByLevel.Evaluate(level * 1f / maxLevels);
        }
    }
}