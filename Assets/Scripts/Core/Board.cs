using Core.Map;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static Core.Events;

namespace Core
{
    public interface IBoard : IBoardMap
    {
        public event OverflowBoard OnOverflow;
        public event TakePiece OnTakePiece;

        public IComboDispatcher ComboDispatcher { get; }
        public void Update(float time, float speed, int level = 0);
        public void MovePiece(bool left = false);
        public void RotatePiece(bool left = false);
        public void PushPiece(bool push);
    }

    public class Board : TokenMap, IBoard
    {
        public event OverflowBoard OnOverflow;
        public event TakePiece OnTakePiece;

        public IComboDispatcher ComboDispatcher { get; protected set; }
        public void MovePiece(bool left = false) => currentPiece?.Move(this, left);
        public void RotatePiece(bool left = false) => currentPiece?.Rotate(this, left);

        private IPieceFactory pieceFactory;
        private IPiece currentPiece;
        private IToken[,] nextPiecePreview;

        public Board(Vector2Int sizeSetup, IPieceFactory pieceFactorySetup)
        {
            Size = sizeSetup;
            Tokens = new Token[Size.x, Size.y];
            pieceFactory = pieceFactorySetup;

            ComboDispatcher = new ComboDispatcher();
            ComboDispatcher.OnDispatch += OnDispatchCombo;
            nextPiecePreview = pieceFactory.GetPiecePreview(0);
        }

        public void Update(float time, float speed, int level = 0)
        {
            if (ComboDispatcher.TryDispatch(this))
                return;

            if (currentPiece != null)
            {
                currentPiece.Update(this, time, speed);
                return;
            }

            currentPiece = pieceFactory.Build(this, nextPiecePreview);
            nextPiecePreview = pieceFactory.GetPiecePreview(level);
            OnTakePiece?.Invoke(currentPiece, nextPiecePreview);
        }

        public void LocatePiece(IPiece piece)
        {
            if (IsOverflowLocation(this, piece.Location))
            {
                OnOverflow?.Invoke();
                Dispose();
                Debug.Log("Board - Overflow - " + piece);
                return;
            }

            piece.Locate(this);
            currentPiece = null;
        }

        public virtual void LocateToken(IToken token)
        {
            Tokens[token.Location.x, token.Location.y] = token;
            ComboDispatcher?.AddCandidate(token);
        }

        public virtual void RemoveToken(Vector2Int location)
        {
            Tokens[location.x, location.y] = null;
        }

        protected async void OnDispatchCombo(List<IToken> comboStack, int index)
        {
            foreach (IToken comboToken in comboStack)
                comboToken.Break(this);

            await Task.Delay(250);

            for (int y = 0; y < Size.y; y++)
                for (int x = 0; x < Size.x; x++)
                    Tokens[x, y]?.Fall(this);

            Debug.Log("Board - Combo Dispatched - " + comboStack.ToString());
        }

        public override void Dispose()
        {
            base.Dispose();
            currentPiece.Dispose();
            Debug.Log("Board - Board Disposed - " + this);
        }

        public void PushPiece(bool push)
        {
            currentPiece?.Push(push);
        }
    }
}