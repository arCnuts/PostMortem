using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float damage = 5f;
    public float damageInterval = 2f;
    private bool isPlayerInLava = false; 
    private Coroutine damageCoroutine; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerInLava = true;
            if (damageCoroutine == null) 
            {
                damageCoroutine = StartCoroutine(ApplyLavaDamage(other.gameObject));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInLava = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyLavaDamage(GameObject player)
    {
        while (isPlayerInLava)
        {

            PlayerMain health = player.GetComponent<PlayerMain>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }
}
