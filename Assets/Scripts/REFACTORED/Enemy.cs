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

    void Start()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        currentEnemy = new enemyType(selectedAttackType, health, speed, damage);
    }

    void Update()
    {
        currentEnemy.health = health;

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
        float stopDistance = 5;
        float timeBetweenAttacks = 2;
        if (playerTransform != null)
            if (Vector3.Distance(transform.position, playerTransform.position) > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            }
            else
            {
                if (Time.time >= AttackTime)
                {
                    StartCoroutine(Attack());
                    AttackTime = Time.time + timeBetweenAttacks;
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

    public void RangeBehavior()
    {
    }
    public void FlyingBehavior()
    {
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (currentEnemy.health <= 0)
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