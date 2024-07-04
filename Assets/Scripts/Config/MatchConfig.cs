using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/MatchConfig", order = 1)]
    public class MatchConfig : ScriptableObject, IMatchConfig<IArcadeMatch>
    {
        public AnimationCurve levelByScore;
        public AnimationCurve speedByLevel;
        public List<ArcadeMatchLevelConfig> levels;
        public long maxScore;

        public int GetLevel(long score)
        {
            return Mathf.Clamp(Mathf.FloorToInt(levelByScore.Evaluate(score * 1f / maxScore)), 0, levels.Count - 1);
        }
        public float GetLevelSpeed(int level)
        {
            return Mathf.Clamp(speedByLevel.Evaluate(level * 1f / levels.Count), 1f, 20f);
        }

        public List<ETokenType> GetLevelTokens(int level)
        {
            return levels[level].tokens;
        }
    }

    [Serializable]
    public class ArcadeMatchLevelConfig
    {
        public List<ETokenType> tokens = new List<ETokenType>() { ETokenType.WATER, ETokenType.LEAF, ETokenType.BUG };
    }
}
