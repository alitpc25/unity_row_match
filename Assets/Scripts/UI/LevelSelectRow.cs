using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Since play button will load the relevant scene

public class LevelSelectRow : MonoBehaviour
{
    public TextMeshProUGUI levelInfoText;
    public TextMeshProUGUI levelHighestScoreText;
    public GameObject playButton;
    public Level level;
    public string sceneToLoad;
    public ParticleSystem breakPlayButtonParticles;
    public BreakPlayButtonController breakPlayButtonController;

    public GameData gameData;

    void Awake()
    {
        gameData = FindObjectOfType<GameData>();
        if (transform.GetSiblingIndex() < 10)
        {
            breakPlayButtonController = this.transform.GetChild(4).GetComponent<BreakPlayButtonController>();
        }
    }

    void SetUp()
    {
        gameData.Load();
        if (level.level - 1 == 0 || gameData.saveData.prevHighScores[level.level - 2] == gameData.saveData.highScores[level.level - 2])
        {
            breakPlayButtonParticles.Stop();
        }
        else
        {
            if (!gameData.saveData.isActive[level.level - 1])
            {
                breakPlayButtonController.playButtonBreakLock();
                gameData.saveData.isActive[level.level - 1] = true;
            }
            gameData.saveData.prevHighScores[level.level - 2] = gameData.saveData.highScores[level.level - 2];
            gameData.Save();
        }
        DecideTexts();
    }

    void OnEnable()
    {
        SetUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    void DecideTexts()
    {
        levelInfoText.text = "Level " + level.level.ToString() + " - " + level.moveCount + " Moves";
        if (!gameData.saveData.isActive[level.level - 1])
        {
            levelHighestScoreText.text = "Locked";
        }
        else if (gameData.saveData.highScores[level.level - 1] == 0)
        {
            levelHighestScoreText.text = "No Score";
        }
        else
        {
            levelHighestScoreText.text = "Highest Score : " + gameData.saveData.highScores[level.level - 1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        PlayerPrefs.SetInt("currentLevel", level.level - 1); //To pass data of which level will be played. Tells Playground scene to load which level.
        PlayerPrefs.Save();
        SceneManager.LoadScene(sceneToLoad);
    }
}
