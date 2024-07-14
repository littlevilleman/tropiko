using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/ArcadeMatchConfig", order = 1)]
    public class ArcadeMatchConfig : MatchConfig, IArcadeMatchConfig
    {
        [SerializeField] private Vector2Int boardSize = new Vector2Int(6, 13);
        [SerializeField] private long maxScore = 1000000;
        [SerializeField] private List<ArcadeMatchTokenConfig> tokens;
        [SerializeField] private AnimationCurve level;
        [SerializeField] private AnimationCurve speed;
        [SerializeField] private AnimationCurve collisionTime;
        [SerializeField] private AnimationCurve buryTokens;

        public Vector2Int BoardSize => boardSize;

        public float GetProgress(long score)
        {
            return score * 1f / maxScore;
        }

        public int GetPlayerLevel(long score)
        {
            return Mathf.FloorToInt(level.Evaluate(GetProgress(score)));
        }

        public float GetScoreToNextLevel(long score)
        {
            return maxScore;
        }

        public float GetSpeed(long score)
        {
            return Mathf.Clamp(speed.Evaluate(GetProgress(score)), 1f, 20f);
        }

        public float GetCollisionTime(long score)
        {
            return Mathf.Clamp(collisionTime.Evaluate(GetProgress(score)), 0f, .5f);
        }

        public ITokenConfig GenerateRandomToken(long score)
        {
            return ProbabilityDispatcher.LaunchProbability(tokens, GetProgress(score)).token;
        }

        public float GetTombs(long score)
        {
            return Mathf.Clamp(buryTokens.Evaluate(GetProgress(score)), 0f, .5f);
        }
    }

    [Serializable]
    public class ArcadeMatchTokenConfig : IArcadeLevelTokenConfig, IProbabilityItem
    {
        public TokenConfig token;
        public AnimationCurve spawnByLevel;

        public float Evaluate(float progress)
        {
            return spawnByLevel.Evaluate(progress);
        }
    }
}