using Core.Map;
using System.Collections.Generic;

namespace Core
{
    public class Events
    {
        //Match Building
        public delegate void BuildPlayer(IPlayer player);
        public delegate void BuildBoard(IBoard board);
        public delegate void BuildToken(IToken token, IBoard board);
        public delegate void DisposePiece();
        public delegate void DisposeToken();

        //Match
        public delegate void LaunchMatch(IMatch match);
        public delegate void PauseMatch(IMatch match);
        public delegate void CloseMatch();

        //Match - Core
        public delegate void TakePiece(IPiece piece, IToken[,] nextPiece);
        public delegate void CollidePiece(IPiece piece);
        public delegate void LocatePiece(IPiece piece);
        public delegate void LocateToken(IToken token);
        public delegate void FallToken(IToken token);
        public delegate void BreakToken(IToken token, int remaining = 0);
        public delegate void EntombToken(IBoardMap board, IToken token, int count);
        public delegate void DispatchCombo(List<IToken> token, int index);
        public delegate void OverflowBoard();

        //Match - Campaign stage
        public delegate void LaunchStage(ICampaignStageConfig stage);
        public delegate void CompleteStage(ICampaignStageConfig stage);

        //Match - Player
        public delegate void LevelUp(int level);
        public delegate void ReceiveScore(IPlayer player, long score);
        public delegate void DefeatPlayer(IPlayer player);
    }
}
