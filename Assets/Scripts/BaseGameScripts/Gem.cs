using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{

    public int col;
    public int row;
    private Board board;
    private GameObject affectedGem;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition; // For switching to happen. Temporarily keeps the target position that the gem should go.
    public double swipeAngle = 0;
    public float swipeResist = .8f; // To ensure swipe happened, not click.

    public bool isMatched = false;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        row = (int)transform.position.y; // current x pos
        col = (int)transform.position.x; // current y pos
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(col - transform.position.x) > .1)
        {
            // Move towards the target.
            tempPosition = new Vector2(col, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .5f); // Linearly interpolates between two points (to move the object gradually)
        }
        else
        {
            // Directly set the position.
            tempPosition = new Vector2(col, transform.position.y);
            transform.position = tempPosition;
        }
        if (Mathf.Abs(row - transform.position.y) > .1)
        {
            // Move towards the target.
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .5f);
        }
        else
        {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, row);
            transform.position = tempPosition;
        }
    }

    private void OnMouseDown()
    {
        if (board.state == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalcAngle();
    }

    void CalcAngle()
    {
        if(Math.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Math.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Math.PI;
            board.state = GameState.wait;
            MoveGems();
        } else
        {
            board.state = GameState.move;
        }
    }

    void MoveGems()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && col < board.boardWidth - 1)
        {
            //Swipe right
            affectedGem = board.gemsArray[col + 1, row];
            board.gemsArray[col + 1, row] = this.gameObject;
            board.gemsArray[col, row] = affectedGem;
            affectedGem.GetComponent<Gem>().col -= 1;
            col += 1;
        }
        else if (swipeAngle <= -45 && swipeAngle > -135 && row > 0)
        {
            //Swipe down
            affectedGem = board.gemsArray[col, row - 1];
            board.gemsArray[col, row - 1] = this.gameObject;
            board.gemsArray[col, row] = affectedGem;
            affectedGem.GetComponent<Gem>().row += 1;
            row -= 1;
        } else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.boardHeight - 1)
        {
            //Swipe up
            affectedGem = board.gemsArray[col, row + 1];
            board.gemsArray[col, row + 1] = this.gameObject;
            board.gemsArray[col, row] = affectedGem;
            affectedGem.GetComponent<Gem>().row -= 1;
            row += 1;
        } else if ((swipeAngle > 135 || swipeAngle < -135) && col > 0)
        {
            //Swipe left
            affectedGem = board.gemsArray[col - 1, row];
            board.gemsArray[col - 1, row] = this.gameObject;
            board.gemsArray[col, row] = affectedGem;
            affectedGem.GetComponent<Gem>().col += 1;
            col -= 1;
        }
        
        if(affectedGem)
        {
            board.FindMatches();
            affectedGem = null;
        }
    }

}
