using static Core.Events;

namespace Core
{
    public interface IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;

        public IPlayer Build(string name);

    }
    public class PlayerFactory : IPlayerFactory
    {
        public event BuildPlayer OnBuildPlayer;
        private IBoardFactory boardFactory;

        public PlayerFactory(IBoardFactory boardFactorySetup)
        {
            boardFactory = boardFactorySetup;
        }

        public IPlayer Build(string name)
        {
            IPlayer player = new Player(name, boardFactory.Build());
            OnBuildPlayer?.Invoke(player);
            return player;
        }
    }

}