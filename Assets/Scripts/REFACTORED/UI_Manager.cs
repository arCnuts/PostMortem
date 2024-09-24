using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI ammo;
    public Slider healthBar;
    public Image reloadCircle;
    public Image hitmarker;
    private float currentReloadProgress;

    void Start()
    {
        healthBar.maxValue = healthBar.value = PlayerMain.health;
        ammo.richText = true;

        Weapon.OnReloadStarted += ReloadStarted;
        Weapon.OnReloadFinished += ReloadFinished;
        Weapon.OnEnemyShot += EnemyShot;
    }

    void ReloadStarted()
    {
        currentReloadProgress = 0f;
        reloadCircle.fillAmount = 0f;
    }

    void ReloadFinished()
    {
        reloadCircle.fillAmount = 0f;
    }

    void EnemyShot()
    {
        hitmarker.enabled = true;
        Invoke("DisableHitmarker", 0.25f);
    }

    void DisableHitmarker()
    {
        hitmarker.enabled = false;
    }

    void Update()
    {
        healthBar.value = PlayerMain.health;
        ammo.text = $"<color=#ffffff>{Weapon.equippedGun.bulletsLeft}<color=#d9d9d9><size=60%>/{Weapon.equippedGun.ammo}<br><color=#ffffff>{Weapon.equippedGun.name}";

        if (Weapon.reloading)
        {
            currentReloadProgress += Time.deltaTime / Weapon.equippedGun.reloadTime;
            reloadCircle.fillAmount = Mathf.Clamp01(currentReloadProgress);
        }

    }
}