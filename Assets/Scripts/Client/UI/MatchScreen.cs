using Core;
using System.Linq;
using UnityEngine;

namespace Client
{
    public class MatchScreen : UIScreen
    {
        [SerializeField] private MatchPlayerWidget[] boardWidgets;

        private ScoreWidgetPool scoreWidgetPool;
        private IMatch match;
        
        protected override void OnDisplay(params object[] parameters)
        {
            match = parameters[0] as IMatch;
            scoreWidgetPool = parameters[1] as ScoreWidgetPool;

            for (int i = 0; i < match.Players.Length; i++)
            {
                match.Players[i].OnDefeat += OnPlayerDefeat;
                boardWidgets[i].Display(match.Players[i], scoreWidgetPool);
            }
        }

        private void OnPlayerDefeat(IPlayer player)
        {
            player.OnDefeat -= OnPlayerDefeat;
            boardWidgets.Where(x => x.player == player).FirstOrDefault().Hide();
        }

        protected override void OnClose()
        {
            for (int i = 0; i < boardWidgets.Length; i++)
            {
                boardWidgets[i].Hide();
            }
        }
    }
}
