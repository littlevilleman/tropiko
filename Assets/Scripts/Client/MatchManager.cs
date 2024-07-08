using Core;
using System.Linq;
using UnityEngine;
using static Core.Events;

namespace Client
{
    public class MatchManager : MonoBehaviour, IMatchBuilder
    {
        [Header("Pools")]
        [SerializeField] private BoardPool boardPool;
        [SerializeField] private PiecePool piecePool;
        [SerializeField] private TokenPool tokenPool;
        [SerializeField] private TokenEffectPool tokenEffectPool;
        [SerializeField] private ScoreWidgetPool scoreWidgetPool;

        private IMatchLobby lobby = new MatchLobby();
        private IMatch match;

        public BuildPlayer OnBuildPlayer => BuildPlayer;
        public BuildBoard OnBuildBoard => BuildBoard;
        public BuildToken OnBuildToken => BuildToken;

        public void Launch<T>() where T : IMatchMode
        {
            match = lobby.Build(this, GameManager.Instance.Config.GetMatchConfig<T>());
            match.OnLaunch += OnLaunchMatch;
            match.OnClose += OnCloseMatch;
            match.Launch();
        }

        private void Update()
        {
            match?.Update(Time.deltaTime);
        }

        private void OnLaunchMatch(IMatch match)
        {
            if(match is Match<IArcadeMatchMode>)
                GameManager.Instance.UI.DisplayScreen<ArcadeMatchScreen>(match, scoreWidgetPool);
            else
                GameManager.Instance.UI.DisplayScreen<MatchScreen>(match, scoreWidgetPool);

            GameManager.Instance.Audio.PlayGameMusic();
        }

        private void OnCloseMatch()
        {
            GameManager.Instance.UI.DisplayScreen<HomeScreen>();
        }

        private void BuildPlayer(IPlayer player)
        {
            //player.OnDefeat += OnDefeatPlayer;
        }

        public void BuildBoard(IBoard board)
        {
            BoardBehavior boardBhv = boardPool.Pull(transform);
            boardBhv.Setup(board/*, piecePool*/);
            boardBhv.onRecycle += boardPool.Recycle;

            BoardLocator.UpdateBoardsLocation(GetComponentsInChildren<BoardBehavior>());
        }

        public void BuildToken(IToken token, IBoard board)
        {
            TokenBehavior tokenBhv = tokenPool.Pull(GetBoardTransform(board));
            tokenBhv.Setup(token, tokenEffectPool);
            tokenBhv.onRecycle += tokenPool.Recycle;
        }

        private Transform GetBoardTransform(IBoard board)
        {
            return GetComponentsInChildren<BoardBehavior>().Where(x => x.Board == board).FirstOrDefault().transform;
        }
    }
}