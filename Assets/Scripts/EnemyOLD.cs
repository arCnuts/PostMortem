using UnityEngine;
using FMODUnity;
using UnityEngine.AI;

public class EnemyOLD : MonoBehaviour
{
    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public NavMeshAgent enemy;
    [HideInInspector]
    public PlayerMovement playerScript;

    public float enemyHealth;
    public int damage;
    public float speed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = FindFirstObjectByType<PlayerMovement>();
        enemy = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

    }
}
