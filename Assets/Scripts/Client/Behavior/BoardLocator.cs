using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public static class BoardLocator
    {
        const float screenWidth = 33.5f;
        const float boardWidth = 7f;
        const float yPos = 0f;

        public static void UpdateBoardsLocation(BoardBehavior[] boards)
        {
            for (int i = 0; i < boards.Length; i++)
            {
                float offset = boardWidth * i - boards.Length * screenWidth / boardWidth + boards.Length / 2f + i + 2f;
                boards[i].transform.position = yPos * Vector3.up + offset * Vector3.right;
            }
        }
    }
}