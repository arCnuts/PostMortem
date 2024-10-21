using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color enemyColor = Color.red;
    public float raycastDistance;

    void Update()
    {
        HandleCrosshair();
    }

    void HandleCrosshair()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Enemy")) 
            {
                crosshairImage.color = enemyColor;  
            }
        }
        else
        {
            crosshairImage.color = normalColor;
        }
    }
}
