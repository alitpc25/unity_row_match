using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanelScript : MonoBehaviour
{
    public World world;
    public CelebrationController celebrationController;
    public ParticleSystem celebrationParticles;
    public GameData gameData;

    // Start is called before the first frame update
    void Awake()
    {
        celebrationController = FindObjectOfType<CelebrationController>();
        celebrationParticles = GameObject.Find("CelebrationParticles").GetComponent<ParticleSystem>();
    }

    void Start()
    {
        decideCelebrate();
    }

    void OnEnable()
    {
        decideCelebrate();
    }

    void decideCelebrate()
    {
        gameData.Load();
        int levelNo = PlayerPrefs.GetInt("currentLevel");
        if (gameData.saveData.prevHighScores[levelNo] < gameData.saveData.highScores[levelNo] && !gameData.saveData.isActive[levelNo + 1])
        {
            //Celebrate
            celebrationController.celebrate();
        }
        else
        {
            celebrationParticles.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        decideCelebrate();
    }
}
