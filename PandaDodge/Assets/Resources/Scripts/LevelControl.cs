using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelControl : MonoBehaviour
{
    public GameObject note;
    public Button[] levels;
    public Text noteText;
    
    SceneSwitcher ss;
    // Start is called before the first frame update
    void Start()
    {
        Save savedData = readData();
        for (int i = 0; i < levels.Length; i++)
        {
            if (savedData.unlocked[i])
            {

                levels[i].GetComponent<Button>().onClick.AddListener(switchScene);
            }
            else
            {
                if (i == 1) 
                {
                    levels[i].GetComponent<Button>().onClick.AddListener(alert1);
                }
                if (i == 2) 
                {
                    levels[i].GetComponent<Button>().onClick.AddListener(alert2);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchScene()
    {
        ss = levels[0].GetComponent<SceneSwitcher>();
        ss.PlayGame();
    }

    public void alert1() 
    {
        Save savedData = readData();
        noteText.text = "You shall not pass!\n Uhhhhhh ------  I want to eat\n STEAMED EGGPLANT (蒜蓉蒸茄子). \n_(:зゝ∠)_.";
        noteText.color = Color.white;
        note.SetActive(true);
        
    }


    public void alert2() 
    {
        Save savedData = readData();
        if (savedData.unlocked[1]==false && savedData.unlocked[2]==true)
        {
            noteText.text = "Level 2 needs to be completed to access the unlocked level 3 !";
        }
        else
        {
            noteText.text = "You shall not pass!\n Uhhhhhh ------  I want to eat\n fried MUSHROOM & GREEN PEPPER WITH diced ONION (洋葱炒蘑菇青椒). \n_(:зゝ∠)_.";
        }
               

        noteText.color = Color.white;
        note.SetActive(true);
    }


    public void close()
    {
        note.SetActive(false);
    }

    public Save readData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream OldFile = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save OldData = (Save)bf.Deserialize(OldFile);
        OldFile.Close();

        return OldData;
    }

    public void saveAfterCraft(Save save) 
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream NewFile = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(NewFile, save);
        NewFile.Close();

        Debug.Log("Game Saved");
    }


}
