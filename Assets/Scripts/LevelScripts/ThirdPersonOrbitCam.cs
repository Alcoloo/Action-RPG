using UnityEngine;
using System.Collections;
using Rpg;

public class ThirdPersonOrbitCam : MonoBehaviour 
{
	public Transform player;                                           // Player's reference.
	public Vector3 pivotOffset = new Vector3(0.0f, 1.0f,  0.0f);       // Offset to repoint the camera.
	public Vector3 camOffset   = new Vector3(0.0f, 0.7f, -3.0f);       // Offset to relocate the camera related to the player position.
	public float smooth = 10f;                                         // Speed of camera responsiveness.
	public float horizontalAimingSpeed = 400f;                         // Horizontal turn speed.
	public float verticalAimingSpeed = 400f;                           // Vertical turn speed.
	public float maxVerticalAngle = 30f;                               // Camera max clamp angle. 
	public float minVerticalAngle = -60f;                              // Camera min clamp angle.
    public float FOVFactor = 5f;
    public float _minDistance;
    public float _maxDistance;
    public Transform currentBoss;

	private float angleH = 0;                                          // Float to store camera horizontal angle related to mouse movement.
	private float angleV = 0;                                          // Float to store camera vertical angle related to mouse movement.
	private Transform cam;                                             // This transform.
	private Vector3 relCameraPos;                                      // Current camera position relative to the player.
	private float relCameraPosMag;                                     // Current camera distance to the player.
	private Vector3 smoothPivotOffset;                                 // Camera current pivot offset on interpolation.
	private Vector3 smoothCamOffset;                                   // Camera current offset on interpolation.
	private Vector3 targetPivotOffset;                                 // Camera pivot offset target to iterpolate.
	private Vector3 targetCamOffset;                                   // Camera offset target to interpolate.
	private float defaultFOV;                                          // Default camera Field of View.
	private float targetFOV;                                           // Target camera FIeld of View.
	private float targetMaxVerticalAngle;                              // Custom camera max vertical clamp angle. 
    private Quaternion aimRotation;
    private Quaternion camYRotation;
    private delegate void Action();
    private Action doAction; 

	void Awake()
	{
		// Reference to the camera transform.
		cam = transform;

		// Set camera default position.
		cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
		cam.rotation = Quaternion.identity;

        if (Input.GetJoystickNames().Length == 0)
        {
            horizontalAimingSpeed = 600;
            verticalAimingSpeed = 600;
        }

		// Get camera position relative to the player, used for collision test.
		relCameraPos = transform.position - player.position;
		relCameraPosMag = relCameraPos.magnitude - 0.5f;

		// Set up references and default values.
		smoothPivotOffset = pivotOffset;
		smoothCamOffset = camOffset;
		defaultFOV = cam.GetComponent<Camera>().fieldOfView;

		ResetTargetOffsets ();
		ResetFOV ();
		ResetMaxVerticalAngle();
        doAction = DoActionBase;
	}

	void LateUpdate()
	{
        // Get mouse movement to orbit the camera.
        if (Mathf.Abs(ControllerInput.manager.camHorizontal) > 0.1) angleH += Mathf.Clamp(ControllerInput.manager.camHorizontal, -1, 1) * horizontalAimingSpeed * Time.deltaTime;

        if (Mathf.Abs(ControllerInput.manager.camVertical) > 0.1) angleV += Mathf.Clamp(ControllerInput.manager.camVertical, -1, 1) * verticalAimingSpeed * Time.deltaTime;

		// Set vertical movement limit.
		angleV = Mathf.Clamp(angleV, minVerticalAngle, targetMaxVerticalAngle);

		// Set camera orientation..
		camYRotation = Quaternion.Euler(0, angleH, 0);
		aimRotation = Quaternion.Euler(-angleV, angleH, 0);
		cam.rotation = aimRotation;

		// Set FOV.
		cam.GetComponent<Camera>().fieldOfView = Mathf.Lerp (cam.GetComponent<Camera>().fieldOfView, targetFOV,  Time.deltaTime);

		// Test for collision with the environment based on current camera position.
		Vector3 baseTempPosition = player.position + camYRotation * targetPivotOffset;
		Vector3 noCollisionOffset = targetCamOffset;
		for(float zOffset = targetCamOffset.z; zOffset <= 0; zOffset += 0.5f)
		{
			noCollisionOffset.z = zOffset;
			if (DoubleViewingPosCheck (baseTempPosition + aimRotation * noCollisionOffset, Mathf.Abs(zOffset)) || zOffset == 0) 
			{
				break;
			} 
		}

		// Repostition the camera.
		smoothPivotOffset = Vector3.Lerp(smoothPivotOffset, targetPivotOffset, smooth * Time.deltaTime);
		smoothCamOffset = Vector3.Lerp(smoothCamOffset, noCollisionOffset, smooth * Time.deltaTime);

        doAction();
		
	}
    #region DoAction
    private void DoActionBase()
    {
        cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
    }

