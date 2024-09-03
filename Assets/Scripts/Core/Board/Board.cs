using Core.Map;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
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
        public int TokensCount { get; }
        public List<IToken> Tombs { get; }
        public void Update(MatchContext context);
        public UniTask DispatchCombo(List<IToken> comboStack, int index);
    }

    public class Board : TokenMap, IBoard
    {
        public event DispatchCombo OnDispatchCombo;
        public event OverflowBoard OnOverflow;
        public IPieceHandler PieceHandler { get; private set; }
        public ITombDispatcher TombDispatcher { get; private set; }
        public IComboDispatcher ComboDispatcher { get; private set; }
        public int TokensCount => GetTokensCount();
        public List<IToken> Tombs => GetTombTokens();

        private int GetTokensCount()
        {
            int count = 0;
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    count += GetToken(x, y) != null ? 1 : 0;

            return count;
        }

        private List<IToken> GetTombTokens()
        {
            List<IToken> tombs = new List<IToken>();
            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                {
                    var token = GetToken(x, y);
                    if (token?.Type == ETokenType.TOMB)
                        tombs.Add(token);
                }

            return tombs;
        }

        public Board(Vector2Int sizeSetup, IPieceHandler pieceHandlerSetup)
        {
            Size = sizeSetup;
            Tokens = new Token[Size.x, Size.y];
            PieceHandler = pieceHandlerSetup;
            TombDispatcher = new TombDispatcher();
            ComboDispatcher = new ComboDispatcher();
        }

        public void Update(MatchContext context)
        {
            if (ComboDispatcher.TryDispatch(this))
                return;

            if (TombDispatcher.TryDispatch(this, context))
                return;

            if (PieceHandler.TryUpdate(this, context))
                return;

            PieceHandler.SwitchPiece(this, context);
            TombDispatcher.AddCandidates(this, context.tombs);
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

        public async UniTask DispatchCombo(List<IToken> comboStack, int index)
        {
            OnDispatchCombo?.Invoke(comboStack, index);
            Debug.Log("Board - Combo Dispatched - " + comboStack.ToString());

            await UniTask.Delay(TimeSpan.FromSeconds(.25f));

            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Tokens[x, y]?.Fall(this);

            await UniTask.Delay(TimeSpan.FromSeconds(.25f));
        }

        public override void Dispose()
        {
            base.Dispose();
            PieceHandler.Dispose();
        }
    }
}