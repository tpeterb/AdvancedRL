using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBehaviors : MonoBehaviour
{
    [SerializeField]
    AgentController agentController;

    [SerializeField]
    ZombieController zombieController;

    [SerializeField]
    public List<ZombieController> spawnedZombies = new List<ZombieController>();

    [SerializeField]
    int zombieCount;

    [SerializeField]
    int timeBetweenSpawns = 1;

    [SerializeField]
    Transform spawnZone1;

    [SerializeField]
    Transform spawnZone2;

    [SerializeField]
    Transform spawnZone3;

    [SerializeField]
    Transform spawnZone4;

    [SerializeField]
    Transform environmentLocation;

    public void spawnAgent()
    {
        agentController.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void callSpawnZombie()
    {
        StartCoroutine(spawnZombie());
    }

    IEnumerator spawnZombie()
    {

        Vector3 spawnZone1Coords = spawnZone1.transform.position;
        Vector3 spawnZone2Coords = spawnZone2.transform.position;
        Vector3 spawnZone3Coords = spawnZone3.transform.position;
        Vector3 spawnZone4Coords = spawnZone4.transform.position;

        if (spawnedZombies.Count != 0)
        {
            removeZombie(spawnedZombies);
        }

        for (int i = 0; i < zombieCount; i++)
        {
            Vector3 randomSpawnZone1Coords = spawnZone1Coords + new Vector3(Random.Range(-4.3f, 4.3f), 1f, Random.Range(-3.9f, 3.9f));
            Vector3 randomSpawnZone2Coords = spawnZone2Coords + new Vector3(Random.Range(-4.3f, 4.3f), 1f, Random.Range(-3.9f, 3.9f));
            Vector3 randomSpawnZone3Coords = spawnZone3Coords + new Vector3(Random.Range(-4.3f, 4.3f), 1f, Random.Range(-3.9f, 3.9f));
            Vector3 randomSpawnZone4Coords = spawnZone4Coords + new Vector3(Random.Range(-4.3f, 4.3f), 1f, Random.Range(-3.9f, 3.9f));

            Vector3[] randomArea = {
                randomSpawnZone1Coords,
                randomSpawnZone2Coords,
                randomSpawnZone3Coords,
                randomSpawnZone4Coords
            };

            int counter = 0;
            bool distanceGood;

            ZombieController newZombie = Instantiate(zombieController);
            newZombie.target = agentController.transform;
            newZombie.transform.parent = environmentLocation;
            Vector3 zombieLocation = randomArea[Random.Range(0, randomArea.Length)];

            if (spawnedZombies.Count  == 0)
            {
                newZombie.moveZombie(zombieLocation);
                spawnedZombies.Add(newZombie);
                yield return new WaitForSeconds(timeBetweenSpawns);
            } else
            {
                for (int k = 0; k < spawnedZombies.Count; k++)
                {
                    if (counter < 10)
                    {
                        distanceGood = CheckOverlap(zombieLocation, spawnedZombies[k].transform.localPosition, 1f);
                        if (!distanceGood)
                        {
                            zombieLocation = randomArea[Random.Range(0, randomArea.Length)];
                            k--;
                        }
                        counter++;
                    } else
                    {
                        k = spawnedZombies.Count;
                    }
                }
                newZombie.moveZombie(zombieLocation);
                spawnedZombies.Add(newZombie);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
    }

    public bool CheckOverlap(Vector3 objectWeWantToAvoidOverlapping, Vector3 alreadyExistingObject, float minDistanceWanted)
    {
        float distanceBetweenObjects = Vector3.Distance(objectWeWantToAvoidOverlapping, alreadyExistingObject);
        if (minDistanceWanted <= distanceBetweenObjects)
        {
            return true;
        }
        return false;
    }

    public int getZombieCount()
    {
        return spawnedZombies.Count;
    }

    private void removeZombie(List<ZombieController> toBeDeletedGameObjectList)
    {
        foreach (ZombieController zombieController in toBeDeletedGameObjectList)
        {
            Destroy(zombieController.gameObject);
        }
        toBeDeletedGameObjectList.Clear();
    }
}
