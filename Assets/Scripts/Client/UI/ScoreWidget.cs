using DG.Tweening;
using TMPro;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public enum ECombination
    {
        Combox1, Combox2, Combox3, Combox4, Combox5
    }

    public class ScoreWidget : MonoBehaviour, IPoolable<ScoreWidget>
    {
        [SerializeField] private TMP_Text scoreLabel;
        [SerializeField] private Color combox1color;
        [SerializeField] private Color combox2color;
        [SerializeField] private Color combox3color;
        [SerializeField] private Color combox4color;
        [SerializeField] private Color combox5color;

        private Sequence displaySequence;

        public event RecyclePoolable<ScoreWidget> onRecycle;

        public void Display(Vector3 position, long score, ECombination combo = ECombination.Combox1)
        {
            transform.localPosition = position;
            transform.localScale = Vector3.zero;
            scoreLabel.color = combox1color;
            scoreLabel.text = score.ToString();

            transform.DOLocalMoveY(position.y + 14f, 1.5f).Play().OnComplete(OnComplete);
            transform.DOScale(1f, .25f).Play();
            transform.DOScale(1.25f, .25f).SetDelay(.25f).SetLoops(2, LoopType.Yoyo).Play();
            scoreLabel.DOColor(Color.white, .25f).SetLoops(2, LoopType.Yoyo).Play();
            scoreLabel.DOColor(GetAlphaColor(combox1color), .5f).SetDelay(1f).Play();

            gameObject.SetActive(true);
        }

        private void OnComplete()
        {
            displaySequence = null;
            onRecycle?.Invoke(this);
        }

        private Color GetAlphaColor(Color color, float alpha = 0f)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }

}