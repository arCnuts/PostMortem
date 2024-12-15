using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTriggerBox : MonoBehaviour
{
    public GameObject enemy;
    public GameObject meleeEnemy;
    public GameObject Medkit;
    public BoxCollider spawnArea;
    public GameObject objectToDestroyWhenSpawnerEnds; // The object to destroy when spawner ends
    public float repeatRate = 3f;
    public float MrepeatRate = 10f;
    public float medkitRepeatRate = 6f; // Separate repeat rate for medkits
    public float maxNavMeshDistance = 2f;
    public float medkitSpawnHeight = 1f; // Height above ground to spawn medkits

    public void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InvokeRepeating(nameof(EnemySpawner), 0.5f, repeatRate);
            InvokeRepeating(nameof(MeleeEnemySpawner),  0.5f, MrepeatRate);
            InvokeRepeating(nameof(MedkitSpawner), 1f, medkitRepeatRate); // Separate invocation for medkits
            StartCoroutine(EndSpawnerAfterDelay(30)); // Coroutine to handle cleanup
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    void EnemySpawner()
    {
        Vector3 randomPosition = GetRandomValidNavMeshPosition();

        if (randomPosition != Vector3.zero)
        {
            Instantiate(enemy, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position to spawn enemy.");
        }
    }
    void MeleeEnemySpawner()
    {
        Vector3 randomPosition = GetRandomValidNavMeshPosition();

        if (randomPosition != Vector3.zero)
        {
            Instantiate(meleeEnemy, randomPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position to spawn enemy.");
        }
    }

    void MedkitSpawner()
    {
        Vector3 medkitPosition = GetRandomValidNavMeshPosition();

        if (medkitPosition != Vector3.zero)
        {
            medkitPosition.y += medkitSpawnHeight; // Adjust height for medkits
            Instantiate(Medkit, medkitPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Could not find a valid NavMesh position to spawn medkit.");
        }
    }

    Vector3 GetRandomValidNavMeshPosition()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = GetRandomPositionInBox(spawnArea);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, maxNavMeshDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }

    Vector3 GetRandomPositionInBox(BoxCollider box)
    {
        Vector3 center = box.bounds.center;
        Vector3 size = box.bounds.size;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomY = center.y;
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }

    IEnumerator EndSpawnerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        CancelInvoke(nameof(EnemySpawner)); // Stop the enemy spawner
        CancelInvoke(nameof(MedkitSpawner)); // Stop the medkit spawner
        CancelInvoke(nameof(MeleeEnemySpawner));

        if (objectToDestroyWhenSpawnerEnds != null)
        {
            Destroy(objectToDestroyWhenSpawnerEnds); // Destroy the specified object
        }

        Destroy(gameObject); // Destroy the spawner itself
    }
}
