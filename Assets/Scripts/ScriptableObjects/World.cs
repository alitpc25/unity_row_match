using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// World holds the info about all the levels and board class will access at runtime and decide which level to login.
/*
 In Unity, while MonoBehaviour objects are stored in memory and exist only while playing the game; 
 Scriptable Objects exist in the project itself and are stored in serialised form. They are used as a container to store values. 
 A scriptable object exists even when the game is stopped and persists between play modes.
 So, we will use for storing levels.
 */
[CreateAssetMenu (fileName = "World", menuName = "World")]
public class World : ScriptableObject
{
    public Level[] levels;
}
