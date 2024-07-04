using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class PiecePushEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Sequence pushSequence;
        private Sequence stopSequence;

        public void Play(bool play = true)
        {
            if (play)
            {
                if (stopSequence != null)
                {
                    stopSequence.Kill();
                    stopSequence = null;
                }

                if (pushSequence == null)
                    pushSequence = AnimationUtils.GetPiecePushSequence(transform, spriteRenderer, OnCompletePush);
            }
            else
            {
                if (pushSequence != null)
                {
                    pushSequence.Kill();
                    pushSequence = null;
                }

                if (stopSequence == null)
                    stopSequence = AnimationUtils.GetStopSequence(transform, spriteRenderer, OnCompleteStop);
            }
        }

        private void OnCompletePush()
        {
            pushSequence = null;
        }

        private void OnCompleteStop()
        {
            stopSequence = null;
        }
    }
}