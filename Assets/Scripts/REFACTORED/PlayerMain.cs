using Unity.VisualScripting;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float _health;
    public static float health;
    public GameObject deathScreen;
    public float MedkitPoints;

    void Start()
    {
        health = _health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        //Debug.Log(health);

        if (health <= 0)
        {
            Destroy(gameObject);
            deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("MedKit"))
        {
            health += MedkitPoints;
            if(health > 100)
            {
                health = 100;
            }

            //Debug.Log(health);
            Destroy(other.gameObject);
        }
    }
}