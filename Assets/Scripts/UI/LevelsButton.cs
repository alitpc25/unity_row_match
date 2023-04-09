using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelsButton : MonoBehaviour
{
    public GameObject LevelPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevelsPanel()
    {
        if (LevelPanel)
        {
            LevelPanel.SetActive(true);
        }
    }
}
