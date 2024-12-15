using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerMain : MonoBehaviour
{
    public float _health;
    public static float health;
    public GameObject deathScreen;
    public float MedkitPoints;


    void Start()
    {
        int value = Enemy.enemiesKilled;
        health = _health;
        health = Mathf.Clamp(health, 0f, 100f);

    }

    public void TakeDamage(float damage)
    {
        health -= damage;


        if (health <= 0)
        {
            SceneManager.LoadScene("DeathScreen");
            Enemy.enemiesKilled = 0;
            //deathScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MedKit"))
        {
            health += MedkitPoints;
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