using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Background : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite backgroundImage;
    public Sprite backgroundImage2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/lv1background");

        Save savedData = readData();
        Debug.Log("coins: " + savedData.coins);
        if (savedData.unlocked[1] == false)
        {
            spriteRenderer.sprite = backgroundImage;
        }
        else
        {
            spriteRenderer.sprite = backgroundImage2;
        }
        
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight / Screen.height * Screen.width;
        float width = screenWidth / spriteRenderer.sprite.bounds.size.x;
        float height = screenHeight / spriteRenderer.sprite.bounds.size.y;
        transform.localScale = new Vector3(width, height, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Save readData() 
    {
        BinaryFormatter bf = new BinaryFormatter();
        //if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        //{
            FileStream OldFile = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save OldData = (Save)bf.Deserialize(OldFile);
            OldFile.Close();
            return OldData;
        //}
        //return new Save();
    }
}
