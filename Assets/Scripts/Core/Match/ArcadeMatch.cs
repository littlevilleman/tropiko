using System.Collections.Generic;
using System.Linq;
using static Core.Events;

namespace Core
{
    public interface IArcadeMatch : IMatch
    {
        int Level { get; }
        float LevelProgress { get; }
    }

    public class ArcadeMatch : Match, IArcadeMatch
    {
        public event PlayerLevelUp OnLevelUp;
        protected float entombCooldown = 10f;

        public float LevelProgress => (Players[0].Score * 1f - Config.GetScoreToLevel(Level)) / (Config.GetScoreToLevel(Level +1) - Config.GetScoreToLevel(Level));

        public int Level => Config.GetPlayerLevel(Players[0].Score);

        public ArcadeMatch(IMatchBuilderDispatcher dispatcher, IArcadeMatchConfig configSetup, PlayerProfile profile)
        {
            Builder = new MatchBuilder(dispatcher);
            Config = configSetup;
            Players = new IPlayer[1] { Builder.BuildPlayer(Config, profile.Name) };

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
            int level = Config.GetPlayerLevel(player.Score);
            player.ReceiveScore(1500);
            int nextLevel = Config.GetPlayerLevel(player.Score);

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
                config = Config,
                player = player,
                deltaTime = deltaTime,
                matchTime = matchTime,
            };
        }
    }

    public class ArcadeMatchContext : MatchContext
    {
        public override int Level => config.GetPlayerLevel(player.Score);
        public override float Speed => config.GetSpeed(Level);
        public override float CollisionTime => config.GetCollisionTime(player.Score);
        public override IToken[,] PiecePreview => builder.GetPiecePreview(config, Level);
    }
}