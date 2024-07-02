using Core;
using UnityEngine;

namespace Client
{
    public class PiecePreviewBehavior : MonoBehaviour
    {
        [SerializeField] private TokenBehavior[] tokens;

        private IToken[,] piece;

        public void Setup(IToken[,] pieceSetup)
        {
            piece = pieceSetup;

            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i].Setup(piece[0, i]);
            }

            gameObject.SetActive(true);
        }
    }
}