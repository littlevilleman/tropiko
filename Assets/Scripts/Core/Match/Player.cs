using static Core.Events;

namespace Core
{
    public interface IPlayer
    {
        public event PlayerLevelUp OnLevelUp;
        public event ReceiveScore OnReceiveScore;
        public event DefeatPlayer OnDefeat;
        public string Name { get; }
        public IBoard Board { get; }
        public long Score { get; }
        public bool IsDefeat { get; }
        public void Update(MatchContext context);
        public void ReceiveScore(long score, bool levelUp = false);
        public void LevelUp(int level);
    }

    public class Player : IPlayer
    {
        public event PlayerLevelUp OnLevelUp;
        public event ReceiveScore OnReceiveScore;
        public event DefeatPlayer OnDefeat;

        public string Name { get; private set; }
        public IBoard Board { get; private set; }
        public long Score { get; private set; }
        public bool IsDefeat { get; private set; }

        public Player(string name, IBoard board) 
        {
            Board = board;
            Name = name;

            board.OnOverflow += OnBoardOverflow;
        }

        public void Update(MatchContext context)
        {
            Board?.Update(context);
        }

        private void OnBoardOverflow()
        {
            Board.OnOverflow -= OnBoardOverflow;
            IsDefeat = true;
            OnDefeat?.Invoke(this);
        }

        public void ReceiveScore(long score, bool levelUp = false)
        {
            Score += score;
            OnReceiveScore?.Invoke(this, score);
        }

        public void LevelUp(int level)
        {
            OnLevelUp?.Invoke(level);
        }
    }
}