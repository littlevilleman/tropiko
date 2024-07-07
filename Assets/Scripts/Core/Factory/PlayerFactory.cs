using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;
        public IPlayer Build(string name, Vector2Int size);
    }

    public class PlayerFactory : IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;
        private IBoardFactory boardFactory;

        public PlayerFactory(IBoardFactory boardFactorySetup)
        {
            boardFactory = boardFactorySetup;
        }

        public IPlayer Build(string name, Vector2Int size)
        {
            IPlayer player = new Player(name, boardFactory.Build(size));
            OnBuildPlayer?.Invoke(player);
            return player;
        }
    }

}