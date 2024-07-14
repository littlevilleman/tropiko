using Core;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "Data", menuName = "Config/MultiplayerMatchConfig", order = 1)]
    public class MultiplayerMatchConfig : MatchConfig, IMultiplayerMatchConfig
    {
        public Vector2Int boardSize;

        public Vector2Int BoardSize => boardSize;

        public float GetCollisionTime(long score)
        {
            return .5f;
        }

        public int GetPlayerLevel(long score)
        {
            return 1;
        }

        public float GetSpeed(int level)
        {
            return 1f;
        }

        public float GetScoreToLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}