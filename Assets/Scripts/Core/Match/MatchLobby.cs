using System;
using System.Collections.Generic;

namespace Core
{
    public interface IMatchLobby
    {
        public List<PlayerProfile> Players { get; }
        public void AddPlayer(PlayerProfile profile);
        public void RemovePlayer(PlayerProfile profile);
    }

    public class MatchLobby : IMatchLobby
    {
        public List<PlayerProfile> Players { get; private set; } = new List<PlayerProfile>(4);

        public MatchLobby()
        {
            Players = new List<PlayerProfile>
            {
                new PlayerProfile() { Name = "Player01" },
                //new PlayerProfile() { Name = "Player02" },
                //new PlayerProfile() { Name = "Player03" },
                //new PlayerProfile() { Name = "Player04" },
            };
        }

        public void AddPlayer(PlayerProfile profile)
        {
            Players.Add(profile);
        }
        public void RemovePlayer(PlayerProfile profile)
        {
            Players.Remove(profile);
        }
    }

    [Serializable]
    public class PlayerProfile
    {
        public string Name;
    }
}
