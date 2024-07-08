using System.Linq;

namespace Core
{

    public interface ICampaignMatchMode : IMatchMode
    {

    }

    public interface ICampaignMatch : IMatch
    {

    }

    public class CampaignMatch : Match<ICampaignMatchMode>, ICampaignMatch
    {
        public CampaignMatch(IMatchBuilder builder, IMatchConfig<ICampaignMatchMode> configSetup) : base(builder, configSetup)
        {
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.ToList().Where(x => x.IsDefeat == false).Count() == 0)
                Quit();
        }

        protected override void OnReceiveScore(IPlayer player, long score)
        {
        }

        protected override MatchContext<ICampaignMatchMode> GetContext(float deltaTime)
        {
            return new CampaignMatchContext
            {
                pieceFactory = pieceFactory,
                config = Config,
                time = deltaTime,
                //collisionTime = CurrentCollisionTime,
                //speed = CurrentSpeed,
                //level = CurrentLevel,
            };
        }
    }
    public class CampaignMatchContext : MatchContext<ICampaignMatchMode>
    {

    }
}