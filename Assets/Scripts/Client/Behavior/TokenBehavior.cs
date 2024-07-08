using Config;
using Core;
using Core.Map;
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
        [SerializeField] private SpriteRenderer borderRenderer;

        public event RecyclePoolable<TokenBehavior> onRecycle;

        private IToken token;
        private Tween movementTween;
        private TokenEffectPool breakEffectPool;
        private Sequence spawnSequence;

        public void Setup(IToken tokenSetup)
        {
            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(tokenSetup.Type);
            animator.runtimeAnimatorController = config.animator;
            tokenRenderer.color = config.color;

            gameObject.SetActive(true);
        }

        public void Setup(IToken tokenSetup, TokenEffectPool breakEffectPoolSetup)
        {
            token = tokenSetup;
            token.OnLocate += OnLocate;
            token.OnFall += OnFall;
            token.OnBreak += OnBreak;
            token.OnEntomb += OnEntomb;
            token.OnDispose += OnDispose;

            breakEffectPool = breakEffectPoolSetup;


            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(token.Type);
            animator.runtimeAnimatorController = config.animator;

            spawnSequence = AnimationUtils.GetTokenSpawnSequence(tokenRenderer, borderRenderer, config.color, config.borderColor).Play();

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (movementTween != null)
                return;

            if (token != null)
                transform.localPosition = token.Position;
        }

        private void OnFall(IToken token)
        {
            movementTween = transform.DOLocalMoveY(token.Location.y, .25f).OnComplete(OnPushComplete);
        }


        private void OnLocate(IToken token)
        {
            transform.localPosition = token.Position;
            animator.SetTrigger("Locate");
        }

        private void OnBreak(IToken t, int remaining = 0)
        {
            if (remaining > 0)
            {
                tokenRenderer.DOColor(Color.white, .125f).SetLoops(2, LoopType.Yoyo).OnComplete(() => OnTombBreakComplete(remaining));
                GameManager.Instance.Audio.PlaySound(ESound.BreakTomb);
                return;
            }

            borderRenderer.color = GameManager.Instance.Config.GetTokenConfig(token.Type).borderColor;
            tokenRenderer.DOColor(Color.white, .125f).SetLoops(2, LoopType.Yoyo).OnComplete(OnBreakComplete);
        }

        private void OnEntomb(IBoardMap board, IToken token, int count)
        {
            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(token.Type);
            animator.runtimeAnimatorController = config.animator;
            animator.SetInteger("Count", count);

            GameManager.Instance.Audio.PlaySound(ESound.GenerateTomb);
            spawnSequence = AnimationUtils.GetTokenSpawnSequence(tokenRenderer, borderRenderer, config.color, config.borderColor);
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

        private void OnTombBreakComplete(int remaining)
        {
            TokenEffectBehavior effect = breakEffectPool.Pull(null);
            effect.onRecycle += breakEffectPool.Recycle;
            effect.PlayBreak(transform.position, tokenRenderer.color);

            TokenConfig config = GameManager.Instance.Config.GetTokenConfig(token.Type);
            animator.runtimeAnimatorController = config.animator;
            animator.SetInteger("Count", remaining);

            spawnSequence = AnimationUtils.GetTokenSpawnSequence(tokenRenderer, borderRenderer, config.color, config.borderColor);

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