    private void DoActionBow()
    {
        Transform targetPos = EnemyManager.instance.ennemyNear[0].transform;
        Vector3 camTarget = (player.position - targetPos.position).normalized * (player.position - targetPos.position).magnitude / 2;
        cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
        transform.LookAt(camTarget);
    }

    private void DoActionBoss()
    {
        Vector3 camTarget = (currentBoss.position - player.position).normalized * (currentBoss.position - player.position).magnitude / 2;
        float newFOV = Vector3.Distance(player.position, currentBoss.position) * FOVFactor;
        if (newFOV >= _maxDistance) targetFOV = newFOV;
        else
        {
            targetFOV = defaultFOV;
            Vector3 lerpPos = Vector3.Lerp(transform.position, camTarget,Time.deltaTime * 0.5f);
        }
        cam.position = player.position + camYRotation * smoothPivotOffset + aimRotation * smoothCamOffset;
        
    }

    
    #endregion

    #region SetMode
    public void SetModeBase()
    {
        doAction = DoActionBase;
    }

    public void SetModeBow()
    {
        doAction = DoActionBow;
    }

    public void SetModeBoss()
    {
        doAction = DoActionBoss;
    }
    #endregion

    // Set camera offsets to custom values.
    public void SetTargetOffsets(Vector3 newPivotOffset, Vector3 newCamOffset)
	{
		targetPivotOffset = newPivotOffset;
		targetCamOffset = newCamOffset;
	}

	// Reset camera offsets to default values.
	public void ResetTargetOffsets()
	{
		targetPivotOffset = pivotOffset;
		targetCamOffset = camOffset;
	}

	// Set camera vertical offset.
	public void SetYCamOffset(float y)
	{
		targetCamOffset.y = y;
	}

	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	// Set max vertical camera rotation angle.
	public void SetMaxVerticalAngle(float angle)
	{
		this.targetMaxVerticalAngle = angle;
	}

	// Reset max vertical camera rotation angle to default value.
	public void ResetMaxVerticalAngle()
	{
		this.targetMaxVerticalAngle = maxVerticalAngle;
	}

	// Double check for collisions: concave objects doesn't detect hit from outside, so cast in both directions.
	bool DoubleViewingPosCheck(Vector3 checkPos, float offset)
	{
		float playerFocusHeight = player.GetComponent<CapsuleCollider> ().height *0.5f;
		return ViewingPosCheck (checkPos, playerFocusHeight) && ReverseViewingPosCheck (checkPos, playerFocusHeight, offset);
	}

	// Check for collision from camera to player.
	bool ViewingPosCheck (Vector3 checkPos, float deltaPlayerHeight)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.position+(Vector3.up* deltaPlayerHeight) - checkPos, out hit, relCameraPosMag))
		{
			// ... if it is not the player...
			if(hit.transform != player && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				// This position isn't appropriate.
				return false;
			}
		}
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		return true;
	}

	// Check for collision from player to camera.
	bool ReverseViewingPosCheck(Vector3 checkPos, float deltaPlayerHeight, float maxDistance)
	{
		RaycastHit hit;

		if(Physics.Raycast(player.position+(Vector3.up* deltaPlayerHeight), checkPos - player.position, out hit, maxDistance))
		{
			if(hit.transform != player && hit.transform != transform && !hit.transform.GetComponent<Collider>().isTrigger)
			{
				return false;
			}
		}
		return true;
	}

	// Get camera magnitude.
	public float getCurrentPivotMagnitude(Vector3 finalPivotOffset)
	{
		return Mathf.Abs ((finalPivotOffset - smoothPivotOffset).magnitude);
	}
}
