using Core;
using DG.Tweening;
using UnityEngine;
using static Client.Events;

namespace Client
{
    public class PieceBehavior : MonoBehaviour, IPoolable<PieceBehavior>
    {
        //[SerializeField] private PiecePushEffect pushEffect;
        [SerializeField] private SpriteRenderer pieceBorder;
        [SerializeField] private Transform spawnFader;

        public event RecyclePoolable<PieceBehavior> onRecycle;
        private IPiece piece;

        private Sequence spawnSequence;
        private Sequence standbySequence;
        private Sequence locateSequence;

        public void Setup(IPiece pieceSetup)
        {
            piece = pieceSetup;

            piece.OnLocate += OnLocate;
            piece.OnCollide += OnCollidePiece;
            piece.OnDispose += OnDispose;

            transform.localPosition = piece.Position;

            spawnSequence = AnimationUtils.GetPieceSpawnSequence(pieceBorder, spawnFader, false, OnCompleteSpawnAnimation);

            gameObject.SetActive(true);
        }

        private void Update()
        {
            if (piece == null)
                return;

            transform.localPosition = piece.Position;
            //pushEffect.Play(piece.IsPush);
        }

        private void OnCollidePiece(IPiece piece)
        {
            GameManager.Instance.Audio.PlaySound(ESound.Locate);
        }

        private void OnLocate(IPiece piece)
        {
            standbySequence.Kill();
            standbySequence = null;

            locateSequence = AnimationUtils.GetPieceLocateSequence(pieceBorder);
        }

        private void OnCompleteSpawnAnimation()
        {
            spawnSequence.Kill();
            spawnSequence = null;
            standbySequence = AnimationUtils.GetPieceStandbySequence(pieceBorder, OnUpdate);

            void OnUpdate()
            {
                standbySequence.timeScale = Mathf.Clamp(10 * (1f - piece.CollisionTime / .5f), 1f, 10f);
            }
        }

        private void OnDispose()
        {
            piece.OnLocate -= OnLocate;
            piece.OnDispose -= OnDispose;
            piece.OnCollide -= OnCollidePiece;

            locateSequence.Kill();
            locateSequence = null;

            //onRecycle?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

}