using System.Collections.Generic;
using static Core.Events;

namespace Core
{
    public interface IMatch
    {
        public event LaunchMatch OnLaunch;
        public event CloseMatch OnClose;

        public IPlayer[] Players { get; }
        public void Launch();
        public void Close();
    }

    public interface IMatchFactory
    {
        public IMatch BuildSinglePlayerMatch();
        public IMatch BuildMultiplayerMatch();
    }

    public class MatchFactory : IMatchFactory
    {
        private IMatchLobby lobby;

        private IPlayerFactory playerFactory;
        private IBoardFactory boardFactory;
        private ITokenFactory tokenFactory;

        public MatchFactory(BuildPlayer OnBuildPlayer, BuildBoard OnBuildBoard, BuildToken OnBuildToken)
        {
            tokenFactory = new TokenFactory();
            boardFactory = new BoardFactory(new PieceFactory(tokenFactory));
            playerFactory = new PlayerFactory(boardFactory);

            playerFactory.OnBuildPlayer += OnBuildPlayer;
            boardFactory.OnBuildBoard += OnBuildBoard;
            tokenFactory.OnBuildToken += OnBuildToken;
        }

        public IMatch BuildSinglePlayerMatch()
        {
            return new SinglePlayerMatch(playerFactory.Build("Player01"));
        }

        public IMatch BuildMultiplayerMatch()
        {
            lobby = new MatchLobby();

            List<IPlayer> players = new List<IPlayer>();
            foreach (PlayerProfile playerProfile in lobby.Players)
            {
                players.Add(playerFactory.Build(playerProfile.Name));
            }

            return new MultiplayerMatch(players.ToArray());
        }
    }
}
