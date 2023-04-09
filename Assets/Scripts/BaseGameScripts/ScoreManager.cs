using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // To access UI elements
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreValue;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreValue.text = score.ToString();
    }

    public void increaseScore(string gemType)
    {
        switch (gemType)
        {
            case "RedGem":
                score += 100;
                break;
            case "GreenGem":
                score += 150;
                break;
            case "BlueGem":
                score += 200;
                break;
            case "YellowGem":
                score += 250;
                break;
        }
    }
}
