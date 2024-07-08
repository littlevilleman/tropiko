using Core.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TypeComboStrategy : IComboStrategy
    {
        public ComboResultContext Perform(IToken token, IBoardMap board)
        {
            List<IToken> comboTokens = GetComboTokens(token, board);

            foreach (IToken comboToken in comboTokens)
                comboToken.Break(board);

            return new ComboResultContext
            {
                result = EComboResult.SUCCESS,
                tokens = comboTokens
            };
        }

        private List<IToken> GetComboTokens(IToken token, IBoardMap board)
        {
            List<IToken> comboTokens = new List<IToken>() { token };

            board.GetToken(token.Location.x, token.Location.y - 1, out IToken targetToken);
            if (targetToken != null)
            {
                List<IToken> typeTokens = new List<IToken>();
                typeTokens = TakeTypeTokens(board, targetToken);
                typeTokens = TakeRandomDraft(typeTokens, 3);
                comboTokens.AddRange(typeTokens);
            }

            return comboTokens;
        }

        private List<IToken> TakeTypeTokens(IBoardMap board, IToken targetToken)
        {
            List<IToken> comboTokens = new List<IToken>() { targetToken };

            for (int x = 0; x < board.Size.x; x++)
            {
                for (int y = 0; y < board.Size.y; y++)
                {
                    board.GetToken(x, y, out IToken t);
                    if (t != null && t.Type == targetToken.Type && !comboTokens.Contains(t))
                        comboTokens.Add(t);
                }
            }

            return comboTokens;
        }

        private List<IToken> TakeRandomDraft(List<IToken> comboTokens, int count)
        {
            List<IToken> randomDraft = new List<IToken>() { };
            while (count > 0 && comboTokens.Count > 0)
            {
                IToken random = comboTokens[Random.Range(0, comboTokens.Count)];
                randomDraft.Add(random);
                comboTokens.Remove(random);
                count--;
            }

            return randomDraft;
        }
    }
}