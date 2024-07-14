using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public interface IMultiplayerMatch : IMatch
    {
        public IMultiplayerMatchConfig Config { get; }
    }

    public class MultiplayerMatch : Match, IMultiplayerMatch, ITokenGenerator
    {
        public IMultiplayerMatchConfig Config { get; protected set; }
        protected List<IPlayer> AlivePlayers => Players.Where(x => x.IsDefeat == false).ToList();

        public MultiplayerMatch(IMatchBuilderDispatcher dispatcher, IMultiplayerMatchConfig configSetup, List<PlayerProfile> profiles)
        {
            Builder = new MatchBuilder(dispatcher);
            Config = configSetup;

            List<IPlayer> players = new List<IPlayer>();
            foreach (PlayerProfile playerProfile in profiles)
            {
                IPlayer player = Builder.BuildPlayer(Config, playerProfile.Name);
                player.OnDefeat += OnDefeatPlayer;
                players.Add(player);
            }

            Config = configSetup;
            Players = players.ToArray();
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (AlivePlayers.Count == 0)
                Quit();
        }

        protected override MatchContext GetContext(IPlayer player, float deltaTime)
        {
            return new MultiplayerMatchContext
            {
                deltaTime = deltaTime,
                builder = Builder,
                tokenGenerator = this
            };
        }

        protected override void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex)
        {
            List<IPlayer> targets = AlivePlayers;
            targets.Remove(player);
            foreach (IPlayer target in targets)
            {
                //player.AddTombs(2);
            }
        }

        public IToken[,] GenerateRandomPiece()
        {
            throw new System.NotImplementedException();
        }
    }

    public class MultiplayerMatchContext : MatchContext
    {
    }
}