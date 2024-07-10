using Core;
using UnityEngine;

namespace Config
{

    [CreateAssetMenu(fileName = "Data", menuName = "Config/CampaignMatchConfig", order = 1)]
    public class CampaignMatchConfig : MatchConfig, IMatchConfig
    {
        public Vector2Int BoardSize => throw new System.NotImplementedException();

        public float GetCollisionTime(long score)
        {
            throw new System.NotImplementedException();
        }

        public int GetPlayerLevel(long score)
        {
            throw new System.NotImplementedException();
        }

        public float GetSpeed(int level)
        {
            throw new System.NotImplementedException();
        }

        public ITokenConfig GenerateToken(int level)
        {
            throw new System.NotImplementedException();
        }

        public float GetScoreToLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}