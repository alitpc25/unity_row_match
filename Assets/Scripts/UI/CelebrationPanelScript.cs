using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CelebrationPanelScript : MonoBehaviour
{

    public TextMeshProUGUI highestScoreText;
    public GameData gameData;

    void Awake()
    {
        gameData = FindObjectOfType<GameData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (gameData != null)
        {
            highestScoreText.text = "Highest Score : " + gameData.saveData.highScores[PlayerPrefs.GetInt("currentLevel")];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
