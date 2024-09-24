using UnityEditor.Animations;
using UnityEngine;
using TMPro;
using FMODUnity;
using UnityEngine.UI;

public class Hands1 : MonoBehaviour
{
	public GameObject bulletHolePrefab;
	public GameObject bulletHoleContainer;
	public GameObject[] enemies;
	public float destroyDelay;

	public AnimatorController pistolAnim;
	public AnimatorController shotgunAnim;
	public AnimatorController uziAnim;
	public Animator hand;
	public Gun[] Inventory = new Gun[4];
	public Gun equippedGun;

	public Transform muzzleFlash;
	public Animator muzzleFlashAnimator;
	public Light muzzleFlashLight;
	public Light muzzleFlashFlash;
	public ParticleSystem gunImpact;
	public GameObject hitParticlePrefab;
	int selectedGun;
	private float flashIntensity;
	private float lightIntensity;

	public TextMeshProUGUI ammoUI;
	bool reloading;
	bool readyToShoot;
	public Image reloadCircle;
	private float currentReloadProgress;

	public class Gun
	{
		public string name;
		public int ammo;
		public int magSize;
		public float spread;
		public AnimatorController ac;
		public bool acquired;
		public int bulletsLeft;
		public int pellets;
		public float shootingCooldown;
		public float reloadTime;
		public EventReference shootSound;
		public float damage;
		public float KnockBackForce;
		public bool fullAuto;

		public Gun(string Name, int Ammo, int MagSize, float Spread, float ReloadTime, float ShootingCooldown, float Damage, float Knockback, AnimatorController AC, EventReference ShootSound, bool Acquired, int Pellets = 1, bool FullAuto = false)
		{
			name = Name;
			ammo = Ammo;
			magSize = MagSize;
			spread = Spread;
			reloadTime = ReloadTime;
			shootingCooldown = ShootingCooldown;

			ac = AC;
			acquired = Acquired;

			bulletsLeft = magSize;

			pellets = Pellets;

			shootSound = ShootSound;

			damage = Damage;

			fullAuto = FullAuto;

			KnockBackForce = Knockback;

		}
	}

	void Start()
	{
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		// Gun pistol = new Gun("Pistol", 24, 12, 0f, 0.25f, 0.2f, 30f, 400f, pistolAnim, pistolShot, true, 1);
		// Gun shotgun = new Gun("Shotgun", 10, 3, 0.1f, 0.5f, 0.5f, 10f, 250f, shotgunAnim, shotgunShot, true, 6);
		// Gun subMachineGun = new Gun("uzi", 48, 12, 0.1f, 0.3f, 0.1f, 12f, 400f, uziAnim, pistolShot, true, 1, true);
		// Gun machineGun = new Gun("Pistol", 400, 12, 1.2f, 0.5f, 1f, 1f, 250f, null, pistolShot, false);

		// Inventory[0] = pistol;
		// Inventory[1] = shotgun;
		// Inventory[2] = subMachineGun;
		// Inventory[3] = machineGun;

		readyToShoot = true;
	}

	void Update()
	{
		equippedGun = Inventory[selectedGun];
		if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory[0].acquired) selectedGun = 0;
		if (Input.GetKeyDown(KeyCode.Alpha2) && Inventory[1].acquired) selectedGun = 1;
		if (Input.GetKeyDown(KeyCode.Alpha3) && Inventory[2].acquired) selectedGun = 2;
		if (Input.GetKeyDown(KeyCode.Alpha4) && Inventory[3].acquired) selectedGun = 3;
		int nextSelectedGun = selectedGun + (int)Input.mouseScrollDelta.y;
		if (nextSelectedGun < 0)
		{
			nextSelectedGun = Inventory.Length - 1;
		}
		else if (nextSelectedGun >= Inventory.Length)
		{
			nextSelectedGun = 0;
		}

		while (!Inventory[nextSelectedGun].acquired)
		{
			nextSelectedGun += (int)Input.mouseScrollDelta.y;
			if (nextSelectedGun < 0)
			{
				nextSelectedGun = Inventory.Length - 1;
			}
			else if (nextSelectedGun >= Inventory.Length)
			{
				nextSelectedGun = 0;
			}
		}

		// for (int i = 0; i < selectedGun; i++)
		// {
		// 	if (Inventory[i].acquired)
		// 	{
		// 		uim.slot[i].sprite = uim.slotSprite[1];
		// 	}
		// 	else
		// 	{
		// 		uim.slot[i].sprite = uim.slotSprite[0];
		// 	}
		// }

		selectedGun = nextSelectedGun;

		hand.runtimeAnimatorController = equippedGun.ac;

		flashIntensity = muzzleFlashFlash.intensity;
		lightIntensity = muzzleFlashLight.intensity;

