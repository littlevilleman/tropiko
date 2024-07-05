using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public static class MapUtils
    {
        private static Vector2Int[] Horizontal = { Vector2Int.right, Vector2Int.left };
        private static Vector2Int[] Vertical = { Vector2Int.up, Vector2Int.down };
        private static Vector2Int[] LeftDiagonal = { new Vector2Int(-1, 1), new Vector2Int(1, -1) };
        private static Vector2Int[] RightDiagonal = { new Vector2Int(1, 1), new Vector2Int(-1, -1) };
        public static List<Vector2Int[]> Directions => new List<Vector2Int[]> { Horizontal, Vertical, LeftDiagonal, RightDiagonal };
    }

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

        private bool IsDispatching;
        private int comboIndex = 0;

        public void AddCandidate(IToken token)
        {
            candidates.Add(token);
        }

        public bool TryDispatch(IBoard board)
        {
            if(candidates.Count > 0 && !IsDispatching)
            {
                IsDispatching = true;
                Dispatch(board, candidates[0]);
            }

            comboIndex = 0;
            return IsDispatching;
        }

        private async void Dispatch(IBoard board, IToken candidate)
        {
            ComboResultContext comboResult = candidate.Perform(board);

            if (comboResult.result == EComboResult.SUCCESS)
            {
                OnDispatch?.Invoke(comboResult.tokens, comboIndex);
                comboIndex++;

                await Task.Delay(250 * (comboIndex + 1));
            }

            candidates.RemoveAll(x => comboResult.tokens.Contains(x));
            IsDispatching = false;
        }
    }
}