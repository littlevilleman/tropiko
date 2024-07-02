using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class BoardLocator : MonoBehaviour
    {
        float screenWidth = 33.5f;
        float boardWidth = 7f;
        float yPos = 0f;


        public void UpdateBoardsLocation()
        {
            BoardBehavior[] boards = GetComponentsInChildren<BoardBehavior>();

            for (int i = 0; i < boards.Length; i++)
            {
                float offset = boardWidth * i - boards.Length * screenWidth / boardWidth + boards.Length / 2f + i + 2f;
                boards[i].transform.position = yPos * Vector3.up + offset * Vector3.right;
            }
        }
    }
}