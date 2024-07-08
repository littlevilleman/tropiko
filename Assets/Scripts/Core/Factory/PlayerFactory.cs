using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;
        public IPlayer Build(string name, Vector2Int size, IBoardFactory boardFactory);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;

        public IPlayer Build(string name, Vector2Int size, IBoardFactory boardFactory)
        {
            IPlayer player = new Player(name, boardFactory.Build(size));
            OnBuildPlayer?.Invoke(player);
            return player;
        }
    }

}