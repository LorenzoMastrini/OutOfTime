using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabList = new List<GameObject>();
    [SerializeField] int numberOfObjects;

    [SerializeField] float radius = 5f;
    [SerializeField] float minDistance = 0.5f;
    
    private List<Vector3> spawnedPositions = new List<Vector3>();



    void Awake()
    {
        spawnedPositions = new List<Vector3>(numberOfObjects);
    }



    void Start()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            SpawnObject();
        }
    }



    void SpawnObject()
    {
        int attempts = 0;

        while (attempts < 1000)
        {
            float angle = Random.Range(-90f, 90f) * Mathf.Deg2Rad;
            float distance = Mathf.Sqrt(Random.value) * radius;

            Vector3 position = transform.position + new Vector3(
                Mathf.Cos(angle) * distance,
                0,
                Mathf.Sin(angle) * distance
            );

            bool valid = true;

            foreach (Vector3 p in spawnedPositions)
            {
                if (Vector3.Distance(position, p) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                spawnedPositions.Add(position);
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                
                // Generate a random person to instantiate
                int prefabIndex = Random.Range(0, prefabList.Count);

                Instantiate(prefabList[prefabIndex], position, randomRotation);
                return;
            }

            attempts++;
        }
    }
}