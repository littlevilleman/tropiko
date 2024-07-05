using Core.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IComboStrategy
    {
        public ComboResultContext Perform(IToken token, IBoardMap board);
    }

    public enum EComboResult
    {
        FAILED, SUCCESS
    }

    public class ComboResultContext
    {
        public EComboResult result;
        public List<IToken> tokens;
    }

    public class LineComboStrategy : IComboStrategy
    {
        public ComboResultContext Perform(IToken token, IBoardMap board)
        {
            List<IToken> comboTokens = new List<IToken>() { token };

            foreach (Vector2Int[] axis in MapUtils.Directions)
            {
                List<IToken> neighbours = new List<IToken>();
                foreach (Vector2Int direction in axis)
                    neighbours.AddRange(GetLineNeighbours(board, token, direction));

                if (neighbours.Count >= 2)
                    comboTokens.AddRange(neighbours);
            }

            return new ComboResultContext()
            {
                result = comboTokens.Count > 1 ? EComboResult.SUCCESS : EComboResult.FAILED,
                tokens = comboTokens
            };
        }

        private static List<IToken> GetLineNeighbours(ITokenMap board, IToken source, Vector2Int direction)
        {
            List<IToken> lineNeighbours = new List<IToken>();
            Vector2Int location = source.Location + direction;

            while (board.GetToken(location.x, location.y, out IToken neighbour) && neighbour.Type == source.Type)
            {
                lineNeighbours.Add(neighbour);
                location += direction;
            }

            return lineNeighbours;
        }
    }
}