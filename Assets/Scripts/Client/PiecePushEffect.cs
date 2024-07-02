using DG.Tweening;
using UnityEngine;

namespace Client
{
    public class PiecePushEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Vector2 startLocation = Vector3.zero;
        [SerializeField] private Vector2 endLocation = new Vector3(0f, 2.5f, 0f);

        [SerializeField] private Color defaultColor = new Color(1f, 1f, 1f, 1f);
        [SerializeField] private Color fadeColor = new Color(1f, 1f, 1f, 0f);

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
                    pushSequence = GetPushSequence().Play();
            }
            else
            {
                if (pushSequence != null)
                {
                    pushSequence.Kill();
                    pushSequence = null;
                }

                if (stopSequence == null)
                    stopSequence = GetStopSequence().Play();
            }
        }

        private Sequence GetPushSequence()
        {
            return DOTween.Sequence().Pause()
            .Append(transform.DOLocalMove(endLocation, .125f)).SetEase(Ease.InSine)
            .Append(spriteRenderer.DOColor(defaultColor, .25f).SetEase(Ease.OutCubic).OnComplete(OnCompletePush));
        }

        private Sequence GetStopSequence()
        {
            return DOTween.Sequence().Pause()
            .Append(transform.DOLocalMove(startLocation, .25f)).SetEase(Ease.InSine)
            .Append(spriteRenderer.DOColor(fadeColor, .25f).SetEase(Ease.InCubic).OnComplete(OnCompleteStop));
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