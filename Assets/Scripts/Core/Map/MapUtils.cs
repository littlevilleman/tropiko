using Core.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class MapUtils
    {
        private static Vector2Int[] Horizontal = { Vector2Int.right, Vector2Int.left };
        private static Vector2Int[] Vertical = { Vector2Int.up, Vector2Int.down };
        private static Vector2Int[] LeftDiagonal = { new Vector2Int(-1, 1), new Vector2Int(1, -1) };
        private static Vector2Int[] RightDiagonal = { new Vector2Int(1, 1), new Vector2Int(-1, -1) };
        public static List<Vector2Int[]> Directions => new List<Vector2Int[]> { Horizontal, Vertical, LeftDiagonal, RightDiagonal };
        public static List<Vector2Int[]> OrthoDirections => new List<Vector2Int[]> { Horizontal, Vertical};

        public static Vector2Int GetFallLocation(IBoardMap board, Vector2Int location)
        {
            Vector2Int fallLocation = location;
            while (fallLocation.y > 0)
            {
                if (board.GetToken(fallLocation.x, fallLocation.y - 1) != null)
                    break;

                fallLocation += Vector2Int.down;
            }

            return fallLocation;
        }

        public static bool IsColisionLocation(IBoardMap map, Vector2Int location)
        {
            return location.y < 0 || map.GetToken(location.x, location.y) != null;
        }

        public static Vector2 ClampPosition(IBoardMap board, Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, 0, board.Size.x - 1);
            pos.y = Mathf.Clamp(pos.y, 0, board.Size.y + 2);

            return pos;
        }

        public static List<IToken> GetTokenNeighbours(ITokenMap board, Vector2Int sourceLocation, ETokenType type = ETokenType.NONE)
        {
            List<IToken> neighbours = new List<IToken>();
            foreach (Vector2Int[] directions in OrthoDirections)
            {
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int location = sourceLocation + direction;

                    if (board.GetToken(location.x, location.y, out IToken neighbour) && neighbour.Type == type)
                        neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
    }
}