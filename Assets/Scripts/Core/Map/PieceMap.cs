using UnityEngine;

namespace Core.Map
{
    public interface IPieceMap : ITokenMap, ILocatable
    {
        public bool IsPush { get; }
        public void Move(IBoardMap board, bool left);
        public void Rotate(IBoardMap board, bool left);
        public void Push(bool push);
    }

    public abstract class PieceMap : TokenMap, IPieceMap
    {
        public Vector2 Position => Tokens[0, 0].Position;
        public Vector2Int Location => Tokens[0, 0].Location;
        public bool IsPush { get; private set; }

        public void Push(bool push) { IsPush = push; }

        public virtual void Update(IBoard board, float time, float speed)
        {
            Vector2 position = Position + time * speed * Vector2.down;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y].Update(board, position + y * Vector2.up);
        }

        public virtual void Rotate(IBoardMap board, bool left)
        {
            IToken[,] tokens = new IToken[Size.x, Size.y];
            Vector2 position = Position;

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    int index = left ? y - 1 : y + 1;
                    index = index < 0 ? index + Size.y : index >= Size.y ? index - Size.y : index;
                    tokens[0, y] = Tokens[0, index];
                    tokens[0, y]?.Update(board as IBoard, position + y * Vector2Int.up);
                }
            }

            Tokens = tokens;
        }

        public virtual void Move(IBoardMap board, bool left)
        {
            Vector2Int direction = left ? Vector2Int.left : Vector2Int.right;
            Vector2Int position = new Vector2Int(Mathf.CeilToInt(Position.x), Mathf.FloorToInt(Position.y));

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    if (IsColisionLocation(board, position + y * Vector2Int.up + direction))
                        return;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y]?.Update(board, Tokens[x, y].Position + direction);
        }

        public virtual void Locate(IBoardMap board, bool falling = false)
        {
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y]?.Locate(board, falling);

            IsPush = false;
        }
    }
}
