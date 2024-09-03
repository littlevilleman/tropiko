using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace Core
{
    public interface IComboDispatcher
    {
        public void AddCandidate(IToken token);
        public bool TryDispatch(IBoard board);
    }

    public class ComboDispatcher : IComboDispatcher
    {

        private List<IToken> candidates = new List<IToken>();

        private int comboIndex = 0;

        private IToken currentCandidate;

        public void AddCandidate(IToken token)
        {
            candidates.Add(token);
        }

        public bool TryDispatch(IBoard board)
        {
            if (currentCandidate != null)
                return true;

            if(candidates.Count > 0)
            {
                currentCandidate = candidates[0];
                Dispatch(board, currentCandidate);
                return true;
            }

            comboIndex = 0;
            return false;
        }

        private async void Dispatch(IBoard board, IToken candidate)
        {
            ComboResultContext comboResult = candidate.GetCombo(board);

            if (comboResult.result == EComboResult.SUCCESS)
            {
                await board.DispatchCombo(comboResult.tokens, comboIndex);
                comboIndex++;
            }

            candidates.RemoveAll(x => comboResult.tokens.Contains(x));
            currentCandidate = null;
        }
    }
}