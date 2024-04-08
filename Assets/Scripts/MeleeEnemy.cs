using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float stopDistance;
    private float AttackTime;
    public float AttackSpeed;
    public float timeBetweenAttacks;
    public void Update()
    {
        if(player != null)
        {
            if(Vector3.Distance(transform.position, player.position) > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else
            {
                if(Time.time >= AttackTime)
                {
                    StartCoroutine(Attack());
                    AttackTime = Time.time + timeBetweenAttacks;
                }
            }
        }   
    }
    IEnumerator Attack()
    {
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = player.position;
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
        if(other.CompareTag("Player"))
        {
            player.GetComponent<PlayerMovement>().TakeDamage(damage);
        }
    }

}
