using UnityEngine;
using static Core.Events;

namespace Core.Factory
{
    public interface IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;
        public IPlayer Build(string name, IBoard board);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;

        public IPlayer Build(string name, IBoard board)
        {
            IPlayer player = new Player(name, board);
            OnBuildPlayer?.Invoke(player);
            return player;
        }
    }

}