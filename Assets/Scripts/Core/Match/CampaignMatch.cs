using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public interface ICampaignMatch : IMatch
    {

    }

    public class CampaignMatch : Match, ICampaignMatch
    {
        public CampaignMatch(IMatchBuilderDispatcher dispatcher, ICampaignMatchConfig configSetup)
        {
            Builder = new MatchBuilder(dispatcher);
            Config = configSetup;
        }

        protected override MatchContext GetContext(IPlayer player, float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.ToList().Where(x => x.IsDefeat == false).Count() == 0)
                Quit();
        }

        protected override void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex)
        {
            throw new System.NotImplementedException();
        }
    }
    public class CampaignMatchContext : MatchContext
    {
        public override int Level => config.GetPlayerLevel(player.Score);
        public override float Speed => config.GetSpeed(Level);
        public override float CollisionTime => config.GetCollisionTime(player.Score);

        public override IToken[,] PiecePreview => builder.GetPiecePreview(config, Level);
    }
}