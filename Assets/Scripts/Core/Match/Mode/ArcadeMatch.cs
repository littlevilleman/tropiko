using System.Linq;
using static Core.Events;

namespace Core
{
    public interface IMatchMode
    {

    }

    public interface IArcadeMatchMode : IMatchMode
    {
    }

    public class ArcadeMatch : Match<IArcadeMatchMode>
    {
        public event ArcadeMatchLevelUp OnLevelUp;
        public int CurrentLevel => Config.GetLevel(Players[0].Score);
        public float CurrentSpeed => Config.GetLevelSpeed(CurrentLevel);

        public float CurrentCollisionTime => .5f;
        protected float entombCooldown = 10f;

        public ArcadeMatch(IMatchBuilder builder, IMatchConfig<IArcadeMatchMode> configSetup, PlayerProfile profile) : base(builder, configSetup)
        {
            Players = new IPlayer[1] { playerFactory.Build(profile.Name, Config.BoardSize) };

            foreach (IPlayer player in Players)
            {
                player.OnDefeat += OnDefeatPlayer;
                player.OnReceiveScore += OnReceiveScore;
            }
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < Players.Length; i++)
                Players[i].Board.Update(deltaTime, CurrentSpeed, CurrentLevel, CurrentCollisionTime);

            entombCooldown -= deltaTime;
            if (entombCooldown <= 0)
            {
                for (int i = 0; i < Players.Length; i++)
                    Players[i].Board.TombDispatcher.AddCandidates(3);

                entombCooldown = 10f;
            }
        }

        protected override void OnReceiveScore(IPlayer player, long score)
        {
            int previousLevel = Config.GetLevel(player.Score - score);
            if (previousLevel != CurrentLevel)
                OnLevelUp?.Invoke(CurrentLevel);
        }

        protected override void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            if (Players.ToList().Where(x => x.IsDefeat == false).Count() == 0)
                Quit();
        }
    }
}