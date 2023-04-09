using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject
{
    public int boardWidth;
    public int boardHeight;
    public int level;
    public int moveCount;
    public string grid; // bottom-left to top-right
}
