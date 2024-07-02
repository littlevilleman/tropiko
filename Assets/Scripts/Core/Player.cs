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
    }

    public class Player : IPlayer
    {
        public event ReceiveScore OnReceiveScore;
        public event DefeatPlayer OnDefeat;

        public string Name { get; private set; }
        public IBoard Board { get; private set; }
        public long Score { get; private set; }

        public Player(string name, IBoard board) 
        {
            Board = board;
            Name = name;

            board.OnOverflow += OnBoardOverflow;
            board.ComboDispatcher.OnDispatch += OnComboDispatch;
        }

        private void OnComboDispatch(List<IToken> token, int comboIndex)
        {
            ReceiveScore(1500);
        }

        private void OnBoardOverflow()
        {
            Board.OnOverflow -= OnBoardOverflow;
            Board.ComboDispatcher.OnDispatch -= OnComboDispatch;

            OnDefeat?.Invoke(this);
        }

        public void ReceiveScore(long score)
        {
            Score += score;
            OnReceiveScore?.Invoke(score);
        }
    }
}