using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public interface IMultiplayerMatchMode : IMatchMode
    {

    }

    public class MultiplayerMatch : Match<IMultiplayerMatchMode>
    {
        public MultiplayerMatch(IMatchBuilder builder, IMatchConfig<IMultiplayerMatchMode> configSetup, List<PlayerProfile> profiles) : base(builder, configSetup)
        {
            List<IPlayer> players = new List<IPlayer>();
            foreach (PlayerProfile playerProfile in profiles)
            {
                IPlayer player = playerFactory.Build(playerProfile.Name, Config.BoardSize);
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
    }
}