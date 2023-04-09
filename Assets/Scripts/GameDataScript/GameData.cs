using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[Serializable]
public class SaveData
{
    public bool[] isActive;
    public int[] highScores;
    public int[] prevHighScores;
} 

public class GameData : MonoBehaviour
{

    public static GameData gameData;
    public SaveData saveData;

    // Start is called before the first frame update
    void Awake()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject); // Singleton. When loading a new scene, this object won't be destroyed and only one will exist.
            gameData = this; 
        } else
        {
            Destroy(this.gameObject);
        }
        Load();
    }

    void Start()
    {
        
    }

    public void Save()
    {
        //Creating binary formatter to read binary files
        BinaryFormatter formatter = new BinaryFormatter();
        //Creating a route from the program to the file.
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.OpenOrCreate);
        SaveData data = new SaveData();
        data = saveData; // Copy of our data, saveData.
        formatter.Serialize(file, data); //Serialization is the process of converting a data object—a combination of code and data represented within a region of data storage—into a series of bytes. Serialize method saves the data in the file.
        file.Close();
        Debug.Log("Saved");

    }

    private void OnDisable()
    {
        Save();
    }

    public void Load()
    {
        // Check if the save game file exists.
        if(File.Exists(Application.persistentDataPath + "/player.dat"))
        {
            //Creating binary formatter to read binary files
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
        } else
        {
            saveData = new SaveData();
            saveData.isActive = new bool[10];
            saveData.isActive[0] = true;
            saveData.highScores = new int[10];
            saveData.prevHighScores = new int[10];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
