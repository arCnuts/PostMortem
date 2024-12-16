using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    public static float health = 100f; // Static variable for health
    public GameObject deathScreen;
    public float MedkitPoints;

    void Start()
    {
        ResetPlayerState(); // Reset health and any other static variables
    }

    private void ResetPlayerState()
    {
        health = 100f; // Reset health to the initial value
        Enemy.enemiesKilled = 0; // Reset other game states if needed
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0f, 100f); // Clamp health after taking damage
    }

    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
            Enemy.enemiesKilled = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (health > 100f)
        {
            Debug.Log("NOOB");
            Debug.Log(health);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MedKit"))
        {
            health += MedkitPoints;
            health = Mathf.Clamp(health, 0f, 100f); // Clamp health after picking up a medkit
            Weapon.Inventory[Weapon.selectedGun].ammo += 48;
            Destroy(other.gameObject);
        }
        //if(other.CompareTag("Ammo"))
        //{
        //    Weapon.Inventory[Weapon.selectedGun].ammo += 500;
        //    Destroy(other.gameObject);
        //}
    }
}
