using static Core.Events;

namespace Core
{
    public class SinglePlayerMatch : IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;

        public IPlayer[] Players { get; private set; }

        public SinglePlayerMatch(IPlayer playerSetup)
        {
            Players = new IPlayer [1] { playerSetup};
        }

        public void Launch()
        {
            OnLaunch?.Invoke(this);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }

    public class MultiplayerMatch : IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;

        public IPlayer[] Players { get; private set; }

        public MultiplayerMatch(IPlayer[] playersSetup)
        {
            Players = playersSetup;
        }

        public void Launch()
        {
            OnLaunch?.Invoke(this);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }

    public enum EMatchMode
    {
        Single, Multiplayer
    }
}