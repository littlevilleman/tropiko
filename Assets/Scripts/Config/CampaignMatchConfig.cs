using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Config
{

    [CreateAssetMenu(fileName = "Data", menuName = "Config/CampaignMatchConfig", order = 1)]
    public class CampaignMatchConfig : MatchConfig, ICampaignMatchConfig
    {
        public List<CampaignStageConfig> stageConfig;

        public Vector2Int BoardSize => stageConfig[0].boardSize;

        public ICampaignStageConfig GetStage(int stage)
        {
            if (stage >= stageConfig.Count)
                return stageConfig.Last();

            return stageConfig[stage];
        }
    }

    [Serializable]
    public class CampaignStageConfig : ICampaignStageConfig
    {
        public Vector2Int boardSize;

        public IToken[,] GenerateRandomPiece()
        {
            return null;
        }
    }
}