using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class CraftControl : MonoBehaviour
{
    public Dropdown ingredient1;
    public Dropdown ingredient2;
    public Dropdown ingredient3;

    public Text textField1;
    public Text textField2;
    public Button toMap;

    public GameObject craftResultPanel;
    public GameObject[] ingredients = new GameObject[20]; //SUBJECT TO CHANGE FOR FUTURE INGRIDIENTS
    public Dictionary<String, GameObject> ingredientTable = new Dictionary<String, GameObject>();

    private List<String> lv2Recipe = new List<String> {"Eggplant", "Garlic", "Red pepper"};     // My Secret Recipe
    private List<String> Recipe_2 = new List<String> { "Mushroom", "Onion", "Green pepper"};     //  added by Yiwen. My Secret Recipe_2

    void Start()
    {
        ingredient1.ClearOptions();
        ingredient2.ClearOptions();
        ingredient3.ClearOptions();

        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredientTable.Add(ingredients[i].name, ingredients[i]);
        }

       
        Save savedData = readData();
        Dictionary<string, int> ingredientsToShow = savedData.ingredientsCollected;

        List<Dropdown.OptionData> optionDatas = new List<Dropdown.OptionData>();
        //List<string> optionDatas = new List<string>();
        foreach (KeyValuePair<string, int> thisIngredient in ingredientsToShow) {
            string myKey = thisIngredient.Key;
            GameObject myObj = ingredientTable[myKey];
            Sprite mySprite = myObj.GetComponent<SpriteRenderer>().sprite;
            var dataOption = new Dropdown.OptionData(myKey, mySprite);
            optionDatas.Add(dataOption);
        }
        //Debug.Log(optionDatas.Count);
        //for (int i = 0; i < optionDatas.Count; i++)
        //{
         //   Debug.Log(optionDatas[i].GetType());
        //}

        ingredient1.AddOptions(optionDatas);
        ingredient2.AddOptions(optionDatas);
        ingredient3.AddOptions(optionDatas);
    }

    public Save readData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream OldFile = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save OldData = (Save)bf.Deserialize(OldFile);
        OldFile.Close();

        return OldData;
    }

    public void craft()
    {
        string option1 = ingredient1.options[ingredient1.value].text;
        string option2 = ingredient2.options[ingredient2.value].text;
        string option3 = ingredient3.options[ingredient3.value].text;

        Save savedData = readData();
        savedData.ingredientsCollected[option1] -= 1;
        if (savedData.ingredientsCollected[option1] == 0)
        {
            savedData.ingredientsCollected.Remove(option1);
        }
        savedData.ingredientsCollected[option2] -= 1;
        if (savedData.ingredientsCollected[option2] == 0)
        {
            savedData.ingredientsCollected.Remove(option2);
        }
        savedData.ingredientsCollected[option3] -= 1;
        if (savedData.ingredientsCollected[option3] == 0)
        {
            savedData.ingredientsCollected.Remove(option3);
        }

        // CODE BELOW IS SUBJECT TO CHANGE
        string head;
        string myParagraph;
        Debug.Log(lv2Recipe.Contains(option1));
        Debug.Log(lv2Recipe);
        Debug.Log(option1);
        if (lv2Recipe.Contains(option1) && lv2Recipe.Contains(option2) && lv2Recipe.Contains(option3))      // Flawed Logic. Change Later
        {
            savedData.unlocked[1] = true;
            // POP SUCCESS
            head = "Bravo!";
            myParagraph = "Level 2 is unlocked!";
            textField1.color = Color.yellow;
        }
        else
        {   //below is added by Yiwen
            if (Recipe_2.Contains(option1) && Recipe_2.Contains(option2) && Recipe_2.Contains(option3))      // Flawed Logic. Change Later
            {
                savedData.unlocked[2] = true;
                // POP SUCCESS
                head = "Bravo!";
                if (!savedData.unlocked[1])
                {
                    myParagraph = "Level 3 is unlocked! Pass Level 2 to gain access !";
                }
                else
                {
                    myParagraph = "Level 3 is unlocked!";
                }
                textField1.color = Color.yellow;
            }
            //above is added by Yiwen

            else
            {
                // POP FAIL
                head = "Sigh...";
                myParagraph = "You lost your ingredients.";
                textField2.color = Color.red;
            }
            
        }

        


        textField1.text = head;
        textField1.fontSize = 50;
        textField2.text = myParagraph;
        textField2.color = Color.white;
        textField2.fontSize = 25;

        craftResultPanel.SetActive(true);

        saveAfterCraft(savedData);
    }

    public void close()
    {
        craftResultPanel.SetActive(false);
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
