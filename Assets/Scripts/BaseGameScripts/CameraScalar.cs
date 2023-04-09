using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScalar : MonoBehaviour
{
    private Board board;
    public float aspectRatio = .5625f; // 9/16
    public float padding = 2;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        if(board)
        {
            RepositionCamera(board.boardWidth - 1, board.boardHeight - 1);
        }
    }

    void RepositionCamera(float x, float y)
    {
        Vector3 tempPos = new Vector3(x/2, y/2 + 1.0f, -5.0f);
        transform.position = tempPos;
        if(board.boardWidth >= board.boardHeight)
        {
            Camera.main.orthographicSize = (board.boardWidth / 2.0f + padding) / aspectRatio;
        } else
        {
            Camera.main.orthographicSize = (board.boardHeight / 1.5f + padding);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
