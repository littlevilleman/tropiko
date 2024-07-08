using Core.Map;

namespace Core
{
    public interface IFallStrategy
    {
        void Fall(IBoardMap board, IToken token);
    }
    public class BasicFallStrategy : IFallStrategy
    {
        public void Fall(IBoardMap board, IToken token)
        {
            board.RemoveToken(token.Location);
            token.SetPosition(board, MapUtils.GetFallLocation(board, token.Location));
            board.LocateToken(token);
        }
    }
}
