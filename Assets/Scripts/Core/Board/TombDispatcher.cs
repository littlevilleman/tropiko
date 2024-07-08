using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public interface ITombDispatcher
    {
        public void AddCandidates(int count);
        public bool TryDispatch(IBoard board);
    }

    public class TombDispatcher : ITombDispatcher
    {
        private List<IToken> candidates = new List<IToken>();
        private IToken currentCandidate;

        private int stack;

        public bool TryDispatch(IBoard board)
        {
            foreach (IToken token in board.GetRandomTokens(stack, candidates))
                candidates.Add(token);

            stack = 0;

            if (currentCandidate != null)
                return true;

            if (candidates.Count > 0)
            {
                currentCandidate = candidates[0];
                Dispatch(board, currentCandidate);
                return true;
            }

            return false;
        }

        public void AddCandidates(int count)
        {
            stack += count;
            Debug.Log("Board - Stack Tombs - " + this + " - " + count);
        }

        private async void Dispatch(IBoard board, IToken candidate)
        {
            candidate.Entomb(board, Random.Range(1, 4));
            await Task.Delay(250);

            candidates.Remove(candidate);
            currentCandidate = null;
            Debug.Log("Board - Dispatch Tombs - " + this + " - " + candidate);
        }
    }
}