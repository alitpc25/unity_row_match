using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayButton : MonoBehaviour
{
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    public Image buttonImage;
    private Button button;
    private LevelSelectRow levelSelectRow;
    public Level level;
    public GameData gameData;
    
    void Awake()
    {
        button = GetComponent<Button>(); // Directly gets from this playbutton, since there is one of this type of component.
        buttonImage = GetComponent<Image>();
        levelSelectRow = this.transform.parent.gameObject.GetComponent<LevelSelectRow>();
        level = levelSelectRow.level;
        LoadData();
        DecideSprite();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        DecideSprite();
    }

    void OnEnable()
    {
        LoadData();
        DecideSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadData()
    {
        GameData gameData = null;
        if (levelSelectRow)
        {
            levelSelectRow.gameData.Load();
            gameData = levelSelectRow.gameData;
        } else
        {
            gameData.Load();
            gameData = this.gameData;
        }
        if (gameData)
        {
            isActive = gameData.saveData.isActive[level.level - 1];
        }
    }

    void DecideSprite()
    {
        if(isActive) {
            buttonImage.overrideSprite = activeSprite;
            button.enabled = true;
        } else
        {
            buttonImage.overrideSprite = lockedSprite;
            button.enabled = false;
        }
    }
}
