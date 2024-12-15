using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI _enemiesKilled;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI ammo;
    public Slider healthBar;
    public Image reloadCircle;
    public Image hitmarker;
    public Image crosshair;
    private float currentReloadProgress;

    public Color crosshairDefaultColor = new Color(1f, 1f, 1f, 1f);
    public Color crosshairHitColor = new Color(1f, 0f, 0f, 1f);

    private RaycastHit hitInfo;

    public int duration = 60;
    public int timeRemaining;
    public bool isCountingDown = false;

    void Start()
    {
        healthBar.maxValue = PlayerMain.health;
        healthBar.value = PlayerMain.health;
        ammo.richText = true;

        Weapon.OnReloadStarted += ReloadStarted;
        Weapon.OnReloadFinished += ReloadFinished;
        Weapon.OnEnemyShot += EnemyShot;

        if (!isCountingDown)
        {
            isCountingDown = true;
            timeRemaining = duration;
            Invoke("_tick", 1f);
        }

    }

    private void _tick()
    {
        timeRemaining--;
        if (timeRemaining > 0)
        {
            Invoke("_tick", 1f);
        }
        else
        {
            isCountingDown = false;
        }
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
        StopCoroutine("DisableHitmarkerCoroutine");
        StartCoroutine("DisableHitmarkerCoroutine");
    }

    IEnumerator DisableHitmarkerCoroutine()
    {
        yield return new WaitForSeconds(0.25f);
        hitmarker.enabled = false;
    }

    void Update()
    {
        healthBar.value = PlayerMain.health;
        ammo.text = $"<color=#ffffff>{Weapon.equippedGun.bulletsLeft}</color><color=#d9d9d9><size=60%>/{Weapon.equippedGun.ammo}</size></color><br><color=#ffffff>{Weapon.equippedGun.name}</color>";
        int minutes = timeRemaining / 60;
        int seconds = timeRemaining % 60;
        timer.text = $"{minutes:D2}'.{seconds:D2}";
        _enemiesKilled.text = $"{Enemy.enemiesKilled}";

        if (Weapon.reloading)
        {
            currentReloadProgress += Time.deltaTime / Weapon.equippedGun.reloadTime;
            reloadCircle.fillAmount = Mathf.Clamp01(currentReloadProgress);
        }

        CheckForEnemyHit();
    }

    void CheckForEnemyHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                crosshair.color = crosshairHitColor;
            }
            else
            {
                crosshair.color = crosshairDefaultColor;
            }
        }
        else
        {
            crosshair.color = crosshairDefaultColor;
        }
    }

    void OnDestroy()
    {
        Weapon.OnReloadStarted -= ReloadStarted;
        Weapon.OnReloadFinished -= ReloadFinished;
        Weapon.OnEnemyShot -= EnemyShot;
    }

    public void Restart()
    {
        SceneManager.LoadScene("temple");
    }
}
