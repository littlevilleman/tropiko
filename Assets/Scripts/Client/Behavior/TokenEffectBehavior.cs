using UnityEngine;
using static Client.Events;
using static UnityEngine.ParticleSystem;

namespace Client
{
    public class TokenEffectBehavior : MonoBehaviour, IPoolable<TokenEffectBehavior>
    {
        public event RecyclePoolable<TokenEffectBehavior> onRecycle;
        [SerializeField] private ParticleSystem breakParticles;

        public void PlayBreak(Vector3 playPosition, Color color)
        {
            transform.position = playPosition + .5f * Vector3.right;

            MainModule main = breakParticles.main;
            main.startColor = color;
            gameObject.SetActive(true);
        }

        private void OnParticleSystemStopped()
        {
            onRecycle?.Invoke(this);
        }
    }
}
