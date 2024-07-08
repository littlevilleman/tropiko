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
        public AnimationCurve levelByScore;
        public AnimationCurve speedByLevel;
        public long maxScore;
        public Vector2Int boardSize;

        public Vector2Int BoardSize => boardSize;


        public int GetLevel(long score)
        {
            return Mathf.Clamp(Mathf.FloorToInt(levelByScore.Evaluate(score * 1f / maxScore)), 0, levels.Count - 1);
        }
        public float GetLevelSpeed(int level)
        {
            return Mathf.Clamp(speedByLevel.Evaluate(level * 1f / levels.Count), 1f, 20f);
        }

        public ITokenConfig GetRandomToken(int level)
        {
            List<TokenConfig> tokens = levels[level].tokens.ToList();
            return tokens[UnityEngine.Random.Range(0, tokens.Count)];
        }
    }


    [Serializable]
    public class ArcadeMatchLevelConfig
    {
        public List<TokenConfig> tokens = new List<TokenConfig>();
    }
}