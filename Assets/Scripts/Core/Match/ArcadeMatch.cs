using static Core.Events;

namespace Core
{
    public interface IArcadeMatch : IMatch
    {
        public event ArcadeMatchLevelUp OnLevelUp;
        public int CurrentLevel { get; }
        public float Speed { get; }
        public IMatchConfig<IArcadeMatch> Config { get; }
    }

    public class ArcadeMatch : Match, IArcadeMatch
    {
        public event ArcadeMatchLevelUp OnLevelUp;
        public IMatchConfig<IArcadeMatch> Config { get; private set; }

        public int CurrentLevel => Config.GetLevel(Players[0].Score);
        public float Speed => Config.GetLevelSpeed(CurrentLevel);

        public ArcadeMatch(IMatchConfig<IArcadeMatch> configSetup, IPlayer playerSetup)
        {
            Config = configSetup;
            Players = new IPlayer[1] { playerSetup };

            foreach (IPlayer player in Players)
            {
                player.OnDefeat += OnDefeatPlayer;
                player.OnReceiveScore += OnReceiveScore;
            }
        }

        private void OnReceiveScore(IPlayer player, long score)
        {
            int previousLevel = Config.GetLevel(player.Score - score);
            if (previousLevel != CurrentLevel)
                OnLevelUp?.Invoke(CurrentLevel);
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < Players.Length; i++)
                Players[i].Board.Update(deltaTime, Speed, CurrentLevel);
        }
    }
}