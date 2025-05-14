using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int waveCount;
    public int enemyCount;
    
    public Transform[] roomOneSpawnPoints;
    public Transform[] roomTwoSpawnPoints;
    public Transform[] roomThreeSpawnPoints;
    
    public GameObject[] enemiesPrefab;
    public GameObject[] doorsToOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoomOneEnemies());
    }

    public void UpdateEnemyCount()
    {
        if (enemyCount > 0)
        {
            enemyCount--;

            if (enemyCount <= 0)
            {
                //Open the door
                //doorsToOpen[waveCount - 1].SetActive(false);
                
                //Update the wavecount
                waveCount++;

                /*if (waveCount == 2)
                {
                    StartCoroutine(SpawnRoomTwoEnemies());
                }

                if (waveCount == 3)
                {
                    StartCoroutine(SpawnRoomThreeEnemies());
                }*/

                if (waveCount < 5)
                {
                    StartCoroutine(SpawnRoomOneEnemies());
                }

                if (waveCount >= 5)
                {
                    StartCoroutine(SpawnRoomThreeEnemies());
                }
            }
        }
    }

    // Room 1 is standard enemies
    private IEnumerator SpawnRoomOneEnemies()
    {
        int numberOfEnemies = Random.Range(1, 5);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemiesPrefab[0], roomOneSpawnPoints[Random.Range(0, 
                roomOneSpawnPoints.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            enemyCount++;
        }
    }
    
    // Room 2 is standard enemies
    private IEnumerator SpawnRoomTwoEnemies()
    {
        int numberOfEnemies = Random.Range(3, 7);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemiesPrefab[0], roomTwoSpawnPoints[Random.Range(0, 
                roomTwoSpawnPoints.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            enemyCount++;
        }
    }
    
    // Room 3 is the Boss area
    private IEnumerator SpawnRoomThreeEnemies()
    {
        //int numberOfEnemies = Random.Range(1, 5);
        int numberOfEnemies = 1;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            /*Instantiate(enemiesPrefab[1], roomThreeSpawnPoints[Random.Range(0, 
                roomThreeSpawnPoints.Length)].position, Quaternion.identity);*/
            
            Instantiate(enemiesPrefab[1], roomOneSpawnPoints[Random.Range(0, 
                roomOneSpawnPoints.Length)].position, Quaternion.identity);
            
            yield return new WaitForSeconds(2f);
            
            enemyCount++;
        }
    }
}
