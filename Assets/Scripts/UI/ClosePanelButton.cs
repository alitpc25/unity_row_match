using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ClosePanelButton : MonoBehaviour
{

    public GameObject LevelsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseLevelsPanel()
    {
        if (LevelsPanel)
        {
            LevelsPanel.SetActive(false);
        }
    }
}
