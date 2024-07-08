using Core;
using UnityEngine;

namespace Config
{

    [CreateAssetMenu(fileName = "Data", menuName = "Config/CampaignMatchConfig", order = 1)]
    public class CampaignMatchConfig : MatchConfig, IMatchConfig<ICampaignMatchMode>
    {
        public Vector2Int BoardSize => throw new System.NotImplementedException();

        public int GetLevel(long score)
        {
            throw new System.NotImplementedException();
        }

        public float GetLevelSpeed(int level)
        {
            throw new System.NotImplementedException();
        }

        public ITokenConfig GetRandomToken(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}