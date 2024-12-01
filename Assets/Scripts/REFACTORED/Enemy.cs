using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enemyType[] enemies = new enemyType[4];
    public NavMeshAgent enemyNavMesh;
    public Transform playerTransform;
    public enemyType currentEnemy;

    public enum attackType
    {
        Melee,
        Range,
        Flying
    }
    public attackType selectedAttackType;
    public float health;
    public float speed;
    public float damage;

    public class enemyType
    {
        public attackType attackType;
        public float health;
        public float speed;
        public float damage;

        public enemyType(attackType _attackType, float _health, float _speed, float _damage)
        {
            attackType = _attackType;
            health = _health;
            speed = _speed;
            damage = _damage;
        }
    }

    float AttackTime;
    private bool hasIndicator = false;  

    void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        AttackDist = enemyNavMesh.stoppingDistance;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentEnemy = new enemyType(selectedAttackType, health, speed, damage);
    }

    void Update()
    {
       
        if (D1_System.CheckIfObjectInSight != null)
        {
            bool inSight = D1_System.CheckIfObjectInSight(transform);

            if (!inSight && !hasIndicator)
            {
               
                D1_System.CreateIndicator?.Invoke(transform);
                hasIndicator = true;
            }
            else if (inSight && hasIndicator)
            {
               
                hasIndicator = false;
            }
        }

        // Attack behavior based on the current enemy's attack type
        switch (currentEnemy.attackType)
        {
            case attackType.Melee:
                MeleeBehavior();
                break;

            case attackType.Range:
                RangeBehavior();
                break;

            case attackType.Flying:
                FlyingBehavior();
                break;
        }
    }

    public void MeleeBehavior()
    {
        float timeBetweenAttacks = 2;
        if (playerTransform != null)
        {
            bool inDistance = Vector3.Distance(transform.position, playerTransform.position) <= AttackDist;
            if (inDistance && Time.time >= AttackTime)
            {
                StartCoroutine(Attack());
                AttackTime = Time.time + timeBetweenAttacks;
            }
            else
            {
                UpdatePath();
            }
        }
    }

    IEnumerator Attack()
    {
        float AttackSpeed = 3;

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = playerTransform.position;
        float percent = 0;

        while (percent <= 1)
        {
            percent += Time.deltaTime * AttackSpeed;
            float formula = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, targetPosition, formula);
            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMain playerMain = other.GetComponent<PlayerMain>();
            playerMain.TakeDamage(damage);
        }
    }

    [Header("RangeEnemy Stats")]
    private float pathUpdateDeadline;
    private float AttackDist;
    public float pathUpdateDelay = 0.2f;
    private float fireRate = 1.0f;
    private float nextShotTime = 1f;

    public void RangeBehavior()
    {
        if (playerTransform != null)
        {
            bool inRange = Vector3.Distance(transform.position, playerTransform.position) <= AttackDist;
            if (inRange)
            {
                Shoot();
            }
            else
            {
                UpdatePath();
            }
        }
    }

    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + pathUpdateDelay;
            enemyNavMesh.SetDestination(playerTransform.position);
        }
    }

    private void Shoot()
    {
        if (Time.time >= nextShotTime)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer, out hit, AttackDist))
            {
                PlayerMain player = hit.collider.GetComponent<PlayerMain>();
                if (player != null)
                {
                    player.TakeDamage(damage);
                    nextShotTime = Time.time + fireRate;
                }
            }
        }
    }

    public void FlyingBehavior()
    {
        // Add flying behavior here
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyKnockBack(float knockbackForce)
    {
        Vector3 knockBackDirection = (transform.position - playerTransform.position).normalized;
        knockBackDirection.y = 0;

        Vector3 knockBackMovement = knockBackDirection * knockbackForce * Time.deltaTime;

        transform.position += knockBackMovement;
    }
}
