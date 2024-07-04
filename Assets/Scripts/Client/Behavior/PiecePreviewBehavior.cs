using Core;
using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class PiecePreviewBehavior : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer pieceBorder;
        [SerializeField] private Transform spawnFader;
        [SerializeField] private TokenBehavior[] tokens;

        private Sequence spawnSequence;

        private IToken[,] piece;

        public void Setup(IToken[,] pieceSetup)
        {
            piece = pieceSetup;

            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i].Setup(piece[0, i]);
            }

            spawnSequence = AnimationUtils.GetPieceSpawnSequence(pieceBorder, spawnFader, true, OnCompleteSpawnAnimation);

            gameObject.SetActive(true);
        }
        private void OnCompleteSpawnAnimation()
        {
            spawnSequence.Kill();
            spawnSequence = null;
        }
    }
}