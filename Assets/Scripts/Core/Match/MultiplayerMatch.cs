namespace Core
{
    public interface IMultiplayerMatch : IMatch
    {

    }

    public class MultiplayerMatch : Match, IMultiplayerMatch
    {
        public MultiplayerMatch(IPlayer[] playersSetup)
        {
            Players = playersSetup;

            foreach (IPlayer player in Players)
            {
                player.OnDefeat += OnDefeatPlayer;
            }
        }
    }
}