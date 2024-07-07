using System.Collections.Generic;

namespace Core
{
    public class Events
    {
        public delegate void ArcadeMatchLevelUp(int level);
        public delegate void LaunchMatch(IMatch match); 
        public delegate void PauseMatch(IMatch match); 
        public delegate void CloseMatch();

        //Match Building
        public delegate void BuildPlayer(IPlayer player);
        public delegate void BuildBoard(IBoard board);
        public delegate void BuildToken(IToken token, IBoard board);
        public delegate void DisposePiece();
        public delegate void DisposeToken();

        //Match
        public delegate void TakePiece(IPiece piece, IToken[,] nextPiece);
        public delegate void CollidePiece(IPiece piece);
        public delegate void LocatePiece(IPiece piece);
        public delegate void LocateToken(IToken token, bool fallen = false);
        public delegate void BreakToken(IToken token);
        public delegate void DispatchCombo(List<IToken> token, int index);
        public delegate void OverflowBoard();

        public delegate void PowerRouletteResult();

        //Match Player
        public delegate void ReceiveScore(IPlayer player, long score);
        public delegate void DefeatPlayer(IPlayer player);
    }
}
