using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public float DistRange = 10f;
    public Transform centrePoint;
    public GameObject player;
    public bool chase;
    public float speed;
    public float StopDistance = 10f;
    public int damage = 25;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (chase)
        {
            Chase();
            return; 
        }

        if (dist <= range)
        {
            chase = true;
            Chase();
        }
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void Chase()
    {
        agent.SetDestination(player.transform.position);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().TakeDamage(damage);
        }
    }
}
