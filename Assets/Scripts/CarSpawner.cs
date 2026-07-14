using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabList = new List<GameObject>();

    [SerializeField] float speed = 10f;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 1f;
    [SerializeField] Direction spawnedObjDirection = Direction.x;

    private Coroutine spawnCoroutine;


    private void OnEnable()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }



    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }



    void SpawnCar()
    {
        // Generate a random car to instantiate
        int prefabIndex = Random.Range(0, prefabList.Count);
        GameObject newCar = Instantiate(prefabList[prefabIndex], transform.position, Quaternion.identity, transform);

        // Get the prefab script that controls movement
        if (newCar.GetComponent<CarMovement>() != null) // Normal car
        {
            CarMovement movementScript = newCar.GetComponent<CarMovement>();
            
            // Modify direction
            movementScript.SetMoveDirection(spawnedObjDirection);
            movementScript.SetSpeed(speed);

        } else if(newCar.GetComponent<FutureCarMovement>()) // Future car
        {
            FutureCarMovement movementScript = newCar.GetComponent<FutureCarMovement>();

            // Modify direction
            movementScript.SetMoveDirection(spawnedObjDirection);
            movementScript.SetSpeed(speed);
        }
    }



    // Hanlde spawn interval with a coroutine instead of Update
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Randomize spawn time
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            yield return new WaitForSeconds(spawnTime);
            SpawnCar();
        }
    }
}