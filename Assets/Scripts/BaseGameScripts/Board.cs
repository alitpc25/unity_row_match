using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum GameState
{
    wait,
    move,
    win, //Highest score
    lose
}

public class Board : MonoBehaviour
{
    public GameState state = GameState.move;
    public World world;
    public Level level;
    public int levelNo;
    public string[] initialGems;

    // width and height values will be fetched from gamefile.
    public int boardWidth;
    public int boardHeight;

    private int screenWidth;
    private int screenHeight;

    public GameObject backgroundTilePrefab;
    private BackgroundTile[,] tileArray;

    public GameObject[] gems;
    public GameObject[,] gemsArray;

    public GameObject matchedGemPrefab;

    private ScoreManager scoreManager;
    private MoveCountManager moveCountManager;

    public string sceneToLoad;
    private GameData gameData;

    //Start and Awake work in similar ways except that Awake is called first and, unlike Start, will be called even if the script component is disabled.
    private void Awake()
    {
        moveCountManager = FindObjectOfType<MoveCountManager>();
        gameData = FindObjectOfType<GameData>();
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            levelNo = PlayerPrefs.GetInt("currentLevel");
            if (world)
            {
                if (world.levels[levelNo])
                {
                    level = world.levels[levelNo];
                    initialGems = level.grid.Split(",");
                    boardWidth = level.boardWidth;
                    boardHeight = level.boardHeight;
                    moveCountManager.moveCount = level.moveCount;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        tileArray = new BackgroundTile[boardWidth, boardHeight];
        gemsArray = new GameObject[boardWidth, boardHeight];
        SetUp();
    }

    void SetUp()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                Vector2 position = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(backgroundTilePrefab, position, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";
                createGemInit(i, j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool MatchingRow(int col, int row, GameObject obj)
    {
        return (col > boardWidth / 2 && gemsArray[col - 1, row].tag == obj.tag && gemsArray[col - 2, row].tag == obj.tag);
    }

    private IEnumerator DestroyMatchedGem(int col, int row)
    {
        if (gemsArray[col, row].GetComponent<Gem>().isMatched)
        {
            yield return new WaitForSeconds(.2f);
            scoreManager.increaseScore(gemsArray[col, row].tag);
            Destroy(gemsArray[col, row]);
            gemsArray[col, row] = null;
            ShowMatchedGem(col, row);
            yield return new WaitForSeconds(.2f);
            Destroy(gemsArray[col, row]);
            gemsArray[col, row] = null;
        }
    }

    private void ShowMatchedGem(int col, int row)
    {
        Vector2 position = new Vector2(col, row);
        GameObject gem = Instantiate(matchedGemPrefab, position, Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "(" + col + "," + row + ")";
        gem.GetComponent<Renderer>().sortingOrder = 2;
        gemsArray[col, row] = gem;
    }

    public void FindMatches()
    {
        for (int i = 0; i < boardHeight; i++)
        {
            GameObject firstGem = gemsArray[0, i]; // First gem of the current row i.
            bool isRowMatched = true;
            for (int j = 1; j < boardWidth; j++)
            {
                GameObject nextGem = gemsArray[j, i];
                if (firstGem.tag != nextGem.tag)
                {
                    isRowMatched = false;
                    break;
                }
            }
            if (isRowMatched)
            {
                firstGem.GetComponent<Gem>().isMatched = true;
                for (int j = 1; j < boardWidth; j++)
                {
                    GameObject nextGem = gemsArray[j, i];
                    nextGem.GetComponent<Gem>().isMatched = true;
                }
            }
        }
        DestroyMatches();
        if (moveCountManager.moveCount <= 0)
        {
            gameData.saveData.highScores[level.level] = 0;
            if (gameData.saveData.highScores[level.level - 1] < scoreManager.score)
            {
                //WIN
                if (gameData)
                {
                    state = GameState.win; // NOT NECESSARY I GUESS
                    gameData.saveData.highScores[level.level - 1] = scoreManager.score;
                    gameData.Save();
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else
            {
                //LOSE
                state = GameState.lose;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

        public void DestroyMatches()
    {
        moveCountManager.decreaseMoveCount();
        for (int j = 0; j < boardHeight; j++)
        {
            for (int i = 0; i < boardWidth; i++)
            {
                StartCoroutine(DestroyMatchedGem(i, j));
            }
        }
        StartCoroutine(CollapseRowCoroutine());
    }

    private IEnumerator CollapseRowCoroutine()
    {
        yield return new WaitForSeconds(.5f); // Waits for the coroutine before this. Use coroutines wisely.
        int matchedRowCount = 0;
        int matchedRowStart = 0;
        for (int j = 0; j < boardHeight; j++)
        {
            if (!gemsArray[0, j])
            {
                matchedRowCount++;
                if (j + 1 < boardHeight && !gemsArray[0, j + 1])
                {
                    matchedRowCount++;
                }
                matchedRowStart = j;
                break;
            }
        }
        if (matchedRowCount != 0)
        {
            for (int j = matchedRowStart + matchedRowCount; j < boardHeight; j++)
            {
                for (int i = 0; i < boardWidth; i++)
                {
                    gemsArray[i, j].GetComponent<Gem>().row -= matchedRowCount;
                    gemsArray[i, j - matchedRowCount] = gemsArray[i, j];
                    gemsArray[i, j] = null;

                }
            }
        }
        yield return new WaitForSeconds(.2f);
        StartCoroutine(FillBoardCoroutine());
    }

    private void createGemInit(int col, int row)
    {
        Vector2 position = new Vector2(col, row);
        GameObject gem = null;
        // B G R Y gems indexes.
        switch (initialGems[col + row * boardWidth])
        {
            case "b":
                gem = Instantiate(gems[0], position, Quaternion.identity);
                break;
            case "g":
                gem = Instantiate(gems[1], position, Quaternion.identity);
                break;
            case "r":
                gem = Instantiate(gems[2], position, Quaternion.identity);
                break;
            case "y":
                gem = Instantiate(gems[3], position, Quaternion.identity);
                break;

        }
        gem.transform.parent = this.transform;
        gem.name = "(" + col + "," + row + ")";
        gem.GetComponent<Renderer>().sortingOrder = 1;
        gemsArray[col, row] = gem;
        gem.GetComponent<Gem>().row = row;
        gem.GetComponent<Gem>().col = col;
    }

    // Creates random gems
    private void createGemRand(int col, int row)
    {
        Vector2 position = new Vector2(col, row);
        int tempGem = UnityEngine.Random.Range(0, gems.Length);
        while (MatchingRow(col, row, gems[tempGem]))
        {
            tempGem = UnityEngine.Random.Range(0, gems.Length);
        }
        GameObject gem = Instantiate(gems[tempGem], position, Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = "(" + col + "," + row + ")";
        gem.GetComponent<Renderer>().sortingOrder = 1;
        gemsArray[col, row] = gem;
        gem.GetComponent<Gem>().row = row;
        gem.GetComponent<Gem>().col = col;
    }

    private void RefillBoard()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (!gemsArray[i, j])
                {
                    createGemRand(i, j);
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if (gemsArray[i, j] && gemsArray[i, j].GetComponent<Gem>().isMatched)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCoroutine()
    {
        RefillBoard();
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.2f);
            DestroyMatches();
        }
        //isDeadlock(); // How to check deadlock in this type?
        yield return new WaitForSeconds(.2f);

        state = GameState.move;
    }

    //Finds if there is no possible match moves. If deadlock, end the game.
    public bool isDeadlock()
    {
        // Just check for below of an element starting from row[1] to row[height-1] and check the both row and row - 1 for a match.
        for (int r = 1; r < boardHeight; r++)
        {
            if (CheckRowMatch(r)) // Check rows r and r-1 for a match. If match exists, not deadlock.
            {
                return false;
            }
        }
        Debug.Log("DEADLOCK");
        return true;
    }

    private bool CheckRowMatch(int row)
    {

        for (int checkCol = 0; checkCol < boardWidth; checkCol++)
        {
            GameObject toBelowRow = gemsArray[checkCol, row];
            GameObject toAboveRow = gemsArray[checkCol, row - 1];

            bool belowRowMatched = true;
            bool aboveRowMatched = true;

            for (int c = 0; c < boardWidth; c++)
            {
                if (checkCol == c) continue;
                if (gemsArray[c, row].tag != toAboveRow.tag)
                {
                    aboveRowMatched = false;
                }
                if (gemsArray[c, row - 1].tag != toBelowRow.tag)
                {
                    belowRowMatched = false;
                }
            }
            if (aboveRowMatched || belowRowMatched)
            {
                return true;
            }
        }
        return false;
    }
}
