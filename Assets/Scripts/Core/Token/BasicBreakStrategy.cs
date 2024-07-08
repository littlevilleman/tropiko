using Core.Map;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Core
{
    public interface IBreakStrategy
    {
        public int Counter { get; }

        int Break(IBoardMap board, IToken token);
    }

    public class BasicBreakStrategy : IBreakStrategy
    {
        public int Counter { get; protected set; } = 1;

        public int Break(IBoardMap board, IToken token)
        {
            Counter--;
            board.RemoveToken(token.Location);
            return Counter;
        }
    }
}