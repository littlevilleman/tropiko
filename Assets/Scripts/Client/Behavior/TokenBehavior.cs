using Config;
using Core;
using DG.Tweening;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public class TokenBehavior : MonoBehaviour, IPoolable<TokenBehavior>
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer tokenRenderer;
        [SerializeField] private SpriteRenderer backgroundRenderer;

        public event RecyclePoolable<TokenBehavior> onRecycle;

        private IToken token;
        private Tween movementTween;
        private TokenEffectPool breakEffectPool;

        public void Setup(IToken tokenSetup)
        {
            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(tokenSetup.Type);
            animator.runtimeAnimatorController = config.animator;
            tokenRenderer.color = config.color;

            //transform.position = location;
            gameObject.SetActive(true);
        }

        public void Setup(IToken tokenSetup, TokenEffectPool breakEffectPoolSetup)
        {
            token = tokenSetup;
            token.OnLocate += OnLocate;
            token.OnBreak += OnBreak;
            token.OnDispose += OnDispose;

            breakEffectPool = breakEffectPoolSetup;

            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(token.Type);
            animator.runtimeAnimatorController = config.animator;
            tokenRenderer.color = config.color;

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (movementTween != null)
                return;

            if (token != null)
                transform.localPosition = token.Position;
        }

        private void OnLocate(IToken token, bool falling = false)
        {
            if (falling)
                movementTween = transform.DOLocalMoveY(token.Location.y, .25f).OnComplete(OnPushComplete);

            animator.SetTrigger("Locate");
        }

        private void OnBreak(IToken token)
        {
            Color tokenColor = tokenRenderer.color;
            Color bgColor = backgroundRenderer.color;
            tokenRenderer.DOColor(Color.white, .125f).SetLoops(2, LoopType.Yoyo).OnComplete(OnBreakComplete);
            //backgroundRenderer.DOColor(Color.white, .125f).SetLoops(2, LoopType.Yoyo); 
            //transform.DOScale(1.125F, .120f).SetLoops(2, LoopType.Yoyo);
        }

        private void OnPushComplete()
        {
            movementTween = null;
        }

        private void OnBreakComplete()
        {
            Color color = GameManager.Instance.Config.GetTokenConfig(token.Type).color;
            TokenEffectBehavior effect = breakEffectPool.Pull(null);
            effect.onRecycle += breakEffectPool.Recycle;
            effect.PlayBreak(transform.position, color);

            animator.SetTrigger("Break");
            OnDispose();
        }
        private void OnDispose()
        {
            movementTween = null;
            token.OnLocate -= OnLocate;
            token.OnBreak -= OnBreak;
            token.OnDispose -= OnDispose;

            onRecycle?.Invoke(this);
        }
    }
}
