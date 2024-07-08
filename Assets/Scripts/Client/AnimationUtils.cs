using DG.Tweening;
using System;
using UnityEngine;

namespace Client
{
    public static class AnimationUtils
    {
        private const float PIECE_SPAWN_TIME = .25F;
        private const float PIECE_STANDBY_TIME = .5F;
        private const float PIECE_LOCATE_TIME = .25F;

        private const float TOKEN_SPAWN_TIME = .25F;

        private static Vector3 PIECE_SPAWN_SCALE = new Vector3(.75f, 2f, 1f);
        private static Color PIECE_HIGHLIGHT_COLOR = new Color(0.7803922f, 0.7098039f, 0.5960785f, 1f);

        private static Vector3 PIECE_PUSH_OFFSET = new Vector3(0f, 2.5f, 0f);
        private static Color PIECE_PUSH_COLOR = new Color(0.1490196f, 0.3333333f, 0.372549f, 1f);
        private static Color PIECE_BACKGROUND_COLOR = new Color(0.1490196f, 0.3333333f, 0.372549f, 1f);

        public static Color GetFadeColor(Color color, float fade)
        {
            return new Color(color.r, color.g, color.b, fade);
        }

        public static Sequence GetPieceSpawnSequence(SpriteRenderer pieceBorder, Transform spawnFader, bool preview = false, TweenCallback onComplete = null)
        {
            pieceBorder.color = preview ? GetFadeColor(PIECE_PUSH_COLOR, 0f) : GetFadeColor(PIECE_HIGHLIGHT_COLOR, 0f);
            pieceBorder.transform.localScale = Vector3.one * 2f;
            spawnFader.transform.localScale = Vector3.zero;

            return DOTween.Sequence()
                .Insert(0, pieceBorder.DOColor(preview ? PIECE_PUSH_COLOR : PIECE_HIGHLIGHT_COLOR, PIECE_SPAWN_TIME).SetEase(Ease.OutSine))
                .Insert(0, pieceBorder.transform.DOScale(1f, PIECE_SPAWN_TIME).SetEase(Ease.OutSine))
                .Insert(0, spawnFader.DOScale(PIECE_SPAWN_SCALE, PIECE_SPAWN_TIME).SetEase(Ease.OutSine))
                .OnComplete(onComplete).Play();
        }

        public static Sequence GetPieceStandbySequence(SpriteRenderer pieceBorder, TweenCallback onUpdate)
        {
            return DOTween.Sequence()
                .Insert(0, pieceBorder.DOColor(GetFadeColor(PIECE_HIGHLIGHT_COLOR, .1f), PIECE_STANDBY_TIME).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine))
                .OnUpdate(onUpdate).Play();
        }

        public static Sequence GetPieceLocateSequence(SpriteRenderer pieceBorder, TweenCallback onComplete)
        {
            return DOTween.Sequence()
                .Append(pieceBorder.DOColor(Color.white, PIECE_LOCATE_TIME / 2f))
                .Append(pieceBorder.DOColor(GetFadeColor(PIECE_HIGHLIGHT_COLOR, 0f), PIECE_LOCATE_TIME / 2f))
                .OnComplete(onComplete)
                .Play();
        }

        public static Sequence GetPiecePushSequence(Transform transform, SpriteRenderer fader, TweenCallback onComplete)
        {    
            return DOTween.Sequence()
            .Append(transform.DOLocalMove(PIECE_PUSH_OFFSET, .125f)).SetEase(Ease.InSine)
            .Append(fader.DOColor(PIECE_PUSH_COLOR, .25f).SetEase(Ease.OutCubic).OnComplete(onComplete));
        }

        public static Sequence GetStopSequence(Transform transform, SpriteRenderer fader, TweenCallback onComplete)
        {
            return DOTween.Sequence().Pause()
            .Append(transform.DOLocalMove(Vector2.zero, .25f)).SetEase(Ease.InSine)
            .Append(fader.DOColor(GetFadeColor(PIECE_PUSH_COLOR, .5f), .25f).SetEase(Ease.InCubic).OnComplete(onComplete));
        }

        public static Sequence GetTokenSpawnSequence(SpriteRenderer tokenRenderer, SpriteRenderer borderRenderer, Color color, Color borderColor)
        {
            tokenRenderer.color = GetFadeColor(color, 0f);
            borderRenderer.color = GetFadeColor(borderColor, 0f);
            tokenRenderer.transform.localScale = Vector3.zero;


            return DOTween.Sequence()
                .Insert(0, borderRenderer.DOColor(borderColor, TOKEN_SPAWN_TIME).SetEase(Ease.OutSine))
                .Insert(0, tokenRenderer.DOColor(color, TOKEN_SPAWN_TIME).SetEase(Ease.OutSine))
                .Insert(0, tokenRenderer.transform.DOScale(1f, TOKEN_SPAWN_TIME).SetEase(Ease.OutSine))
                .Play();
        }
    }
}
