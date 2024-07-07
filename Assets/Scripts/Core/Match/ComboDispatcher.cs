using System.Collections.Generic;
using System.Threading.Tasks;
using static Core.Events;

namespace Core
{
    public interface IComboDispatcher
    {
        public event DispatchCombo OnDispatch;
        public void AddCandidate(IToken token);
        public bool TryDispatch(IBoard board);
    }

    public class ComboDispatcher : IComboDispatcher
    {
        public event DispatchCombo OnDispatch;

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
            ComboResultContext comboResult = candidate.Perform(board);

            if (comboResult.result == EComboResult.SUCCESS)
            {
                OnDispatch?.Invoke(comboResult.tokens, comboIndex);
                comboIndex++;

                await Task.Delay(250);
            }

            candidates.RemoveAll(x => comboResult.tokens.Contains(x));
            currentCandidate = null;
        }
    }
}