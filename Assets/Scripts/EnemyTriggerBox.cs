using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTriggerBox : MonoBehaviour
{
    public GameObject enemy;
    public BoxCollider spawnArea;
    public float repeatRate = 3f;
    public float maxNavMeshDistance = 2f;

    public void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InvokeRepeating(nameof(EnemySpawner), 0.5f, repeatRate);
            Destroy(gameObject, 30); 
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
}
