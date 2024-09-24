using Unity.VisualScripting;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    public float _health;
    public static float health;
    public GameObject deathScreen;

    void Start()
    {
        health = _health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log(health);

        if (health <= 0)
        {
            Destroy(gameObject);
            deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}