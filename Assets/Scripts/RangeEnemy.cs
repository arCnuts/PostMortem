using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : Enemy
{
    public float timeBetweenShots = 1f;
    private float lastShotTime;
    public float maxDistanceToPlayer = 10f;
    public float shootInterval = 1f;
    private float lastShootTime;
    public float raycastSpreadAngle = 10f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = FindFirstObjectByType<PlayerMovement>();
        enemy = GetComponent<NavMeshAgent>();
        lastShootTime = Time.time - shootInterval;
        lastShotTime = Time.time;
    }

    void Update()
    {
        enemy.SetDestination(player.transform.position);
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= maxDistanceToPlayer && Time.time - lastShootTime >= shootInterval && Time.time - lastShotTime >= timeBetweenShots)
        {
            ShootAtPlayer();
            lastShootTime = Time.time;
        }
    }
    private void ShootAtPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerScript.TakeDamage(damage);
                lastShotTime = Time.time;
            }
        }
    }
}
