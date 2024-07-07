using Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/MultiplayerMatchConfig", order = 1)]
    public class MultiplayerMatchConfig : MatchConfig, IMatchConfig<IMultiplayerMatchMode>
    {
        public Vector2Int boardSize;

        public Vector2Int BoardSize => boardSize;

        public int GetLevel(long score)
        {
            return 1;
        }

        public float GetLevelSpeed(int level)
        {
            return 1f;
        }

        public ITokenConfig GetRandomToken(int level)
        {
            List<TokenConfig> tokens = levels[level].tokens.ToList();
            return tokens[Random.Range(0, tokens.Count)];
        }
    }
}