using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IComboDispatcher
    {
        public event DispatchCombo OnDispatch;
        void AddCandidate(IToken token);
        void Update(IBoard board);

        bool IsClear { get; }
    }

    public class ComboDispatcher : IComboDispatcher
    {
        public event DispatchCombo OnDispatch;
        public bool IsClear => candidates.Count == 0;

        private List<IToken> candidates = new List<IToken>();
        private IToken CurrentCandidate;
        private int comboIndex = 0;

        public void AddCandidate(IToken token)
        {
            candidates.Add(token);
        }

        public void Update(IBoard board)
        {
            if (CurrentCandidate != null)
                return;

            CurrentCandidate = candidates[0];
            List<IToken> comboStack = GetComboTokens(board, CurrentCandidate);
            Dispatch(board, comboStack);
        }

        private async void Dispatch(IBoard board, List<IToken> comboStack)
        {
            if (comboStack.Count >= 3)
            {
                OnDispatch?.Invoke(comboStack, comboIndex);
                comboIndex++;
                await Task.Delay(250);
            }

            candidates.RemoveAll(x => comboStack.Contains(x));
            CurrentCandidate = null;

            if (candidates.Count == 0)
                comboIndex = 0;
        }

        private List<IToken> GetComboTokens(IBoard board, IToken sourceToken)
        {
            List<IToken> list = new List<IToken> { sourceToken };

            List<IToken> horizontal = new List<IToken>();
            List<IToken> vertical = new List<IToken>();
            List<IToken> diagonalLeft = new List<IToken>();
            List<IToken> diagonalRight = new List<IToken>();

            vertical.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, Vector2Int.down));
            vertical.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, Vector2Int.up));

            horizontal.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, Vector2Int.left));
            horizontal.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, Vector2Int.right));

            diagonalLeft.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, new Vector2Int(-1, 1)));
            diagonalLeft.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, new Vector2Int(1, -1)));

            diagonalRight.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, new Vector2Int(-1, -1)));
            diagonalRight.AddRange(GetLineNeighbours(board, sourceToken.Type, sourceToken.Location, new Vector2Int(1, 1)));

            if (horizontal.Count >= 2)
                list.AddRange(horizontal);

            if (vertical.Count >= 2)
                list.AddRange(vertical);

            if (diagonalRight.Count >= 2)
                list.AddRange(diagonalRight);

            if (diagonalLeft.Count >= 2)
                list.AddRange(diagonalLeft);

            return list;
        }

        private static List<IToken> GetLineNeighbours(IBoard board, ETokenType type, Vector2Int location, Vector2Int direction)
        {
            List<IToken> lineNeighbours = new List<IToken>();
            location += direction;

            while (location.x >= 0 && location.y >= 0 && location.x < board.Size.x && location.y < board.Size.y)
            {
                IToken neighbourToken = board.GetToken(location.x, location.y);

                if (neighbourToken == null || neighbourToken.Type != type)
                    break;

                lineNeighbours.Add(neighbourToken);
                location += direction;
            }

            return lineNeighbours;
        }
    }
}