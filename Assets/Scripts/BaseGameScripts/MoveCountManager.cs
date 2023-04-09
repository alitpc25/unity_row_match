using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveCountManager : MonoBehaviour
{
    public TextMeshProUGUI moveCountValue;
    public int moveCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveCountValue.text = moveCount.ToString();
    }

    public void decreaseMoveCount()
    {
        moveCount--;
    }
}
