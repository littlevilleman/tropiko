using Core.Map;

namespace Core
{
    public class CountBreakStrategy : IBreakStrategy
    {
        public int Counter { get; protected set; }

        public CountBreakStrategy(int breakCount)
        {
            Counter = breakCount;
        }
        public int Break(IBoardMap board, IToken token)
        {
            Counter--;

            if (Counter <= 0)
                token.Untomb(board);

            return Counter;
        }
    }
}