		if (Input.GetKeyDown(KeyCode.R) && equippedGun.bulletsLeft < equippedGun.magSize && !reloading) Reload();
		if (Input.GetMouseButtonDown(0) && readyToShoot && !reloading && !equippedGun.fullAuto)
		{
			if (equippedGun.bulletsLeft > 0)
			{
				Shoot();
			}
			else
			{
				readyToShoot = false;
				// RuntimeManager.PlayOneShot(emptyClip);
				Invoke("ResetShot", equippedGun.shootingCooldown);
			}
		}
		if (Input.GetMouseButton(0) && readyToShoot && !reloading && equippedGun.fullAuto)
		{
			if (equippedGun.bulletsLeft > 0)
			{
				Shoot();
			}
			else
			{
				readyToShoot = false;
				// RuntimeManager.PlayOneShot(emptyClip);
				Invoke("ResetShot", equippedGun.shootingCooldown);
			}
		}

		if (reloading)
		{
			currentReloadProgress += Time.deltaTime / equippedGun.reloadTime;
			reloadCircle.fillAmount = Mathf.Clamp01(currentReloadProgress);
		}


		muzzleFlashLight.intensity = Mathf.Lerp(lightIntensity, 0f, Time.deltaTime * 10f);
		muzzleFlashFlash.intensity = Mathf.Lerp(flashIntensity, 0f, Time.deltaTime * 15f);
	}

	void Shoot()
	{
		bool isFullAuto = equippedGun.fullAuto;
		readyToShoot = false;

		muzzleFlashAnimator.Play("flashOn");
		hand.Play("gunShoot");
		CameraShaker.Invoke();

		flashIntensity = 0.3f;
		lightIntensity = 300f;

		RuntimeManager.PlayOneShot(equippedGun.shootSound);

		for (int i = 0; i < equippedGun.pellets; i++)
		{
			float y = Random.Range(-equippedGun.spread, equippedGun.spread);
			float x = Random.Range(-equippedGun.spread, equippedGun.spread);

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray.origin, ray.direction + new Vector3(x, y, 0), out hit))
			{
				if (hit.collider.CompareTag("Enemy"))
				{
					Enemy enemy = hit.collider.GetComponent<Enemy>();
					if (enemy != null)
					{
						enemy.TakeDamage(equippedGun.damage);
						ApplyKnockBack(enemy, equippedGun.KnockBackForce);

						if (hitParticlePrefab != null)
						{

							GameObject particleInstance = Instantiate(hitParticlePrefab, hit.point, Quaternion.identity);
							particleInstance.transform.LookAt(hit.point + hit.normal);


							Destroy(particleInstance, 2f);
						}
					}
				}

				gunImpact.transform.position = hit.point;
				gunImpact.transform.forward = hit.normal;
				gunImpact.Emit(1);


				SpawnBulletHole(hit, ray);
			}
		}

		// if(equippedGun.fullAuto)
		// {
		// 	Invoke("Shoot", 0.1f);
		// }

		equippedGun.bulletsLeft--;

		Invoke("ResetShot", equippedGun.shootingCooldown);

	}
	void ApplyKnockBack(Enemy enemy, float knockbackForce)
	{

		Vector3 knockBackDirection = (enemy.transform.position - transform.position).normalized;
		knockBackDirection.y = 0;

		Vector3 knockBackMovement = knockBackDirection * knockbackForce * Time.deltaTime;

		enemy.transform.position += knockBackMovement;
	}

	void SpawnBulletHole(RaycastHit hit, Ray ray)
	{
		float positionMultiplier = 0.5f;
		float spawnX = hit.point.x - ray.direction.x * positionMultiplier;
		float spawnY = hit.point.y - ray.direction.y * positionMultiplier;
		float spawnZ = hit.point.z - ray.direction.z * positionMultiplier;
		Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);

		GameObject spawnedObject = Instantiate(bulletHolePrefab, spawnPosition, Quaternion.identity);
		Quaternion targetRotation = Quaternion.LookRotation(ray.direction);

		spawnedObject.transform.rotation = targetRotation;
		spawnedObject.transform.SetParent(bulletHoleContainer.transform);
		spawnedObject.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
		Destroy(spawnedObject, destroyDelay);
	}

	private void ResetShot()
	{
		readyToShoot = true;
	}

	private void Reload()
	{
		reloading = true;
		currentReloadProgress = 0f; // Reset progress
		reloadCircle.fillAmount = 0f; // Reset UI
		Invoke("ReloadFinished", equippedGun.reloadTime);
	}

	private void ReloadFinished()
	{
		if (Inventory[selectedGun].ammo >= Inventory[selectedGun].magSize - Inventory[selectedGun].bulletsLeft)
		{
			Inventory[selectedGun].ammo -= Inventory[selectedGun].magSize - Inventory[selectedGun].bulletsLeft;
			Inventory[selectedGun].bulletsLeft = Inventory[selectedGun].magSize;
		}
		else
		{
			Inventory[selectedGun].bulletsLeft += Inventory[selectedGun].ammo;
			Inventory[selectedGun].ammo = 0;
		}
		reloading = false;
		reloadCircle.fillAmount = 0f;

	}

}