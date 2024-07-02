using Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;

        [Header("Manager")]
        [SerializeField] public SoundManager Audio;
        [SerializeField] public ConfigManager Config;
        [SerializeField] public UIManager UI;

        [Header("Pooling")]
        [SerializeField] private BoardPool boardPool;
        [SerializeField] private PiecePool piecePool;
        [SerializeField] private TokenPool tokenPool;
        [SerializeField] private TokenEffectPool tokenEffectPool;
        [SerializeField] private ScoreWidgetPool scoreWidgetPool;

        [SerializeField] private BoardLocator world;

        private IMatchFactory matchFactory;
        private IMatch match;
        private List<BoardBehavior> boards = new List<BoardBehavior>();

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            instance = this;
        }

        void Start()
        {
            matchFactory = new MatchFactory(OnBuildPlayer, OnBuildBoard, OnBuildToken);
            UI.DisplayScreen<HomeScreen>();
        }

        public void LaunchMatch(EMatchMode mode)
        {
            match = matchFactory.BuildMultiplayerMatch();
            match.OnLaunch += OnLaunchMatch;
            match.OnClose += OnCloseMatch;
            match.Launch();
        }

        private void OnCloseMatch()
        {
            UI.DisplayScreen<HomeScreen>();
        }

        private void OnLaunchMatch(IMatch match)
        {
            UI.DisplayScreen<MatchScreen>(match, scoreWidgetPool);
            Audio.PlayGameMusic();
        }

        private void OnBuildPlayer(IPlayer player)
        {
            player.OnDefeat += OnDefeatPlayer;
        }

        private void OnBuildBoard(IBoard board)
        {
            BoardBehavior boardBhv = boardPool.Pull(world.transform);
            boardBhv.Setup(board, piecePool);
            boardBhv.onRecycle += boardPool.Recycle;

            boards.Add(boardBhv);
            world.UpdateBoardsLocation();
        }

        private void OnBuildToken(IToken token, IBoard board)
        {
            TokenBehavior tokenBhv = tokenPool.Pull(GetBoardTransform(board));
            tokenBhv.Setup(token, tokenEffectPool);
            tokenBhv.onRecycle += tokenPool.Recycle;
        }

        private void OnDefeatPlayer(IPlayer player)
        {
            player.OnDefeat -= OnDefeatPlayer;

            BoardBehavior boardBhv = boards.Where(x => x.Board == player.Board).FirstOrDefault();
            boards.Remove(boardBhv);

            if (boards.Count == 0)
                match.Close();
        }

        private Transform GetBoardTransform(IBoard board)
        {
            return boards.Where(x => x.Board == board).FirstOrDefault().transform;
        }
    }
}