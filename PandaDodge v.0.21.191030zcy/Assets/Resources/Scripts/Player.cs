using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Player : MonoBehaviour
{

    public GameObject losePanel;

    public Text healthDisplay;
	public Text timeDisplay;
	public Text coinDisplay;
	public Text collectionDisplay;

    public float speed;
    private float input;
    private string system;
    private int controlFlag;

    Rigidbody2D rb;
    Animator anim;
    AudioSource source;

    public int health;
	public int coin;
    public int totalTime;
    public int currentLevel;
	// Dict to keep ingredients we collect.
	public Dictionary<string, int> ingredients;

    // Start is called before the first frame update
    void Start()
    {
        // source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthDisplay.text = health.ToString();
		timeDisplay.text = totalTime.ToString();
        StartCoroutine(CountDown());
        system = SystemInfo.operatingSystem;
        char systemChar = system.ToCharArray(0, 1)[0];
        if (systemChar.Equals('A') || systemChar.Equals('i')){          // Android or iPhone
            controlFlag = 0;
        } else if (systemChar.Equals('W') || systemChar.Equals('M')){       // Windows or Mac OS
            controlFlag = 1;
        }

		ingredients = new Dictionary<string, int>();
    }
	
    private void Update()
    {
        if (input != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else {
            anim.SetBool("isRunning", false);
        }
	
        //if (input > 0)
        //{
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //}
        //else if (input < 0) {
        //    transform.eulerAngles = new Vector3(0, 180, 0);
        //}
        
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        // Storing player's input
        if (controlFlag == 1)
        {
            input = Input.GetAxisRaw("Horizontal");
            //Debug.Log(input);
        }
        else if (controlFlag == 0)
        {
            input = 0;
            if (Input.touchCount > 0) {
                Vector2 touchPosition;
                touchPosition = Input.GetTouch(0).position;
                if (touchPosition.x > Screen.width / 2)
                {
                    input = 1;
                }
                else
                {
                    input = -1;
                }
            }
        }

        // Moving player
        rb.velocity = new Vector2(input * speed, rb.velocity.y);
      
    }

    public void TakeDamage(int damageAmount) {
        // source.Play();
        health = health - damageAmount < 0 ? 0 : health - damageAmount;
        healthDisplay.text = health.ToString();

        if (health <= 0) {
			ingredients.Clear();
            GameFinished();
        }
    }

	public void AddHealth() {
        if (health < 3) {
            health += 1;
            healthDisplay.text = health.ToString();
        }
	}

	public void CollectIngredient(string ingredient) {
		if (ingredients.ContainsKey(ingredient))
			ingredients[ingredient] = ingredients[ingredient] + 1;
		else
			ingredients[ingredient] = 1;
		// Debug.Log(ingredient + ": " + ingredients[ingredient]);
	}

	public void CollectCoin() {
		coin += 1;
		coinDisplay.text = coin.ToString();
	}

    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            FileStream OldFile = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save OldData = (Save)bf.Deserialize(OldFile);
            OldFile.Close();

            Dictionary<string, int> UpdatedIngredients = OldData.ingredientsCollected;
            //These lines overwrites the loaded data
            foreach (KeyValuePair<string, int> ingredient in ingredients)
            {
                string thisKey = ingredient.Key;
                int amount = ingredient.Value;
                if (UpdatedIngredients.ContainsKey(thisKey))
                {
                    UpdatedIngredients[thisKey] += amount;
                } else
                {
                    UpdatedIngredients.Add(thisKey, amount);
                }
            }
            save.ingredientsCollected = UpdatedIngredients;
            save.coins += OldData.coins;
            save.levelCompleted = currentLevel;
        }
        FileStream NewFile = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(NewFile, save);
        NewFile.Close();

        Debug.Log("Game Saved");
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        save.ingredientsCollected = ingredients;
        save.coins = coin;
        save.levelCompleted = currentLevel;

        return save;
    }


    private void GameFinished() {
		string collection = "";
		if (ingredients.Count == 0 && coin == 0)
			collection = "You Got Nothing!";
		else {
			collection = "You Got: \n\n";
			collection += "Coins X " + coin + "\n";
			foreach(KeyValuePair<string, int> ingredient in ingredients) { 
				collection += ingredient.Key + " X " + ingredient.Value + "\n"; 
			}
		}
		collectionDisplay.text = collection;
		losePanel.SetActive(true);
        Destroy(gameObject);

        // Store data to local
        SaveGame();

    }

    IEnumerator CountDown()
    {
        while (totalTime >= 0) {
			timeDisplay.text = totalTime.ToString();
            yield return new WaitForSeconds(1);
            totalTime--;
            if (totalTime <= 0) {
				GameFinished();
			}
        }
    }

}
