using Core;
using DG.Tweening;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public class PieceBehavior : MonoBehaviour, IPoolable<PieceBehavior>
    {
        [SerializeField] private PiecePushEffect pushEffect;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private Transform mask;
        [SerializeField] private Color color;

        public event RecyclePoolable<PieceBehavior> onRecycle;
        private IPiece piece;

        public void Setup(IPiece pieceSetup)
        {
            piece = pieceSetup;

            piece.OnLocate += OnLocatePiece;
            piece.OnDispose += OnDispose;

            transform.localPosition = new Vector2(6, 11);
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
            spriteRenderer.transform.localScale = Vector3.one * 2f;
            mask.transform.localScale = Vector3.zero;// DOScale(3, .25f).SetEase(Ease.OutSine);

            spriteRenderer.DOColor(color, .25f).SetEase(Ease.OutSine);
            spriteRenderer.transform.DOScale(1f, .25f).SetEase(Ease.OutSine);
            mask.DOScale(new Vector3(.75f, 2f, 1f), .25f).SetEase(Ease.OutSine);

            //background.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (piece == null)
                return;

            transform.localPosition = piece.Position;
            pushEffect.Play(piece.IsPush);
        }

        private void OnLocatePiece(IPiece piece)
        {
            spriteRenderer.DOColor(Color.white, .125f);
            spriteRenderer.DOColor(new Color(color.r, color.g, color.b, 0f), .125f).SetDelay(.125f);
            GameManager.Instance.Audio.PlaySound(ESound.Locate);
            //background.gameObject.SetActive(false);
            OnDispose();
        }

        private void OnDispose()
        {
            piece.OnLocate -= OnLocatePiece;
            piece.OnDispose -= OnDispose;

            onRecycle?.Invoke(this);
        }
    }
}