using UnityEngine;
using UnityEngine.AI;

public class NavMeshSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] people = null;
    [SerializeField] int spawnCount = 0;
    [SerializeField] float radius = 0f;
    [SerializeField] Vector2 boxSize = Vector2.zero;
    [SerializeField] Vector3 spawnOffset = new Vector3(0, 0, 0);



    void Start()
    {
        SpawnPeople();
    }



    public Vector3 FindValidNavMeshSpawnPoint(Vector3 center)
    {
        Vector3 randomPoint;

        float halfWidth = boxSize.x * 0.5f;
        float halfHeight = boxSize.y * 0.5f;

        randomPoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), 0, Random.Range(-halfHeight, halfHeight));

        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            //Debug.LogWarning("Failed to find a valid NavMesh spawn point, retrying.");

            // Retry 10 attemps each
            for (int i = 0; i < 10; i++)
            {
                //Debug.Log($"Attempt: {i}");
                randomPoint = center + new Vector3(Random.Range(-halfWidth, halfWidth), 0, Random.Range(-halfHeight, halfHeight));
                if (NavMesh.SamplePosition(randomPoint, out hit, radius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            // Fallback to the center if no valid point is found
            return center;
        }
    }



    public void SpawnPeople()
    {
        for (int i = 0; i < spawnCount; i++ )
        {
            // Random (valid) position
            Vector3 randomPoint = FindValidNavMeshSpawnPoint(transform.position);

            // Random rotation
            Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            GameObject objectToSpawn = people[Random.Range(0, people.Length)];
            Instantiate(objectToSpawn, randomPoint + spawnOffset, randomRotation, transform);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(boxSize.x, 0, boxSize.y));
    }
}