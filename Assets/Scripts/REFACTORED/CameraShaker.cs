using DG.Tweening;
using UnityEngine;
using Action = System.Action;

public class CameraShaker : MonoBehaviour
{
	public Transform cameraTransform;
	[SerializeField] private Vector3 cameraPosition;
	[SerializeField] private Vector3 cameraRotation;

	private static event Action Shake;

	public static void Invoke()
	{
		Shake?.Invoke();
	}
	private void OnEnable() => Shake += CameraShake;
	private void OnDisable() => Shake += CameraShake;
	private void CameraShake()
	{
		cameraTransform.DOComplete();
		cameraTransform.DOShakePosition(0.3f, cameraPosition);
		cameraTransform.DOShakeRotation(0.3f, cameraRotation);
	}
}