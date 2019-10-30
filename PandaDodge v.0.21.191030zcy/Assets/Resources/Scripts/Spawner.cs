using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject[] hazards;
    public GameObject[] hazards_2;
    public GameObject[] ingredients;
	public GameObject coin;

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public float minTimeBetweenSpawns;
    public float decrease;

    public GameObject player;

    public int foodindex1;
    public int foodindex2;


    // Update is called once per frame
    void Update()
    {
        Save savedData = readData();
        if (player != null)
        {
            if (timeBtwSpawns <= 0)
            {
				// Choose the spawn point.
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

				// The ratio of possibility of dropping attacks, ingredients and coins is 5:1:1.
                GameObject randomHazard = null;
				int isAttack = Random.Range(0, 7);
				if (isAttack < 5)
                {
                    if (savedData.unlocked[1]==false) {
                        randomHazard = hazards[Random.Range(0, hazards.Length)];
                    }
                    else
                    {
                        randomHazard = hazards_2[Random.Range(0, hazards_2.Length)];
                    }

                       
                }
					
				else if (isAttack == 5)
                    //randomHazard = ingredients[Random.Range(0, ingredients.Length)];
                    randomHazard = ingredients[Random.Range(foodindex1, foodindex2)];
				else
					randomHazard = coin;

                ////////////////////////////////////////
                //what if food is dropped more often:
                //if (isAttack < 1)
                //   randomHazard = hazards[Random.Range(0, hazards.Length)];
                //else if (1 <= isAttack && isAttack >= 5)
                //    //randomHazard = ingredients[Random.Range(0, ingredients.Length)];
                //    randomHazard = ingredients[Random.Range(foodindex1, foodindex2)];
                //else
                //    randomHazard = coin;
                //what if food is dropped more often
                /////////////////////////////////////////


                Instantiate(randomHazard, randomSpawnPoint.position, Quaternion.identity);

                if (startTimeBtwSpawns > minTimeBetweenSpawns)
                {
                    startTimeBtwSpawns -= decrease;
                }

                timeBtwSpawns = startTimeBtwSpawns;

             }
            else
            {
                timeBtwSpawns -= Time.deltaTime;
            }
        }
    }

    public Save readData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream OldFile = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
        Save OldData = (Save)bf.Deserialize(OldFile);
        OldFile.Close();

        return OldData;
    }


}
