using UnityEngine;
using FMODUnity;

public class PlayerMovement : MonoBehaviour
{
	private CharacterController CC;
	public Camera playerCamera;
	public Animator camAnim;

	public EventReference footstep;

	private Vector3 inputVector;
	private Vector3 movementVector;

	public bool isWalking;
	private float myGravity = -10f;
	public float playerSpeed = 10f;
	public float momentumDamping = 5f;

	public float sensitivity = 1.5f;
	public float smoothing = 1.5f;
	private float xMousePos;
	private float yMousePos;
	private float smoothedMousePos;
	private float currentLookingPos;

	private float time;

	private float fov;
	private float fovSpeed;

	private float moveY;
	private float moveX;

	private Quaternion currentSlerp;
	private Quaternion AddQuaternion;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		CC = GetComponent<CharacterController>();
	}

	void Update()
	{
		moveY = Input.GetAxisRaw("Horizontal");
		moveX = Input.GetAxisRaw("Vertical");

		#region Handles Camera Sway
		if (Mathf.Abs(moveY) > 0.1f)
		{
			if (moveY > 0)
			{
				AddQuaternion = Quaternion.Euler(0, 0, -3f);
			}
			else
			{
				AddQuaternion = Quaternion.Euler(0, 0, 3f);
			}
		}
		else
		{
			AddQuaternion = Quaternion.Euler(0, 0, 0);
		}
		currentSlerp = Quaternion.Slerp(playerCamera.transform.localRotation, AddQuaternion, Time.deltaTime / 0.1f);
		#endregion

		#region Handles Rotation
		xMousePos = Input.GetAxisRaw("Mouse X");
		xMousePos *= sensitivity * smoothing;
		smoothedMousePos = Mathf.Lerp(smoothedMousePos, xMousePos, 1f / smoothing);
		currentLookingPos += smoothedMousePos;
		transform.localRotation = Quaternion.AngleAxis(currentLookingPos, transform.up);

		yMousePos += -Input.GetAxis("Mouse Y") * sensitivity;
		yMousePos = Mathf.Clamp(yMousePos, -70, 70);
		playerCamera.transform.localRotation = Quaternion.Euler(yMousePos, 0, currentSlerp.eulerAngles.z);
		#endregion

		#region Handles Movement
		inputVector = new Vector3(moveY, 0f, moveX);
		inputVector.Normalize();
		inputVector = transform.TransformDirection(inputVector);

		movementVector = (inputVector * playerSpeed) + (Vector3.up * myGravity);
		CC.Move(movementVector * Time.deltaTime);

		if (CC.velocity.magnitude > 0.1f)
		{
			isWalking = true;
		}
		else
		{
			inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);
			isWalking = false;
		}

		camAnim.SetBool("isWalking", isWalking);
		#endregion

		#region Handles FOV
		fov = 90f;
		fovSpeed = 0.3f;

		if (Mathf.Abs(moveX) > 0.1 || Mathf.Abs(moveY) > 0.1)
		{
			fov = 110f;
		}

		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, fov, Time.deltaTime / fovSpeed);
		#endregion

		#region Handles Footstep Sounds
		if (Time.time > time)
		{
			time = Time.time + 0.3f;
			float currentSpeed = CC.velocity.magnitude;
			if (currentSpeed < 3f) return;
			RuntimeManager.PlayOneShot(footstep, this.gameObject.transform.localPosition + new Vector3(0, -3, 0));
		}
		#endregion
	}
}