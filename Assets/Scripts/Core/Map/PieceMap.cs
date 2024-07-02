using UnityEngine;

namespace Core.Map
{
    public abstract class PieceMap : TokenMap, IPieceMap
    {
        public Vector2 Position => Tokens[0, 0].Position;
        public Vector2Int Location => Tokens[0, 0].Location;

        protected virtual void Update(IBoard board, float time, float speed)
        {
            Vector2 location = Position + time * speed * Vector2.down;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y].Update(board, location + y * Vector2.up, Vector2Int.zero);
        }

        public virtual void Rotate(IBoardMap board, bool left)
        {
            IToken[,] tokens = new Core.IToken[Size.x, Size.y];

            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    int index = left ? y - 1 : y + 1;
                    index = index < 0 ? index + Size.y : index >= Size.y ? index - Size.y : index;
                    tokens[0, y] = Tokens[0, index];
                    tokens[0, y]?.Update(board as IBoard, Position + y * Vector2Int.up, Vector2Int.zero);
                }
            }

            Tokens = tokens;
        }

        public virtual void Move(IBoardMap board, bool left)
        {
            Vector2Int direction = left ? Vector2Int.left : Vector2Int.right;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    if (IsValidLocation(board, Tokens[x, y].Location + direction))
                        return;

            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y]?.Update(board as IBoard, Tokens[x, y].Position, direction);
        }

        public virtual void Locate(IBoardMap board)
        {
            for (int x = 0; x < Size.x; x++)
                for (int y = 0; y < Size.y; y++)
                    Tokens[x, y]?.Locate(board, Location + Vector2Int.up * y);
        }
    }
}
