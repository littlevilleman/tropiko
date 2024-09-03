using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public interface ITombDispatcher
    {
        public void AddCandidates(IBoard board, float count);
        public bool TryDispatch(IBoard board, MatchContext context);
    }

    public class TombDispatcher : ITombDispatcher
    {
        private IToken currentCandidate;
        private float candidateStack = 0;

        public bool TryDispatch(IBoard board, MatchContext context)
        {
            if (currentCandidate != null)
                return true;

            if (candidateStack >= 1)
            {
                currentCandidate = board.GetRandomTokens(1, board.Tombs)[0];
                Dispatch(board);
                return true;
            }

            return false;
        }

        public void AddCandidates(IBoard board, float count)
        {
            candidateStack += count;
            Debug.Log("Board - Stack Tombs - " + this + " - " + count);
        }

        private async void Dispatch(IBoard board)
        {
            currentCandidate.Entomb(board, UnityEngine.Random.Range(1, 4));
            await UniTask.Delay(TimeSpan.FromSeconds(.25f));

            candidateStack--;
            currentCandidate = null;
            Debug.Log("Board - Dispatch Tombs - " + this + " - " + currentCandidate);
        }
    }
}