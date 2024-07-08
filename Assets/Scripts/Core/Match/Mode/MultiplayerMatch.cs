using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public interface IMultiplayerMatchMode : IMatchMode
    {

    }
    public interface IMultiplayerMatch : IMatch
    {

    }

    public class MultiplayerMatch : Match<IMultiplayerMatchMode>, IMultiplayerMatch
    {
        public MultiplayerMatch(IMatchBuilder builder, IMatchConfig<IMultiplayerMatchMode> configSetup, List<PlayerProfile> profiles) : base(builder, configSetup)
        {
            List<IPlayer> players = new List<IPlayer>();
            foreach (PlayerProfile playerProfile in profiles)
            {
                IPlayer player = playerFactory.Build(playerProfile.Name, Config.BoardSize, boardFactory);
                player.OnDefeat += OnDefeatPlayer;
                players.Add(player);
            }

            Config = configSetup;
            Players = players.ToArray();
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

        protected override MatchContext<IMultiplayerMatchMode> GetContext(float deltaTime)
        {
            return new MultiplayerMatchContext
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

    public class MultiplayerMatchContext : MatchContext<IMultiplayerMatchMode>
    {

    }
}