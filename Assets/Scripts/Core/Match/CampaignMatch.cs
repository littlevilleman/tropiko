using System.Collections.Generic;
using System.Linq;
using static Core.Events;

namespace Core
{
    public interface ICampaignMatch : IMatch
    {
    }

    public class CampaignMatch : Match, ICampaignMatch
    {
        public event LaunchStage OnLaunchStage;
        public event CompleteStage OnCompleteStage;

        private ICampaignMatchConfig config;
        private ICampaignStageConfig currentStage;

        public CampaignMatch(IMatchBuilderDispatcher dispatcher, ICampaignMatchConfig configSetup)
        {
            Builder = new MatchBuilder(dispatcher);
            config = configSetup;
            //CurrentStage = Config.GetStage(0);
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.ToList().Where(x => x.IsDefeat == false).Count() == 0)
                Quit();
        }

        protected override void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex)
        {
        }

        protected override MatchContext GetContext(IPlayer player, float deltaTime)
        {
            return new CampaignMatchContext
            {
                builder = Builder,
                deltaTime = deltaTime,
                matchTime = matchTime,
                level = 0,
                tokenGenerator = currentStage,
            };
        }
    }

    public class CampaignMatchContext : MatchContext
    {
    }

    public interface ICampaignStageConfig : ITokenGenerator
    {
    }
}