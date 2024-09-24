using UnityEngine;

public class PhysicalUI : MonoBehaviour
{
    public SpriteRenderer bloodOverlay;
    private float maxHealth;

    public float smooth;
    public float swayAmount;
    private float xMousePos;
    private float yMousePos;
    public Transform weaponAnchorTransform;

    void Start()
    {
        maxHealth = PlayerMain.health;
    }

    void Update()
    {
        float newOpacity = 1f - (PlayerMain.health / maxHealth);
        bloodOverlay.SetSpriteTransparency(Mathf.Lerp(bloodOverlay.color.a, newOpacity, Time.deltaTime * 5f));

        #region  Weapon Sway
        xMousePos = Input.GetAxisRaw("Mouse X");
        yMousePos = Input.GetAxisRaw("Mouse Y");
        float offsetX = xMousePos * swayAmount;
        float offsetY = yMousePos * swayAmount;
        Vector3 targetPosition = new Vector3(-offsetX, -offsetY, 0f);
        weaponAnchorTransform.localPosition = Vector3.Lerp(weaponAnchorTransform.localPosition, targetPosition, smooth * Time.deltaTime);
        #endregion
    }
}