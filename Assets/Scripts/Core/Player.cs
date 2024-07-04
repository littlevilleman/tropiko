using System;
using System.Collections.Generic;
using static Core.Events;

namespace Core
{
    public interface IPlayer
    {
        public event ReceiveScore OnReceiveScore;
        public event DefeatPlayer OnDefeat;
        public string Name { get; }
        public IBoard Board { get; }
        public long Score { get; }
        public bool IsDefeat { get; }
    }

    public class Player : IPlayer
    {
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
            board.ComboDispatcher.OnDispatch += OnComboDispatch;
        }

        private void OnComboDispatch(List<IToken> token, int comboIndex)
        {
            ReceiveScore(this, 1500);
        }

        private void OnBoardOverflow()
        {
            Board.OnOverflow -= OnBoardOverflow;
            Board.ComboDispatcher.OnDispatch -= OnComboDispatch;
            IsDefeat = true;
            OnDefeat?.Invoke(this);
        }

        public void ReceiveScore(IPlayer player, long score)
        {
            Score += score;
            OnReceiveScore?.Invoke(this, score);
        }
    }
}