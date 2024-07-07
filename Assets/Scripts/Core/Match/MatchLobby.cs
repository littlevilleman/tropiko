using System;
using System.Collections.Generic;

namespace Core
{
    public interface IMatchLobby
    {
        public List<PlayerProfile> PlayerProfiles { get; }
        public void AddPlayer(PlayerProfile profile);
        public void RemovePlayer(PlayerProfile profile);
        public IMatch Build<T>(IMatchBuilder builder, IMatchConfig<T> config) where T : IMatchMode;
    }

    public class MatchLobby : IMatchLobby
    {
        public List<PlayerProfile> PlayerProfiles { get; private set; } = new List<PlayerProfile>(4)
        {
            new PlayerProfile() { Name = "Player01" },
            new PlayerProfile() { Name = "Player02" },
            new PlayerProfile() { Name = "Player03" },
            new PlayerProfile() { Name = "Player04" },
        };

        public void AddPlayer(PlayerProfile profile)
        {
            PlayerProfiles.Add(profile);
        }
        public void RemovePlayer(PlayerProfile profile)
        {
            PlayerProfiles.Remove(profile);
        }

        public IMatch Build<T>(IMatchBuilder builder, IMatchConfig<T> config) where T : IMatchMode
        {
            if (config is IMatchConfig<IArcadeMatchMode> arcadeConfig)
                return new ArcadeMatch(builder, arcadeConfig, PlayerProfiles[0]);

            if (config is IMatchConfig<IMultiplayerMatchMode> multiplayerConfig)
                return new MultiplayerMatch(builder, multiplayerConfig, PlayerProfiles);

            return null;
        }
    }

    [Serializable]
    public class PlayerProfile
    {
        public string Name;
    }
}