using Core.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class NonComboStrategy : IComboStrategy
    {
        public ComboResultContext Perform(IToken token, IBoardMap board)
        {
            return new ComboResultContext()
            {
                result = EComboResult.FAILED,
                tokens = new List<IToken>() { token }
            };
        }
    }
}