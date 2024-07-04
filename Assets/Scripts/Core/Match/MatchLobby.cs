using System;
using System.Collections.Generic;

namespace Core
{
    public interface IMatchLobby : IMatchFactory<IMultiplayerMatch>
    {
        public List<PlayerProfile> PlayerProfiles { get; }
        public void AddPlayer(PlayerProfile profile);
        public void RemovePlayer(PlayerProfile profile);
    }

    public class MatchLobby : IMatchLobby
    {
        public List<PlayerProfile> PlayerProfiles { get; private set; } = new List<PlayerProfile>(4);

        private IPlayerFactory playerFactory;
        private IBoardFactory boardFactory;
        private ITokenFactory tokenFactory;

        public MatchLobby(IMatchBuilder matchBuilder)
        {
            tokenFactory = new TokenFactory();
            boardFactory = new BoardFactory(new PieceFactory(tokenFactory));
            playerFactory = new PlayerFactory(boardFactory);

            playerFactory.OnBuildPlayer += matchBuilder.OnBuildPlayer;
            boardFactory.OnBuildBoard += matchBuilder.OnBuildBoard;
            tokenFactory.OnBuildToken += matchBuilder.OnBuildToken;

            PlayerProfiles = new List<PlayerProfile>
            {
                new PlayerProfile() { Name = "Player01" },
                new PlayerProfile() { Name = "Player02" },
                new PlayerProfile() { Name = "Player03" },
                //new PlayerProfile() { Name = "Player04" },
            };
        }

        public void AddPlayer(PlayerProfile profile)
        {
            PlayerProfiles.Add(profile);
        }
        public void RemovePlayer(PlayerProfile profile)
        {
            PlayerProfiles.Remove(profile);
        }

        public IMultiplayerMatch Build(IMatchConfig<IMultiplayerMatch> config)
        {
            List<IPlayer> players = new List<IPlayer>();
            foreach (PlayerProfile playerProfile in PlayerProfiles)
            {
                players.Add(playerFactory.Build(playerProfile.Name));
            }

            return new MultiplayerMatch(players.ToArray());
        }
    }

    [Serializable]
    public class PlayerProfile
    {
        public string Name;
    }
}
