using System.Collections.Generic;
using System.Linq;
using static Core.Events;

namespace Core
{
    public interface IArcadeMatch : IMatch
    {
        public int Level { get; }
        public float Progress { get; }
    }

    public class ArcadeMatch : Match, IArcadeMatch, ITokenGenerator
    {
        public event LevelUp OnLevelUp;
        protected float entombCooldown = 10f;
        protected IArcadeMatchConfig config;

        public int Level => config.GetPlayerLevel(Players[0].Score);
        public float Progress => (Players[0].Score * 1f - config.GetScoreToNextLevel(Level)) / (config.GetScoreToNextLevel(Level +1) - config.GetScoreToNextLevel(Level));


        public ArcadeMatch(IMatchBuilderDispatcher dispatcher, IArcadeMatchConfig configSetup, PlayerProfile profile)
        {
            Builder = new MatchBuilder(dispatcher);
            config = configSetup;
            Players = new IPlayer[1] { Builder.BuildPlayer(config, profile.Name) };

            foreach (IPlayer player in Players)
            {
                player.OnDefeat += OnDefeatPlayer;
                player.Board.OnDispatchCombo += (tokens, index) => OnDispatchCombo(player, tokens, index);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            //entombCooldown -= deltaTime;
            //if (entombCooldown <= 0)
            //{
            //    for (int i = 0; i < Players.Length; i++)
            //        Players[i].Board.TombDispatcher.AddCandidates(3);
            //
            //    entombCooldown = 10f;
            //}
        }

        protected override void OnDispatchCombo(IPlayer player, List<IToken> tokens, int comboIndex)
        {
            int level = config.GetPlayerLevel(player.Score);
            player.ReceiveScore(1500);
            int nextLevel = config.GetPlayerLevel(player.Score);

            if (level < nextLevel)
                player.LevelUp(nextLevel);
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.Where(x => !x.IsDefeat).Count() == 0)
                Quit();
        }

        protected override MatchContext GetContext(IPlayer player, float deltaTime)
        {
            return new ArcadeMatchContext
            {
                builder = Builder,
                deltaTime = deltaTime,
                matchTime = matchTime,
                level = config.GetPlayerLevel(player.Score),
                speed = config.GetSpeed(player.Score),
                collisionTime = config.GetCollisionTime(player.Score),
                tombs = config.GetTombs(player.Score),
                tokenGenerator = this
            };
        }

        public IToken[,] GenerateRandomPiece()
        {
            ITokenConfig[] tokens = new ITokenConfig[] { config.GenerateRandomToken(Players[0].Score), config.GenerateRandomToken(Players[0].Score), config.GenerateRandomToken(Players[0].Score) };
            return Builder.GetPiecePreview(tokens);
        }
    }

    public class ArcadeMatchContext : MatchContext
    {
    }
}