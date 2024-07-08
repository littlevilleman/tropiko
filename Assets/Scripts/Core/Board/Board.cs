using Core.Map;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoard : IBoardMap
    {
        public event DispatchCombo OnDispatchCombo;
        public event OverflowBoard OnOverflow;

        public IPieceHandler PieceHandler { get; }
        public ITombDispatcher TombDispatcher { get; }
        public IComboDispatcher ComboDispatcher { get; }
        public void Update<T>(MatchContext<T> context) where T : IMatchMode;
        public Task DispatchCombo(List<IToken> comboStack, int index);
    }

    public abstract class MatchContext<T> where T : IMatchMode
    {
        public IMatchConfig<T> config;
        public IPieceFactory pieceFactory;
        public int level = 0;
        public float time = 0f;
        public float speed = 1f;
        public float collisionTime = .5f;
    }

    public class ArcadeMatchContext : MatchContext<IArcadeMatchMode>
    {
    }

    public class Board : TokenMap, IBoard
    {
        public event DispatchCombo OnDispatchCombo;
        public event OverflowBoard OnOverflow;
        public IPieceHandler PieceHandler { get; private set; }
        public ITombDispatcher TombDispatcher { get; private set; }
        public IComboDispatcher ComboDispatcher { get; private set; }

        public Board(Vector2Int sizeSetup, IPieceHandler pieceHandlerSetup)
        {
            Size = sizeSetup;
            Tokens = new Token[Size.x, Size.y];
            PieceHandler = pieceHandlerSetup;
            TombDispatcher = new TombDispatcher();
            ComboDispatcher = new ComboDispatcher();
        }

        public void Update<T>(MatchContext<T> context) where T : IMatchMode
        {
            if (ComboDispatcher.TryDispatch(this))
                return;

            if (PieceHandler.TryUpdate(this, context))
                return;

            if (TombDispatcher.TryDispatch(this))
                return;

            PieceHandler.SwitchPiece(this, context);
        }

        public void TryLocatePiece(IPiece piece)
        {
            if (IsOverflowLocation(this, piece.Location))
            {
                OnOverflow?.Invoke();
                Debug.Log("Board - Overflow - " + this);
                Dispose();
                return;
            }

            PieceHandler.LocatePiece(this);
        }

        public virtual void LocateToken(IToken token)
        {
            Tokens[token.Location.x, token.Location.y] = token;
            ComboDispatcher?.AddCandidate(token);
        }

        public async Task DispatchCombo(List<IToken> comboStack, int index)
        {
            OnDispatchCombo?.Invoke(comboStack, index);
            Debug.Log("Board - Combo Dispatched - " + comboStack.ToString());

            await Task.Delay(250);

            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Tokens[x, y]?.Fall(this);

            await Task.Delay(250);
        }

        public override void Dispose()
        {
            base.Dispose();
            PieceHandler.Dispose();
        }
    }
}