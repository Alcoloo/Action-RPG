﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Rpg.Characters;
using Rpg;
using Rpg.Manager;
using Rpg.Controller;
using Assets.Scripts.Game;

public enum Weapon 
{
	UNARMED = 0,
	TWOHANDSWORD = 1,
	TWOHANDSPEAR = 2,
	TWOHANDAXE = 3,
	TWOHANDBOW = 4,
	TWOHANDCROSSBOW = 5,
	STAFF = 6,
	ARMED = 7,
	RELAX = 8,
	RIFLE = 9,
	TWOHANDCLUB = 10,
	SHIELD = 11
}

public enum RPGCharacterState
{
	DEFAULT,
	BLOCKING,
	STRAFING,
	CLIMBING,
	SWIMMING,
    CINEMATIC
}
	
public class RPGCharacterController : MonoBehaviour 
{
	#region Variables

    private ComboManager m_combo= new ComboManager();
    private static RPGCharacterController _instance;
    public static RPGCharacterController instance { get { return _instance; } }
    //Components
    [HideInInspector]
	public Rigidbody rb;
	public Animator animator;
	public GameObject target;
	[HideInInspector]
	public Vector3 targetDashDirection;
	CapsuleCollider capCollider;
	ParticleSystem FXSplash;
	public Camera sceneCamera;
	public Vector3 waistRotationOffset;
	public RPGCharacterState rpgCharacterState = RPGCharacterState.DEFAULT;
    private ThirdPersonOrbitCam camScript;         // Reference to the third person camera script.

    //jumping variables
    public float gravity = -9.8f;
	[HideInInspector]
	public float gravityTemp = 0f;
	[HideInInspector]
	public bool canJump;
	bool isJumping = false;
	[HideInInspector]
	public bool isGrounded;
	public float jumpSpeed = 12;
	public float doublejumpSpeed = 12;
	bool doJump = false;
	bool doublejumping = true;
	[HideInInspector]
	public bool canDoubleJump = false;
	[HideInInspector]
	public bool isDoubleJumping = false;
	bool doublejumped = false;
	bool isFalling;
	bool startFall;
	float fallingVelocity = -1f;

	// Used for continuing momentum while in air
	public float inAirSpeed = 8f;
	float maxVelocity = 2f;
	float minVelocity = -2f;

	//rolling variables
	public float rollSpeed = 8;
	bool isRolling = false;
	public float rollduration;

	//movement variables
	[HideInInspector]
	public bool isMoving = false;
//	[HideInInspector]
	public bool canMove = true;
	public float walkSpeed = 1.35f;
	float moveSpeed;
	public float runSpeed = 6f;
	float rotationSpeed = 40f;
  
	float x;
	float z;
	float dv;
	float dh;
	Vector3 inputVec;
	Vector3 newVelocity;

	//Weapon and Shield
	public Weapon weapon;
	[HideInInspector]
	public int rightWeapon = 0;
	[HideInInspector]
   public int leftWeapon = 0;
	[HideInInspector]
	public bool isRelax = false;

	//isStrafing/action variables
	[HideInInspector]
	public bool canAction = true;
	bool isStrafing = false;
	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	public bool isBlocking = false;
	public float knockbackMultiplier = 1f;
	bool isKnockback;
	[HideInInspector]
	public bool isSitting = false;
	bool isAiming = false;
	bool isClimbing = false;
	[HideInInspector]
	public bool isNearLadder = false;
	[HideInInspector]
	public bool isNearCliff = false;
	[HideInInspector]
	public GameObject ladder;
	[HideInInspector]
	public GameObject cliff;

	//Swimming variables
	public float inWaterSpeed = 8f;

	//Weapon Models
	
	public GameObject twoHandBow;

	public GameObject swordL;
	public GameObject swordR;
  

    private bool _attackLeft = false;

    private Caracteristic carac;
    private WeaponController weaponController; 

    public KIND currentKind;

    public float animationSpeed;
    #endregion

    #region Initialization
    void Awake() 
	{
        //set the animator component
        _instance = this;
        animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody>();
		capCollider = GetComponent<CapsuleCollider>();
		FXSplash = transform.GetChild(2).GetComponent<ParticleSystem>();
		//hide all weapons
		
		if(twoHandBow != null)
		{
			twoHandBow.SetActive(false);
		}
		if(swordL != null)
		{
			swordL.SetActive(false);
		}
		if(swordR != null)
		{
			swordR.SetActive(false);
		}
	}

	#endregion
	void Start()
    {
        animator.speed = animationSpeed;
        carac = GetComponent<Caracteristic>();
        weaponController = GetComponent<WeaponController>();
        if(carac == null)
        {
            gameObject.AddComponent<Caracteristic>();
            carac = GetComponent<Caracteristic>();
        }
        if(weaponController == null)
        {
            gameObject.AddComponent<WeaponController>();
            weaponController = GetComponent<WeaponController>();
        }
        m_combo.initCombo();
        StartCoroutine(_DoubleWeapon());
        carac.isDeath.AddListener(Death);
        carac.isHit.AddListener(BeHit);
    }
	#region UpdateAndInput
	
	void Update()
	{

        animator.speed = animationSpeed;
        if (rpgCharacterState == RPGCharacterState.CINEMATIC) return;
		//input abstraction for easier asset updates using outside control schemes
		bool inputJump = Input.GetButtonDown("Jump");
		bool inputLightHit = Input.GetButtonDown("LightHit");
		bool inputDeath = Input.GetButtonDown("Death");
		bool inputUnarmed = Input.GetButtonDown("Unarmed");
		bool inputShield = Input.GetButtonDown("Shield");
		bool inputAttackL = Input.GetButtonDown("AttackL");
		bool inputAttackR = Input.GetButtonDown("AttackR");
		bool inputCastL = Input.GetButtonDown("CastL");
		bool inputCastR = Input.GetButtonDown("CastR");
		float inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
		float inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
		bool inputStrafe = Input.GetKey(KeyCode.LeftShift);
		float inputTargetBlock = Input.GetAxisRaw("TargetBlock");
		float inputHorizontal = Input.GetAxisRaw("Horizontal");
		float inputVertical = Input.GetAxisRaw("Vertical");
		bool inputAiming = Input.GetButtonDown("Aiming");
		//Camera relative movement
		Transform cameraTransform = sceneCamera.transform;
        camScript = cameraTransform.GetComponent<ThirdPersonOrbitCam>();
        //Forward vector relative to the camera along the x-z plane   
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
        //directional inputs
        //dv = 0;//inputDashVertical;
               //dh = inputDashHorizontal;
        if (inputLightHit)
        {
            dh = inputHorizontal;
            dv = inputVertical ;
            Debug.Log("dh: " + dh);
            Debug.Log("dv: " + dv);
        }
		if(!isRolling && !isAiming)
		{
            if (dv == 0 && dh == 0) targetDashDirection = 0.5f * -transform.forward;
            else targetDashDirection = dh * right + dv * forward;
          
		}
		x = inputHorizontal;
		z = inputVertical;
		inputVec = x * right + z * forward;
		//make sure there is animator on character
		if(animator)
		{
			if(canMove && !isBlocking && !isDead)
			{

			}
			else
			{
				inputVec = new Vector3(0, 0, 0);
			}
			if(inputCastL)
			{
				doJump = true;
			} 
			else
			{
				doJump = false;
			}
			if(rpgCharacterState != RPGCharacterState.SWIMMING)
			{
				Rolling();
				Jumping();
				Blocking();
			}
			if(inputUnarmed && canAction && isGrounded && !isBlocking && weapon != Weapon.UNARMED)
			{
				StartCoroutine(_SwitchWeapon(0));
			}
			if(inputShield && canAction && isGrounded && !isBlocking && leftWeapon != 7)
			{
				StartCoroutine(_SwitchWeapon(7));
			}

			if(inputAttackL && canAction && isGrounded && !isBlocking)
			{
                
                if(currentKind != KIND.demonic)
                {
                    // animationSpeed = 1000; 
                    StartCoroutine(_DoubleWeapon());
                }
                else if (weapon != Weapon.UNARMED )
                {
                    if (_attackLeft)
                    {
                        Attack(m_combo.Combo("X"));
                        _attackLeft = !_attackLeft;
                    }
                    else
                    {
                        Attack(m_combo.Combo("X"));
                        _attackLeft = !_attackLeft;
                    }
                }	
			}
            if (inputCastR && canAction && isGrounded && !isBlocking)
            {
                if (currentKind != KIND.angelic)
                {
                    //animationSpeed = 1000;
                    SwitchWeaponTwoHand(0);
                }
                else if (weapon != Weapon.UNARMED )
                {
                    if (_attackLeft)
                    {
                        Attack(m_combo.Combo("B"));
                        _attackLeft = !_attackLeft;
                    }
                    else
                    {
                        Attack(m_combo.Combo("B"));
                        _attackLeft = !_attackLeft;
                    }
                }
            }
          /*  if (inputSwitchUpDown < -0.1f && canAction && !isBlocking && isGrounded)
			{  
				SwitchWeaponTwoHand(0);
			}
			else if(inputSwitchUpDown > 0.1f && canAction && !isBlocking && isGrounded)
			{  
				SwitchWeaponTwoHand(1);
			}
			if(inputSwitchLeftRight < -0.1f && canAction && !isBlocking && isGrounded)
			{
                StartCoroutine(_DoubleWeapon());
                //SwitchWeaponLeftRight(0);
			}
			else if(inputSwitchLeftRight > 0.1f && canAction && !isBlocking && isGrounded)
			{
                StartCoroutine(_DoubleWeapon());
				//SwitchWeaponLeftRight(1);
			}*/
            //if strafing 
            /*if(inputStrafe || inputTargetBlock > 0.1f && canAction && weapon != Weapon.RIFLE)
			{  
				isStrafing = true;
				animator.SetBool("Strafing", true);
				if(inputCastL && canAction && isGrounded && !isBlocking)
				{
					CastAttack(1);
				}
				if(inputCastR && canAction && isGrounded && !isBlocking)
				{
					CastAttack(2);
				}
			}
			else
			{  
				isStrafing = false;
				animator.SetBool("Strafing", false);
			}*/
            //Shooting
            /*if (weapon == Weapon.RIFLE)
			{
				if(Input.GetMouseButtonDown(0))
				{
					animator.SetTrigger("Attack1Trigger");
				}
				if(Input.GetMouseButtonDown(2))
				{
					animator.SetTrigger("ReloadTrigger");
				}
				if(inputAiming)
				{
					if(!isAiming)
					{
						isAiming = true;
						animator.SetBool("Aiming", true);
					}
					else
					{
						isAiming = false;
						animator.SetBool("Aiming", false);
					}
				}
			}*/
			//Climbing
			if(rpgCharacterState == RPGCharacterState.CLIMBING && !isClimbing)
			{
				if(inputVertical > 0.1f)
				{
					animator.applyRootMotion = true;
					animator.SetTrigger("Climb-UpTrigger");
					isClimbing = true;
				}
				if(inputVertical < -0.1f)
				{
					animator.applyRootMotion = true;
					animator.SetTrigger("Climb-DownTrigger");
					isClimbing = true;
				}
			}
			if(rpgCharacterState == RPGCharacterState.CLIMBING && isClimbing)
			{
				if(inputVertical == 0)
				{
					isClimbing = false;
				}
			}
		}
		else
		{
			Debug.Log("ERROR: There is no animator for character.");
		}
		if(Input.GetKeyDown(KeyCode.T))
		{
			if(Time.timeScale != 1)
			{
				Time.timeScale = 1;
			}
			else
			{
				Time.timeScale = 0.15f;
			}
		}
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(Time.timeScale != 1)
			{
				Time.timeScale = 1;
			}
			else
			{
				Time.timeScale = 0f;
			}
		}

        m_combo.UpdateCombo();
	}
	
	#endregion

	#region Fixed/Late Updates
	
	void FixedUpdate()
	{
		if(rpgCharacterState != RPGCharacterState.SWIMMING)
		{
			CheckForGrounded();
			//apply gravity force
			rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
			//check if character can move
			if(canMove && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING)
			{
				AirControl();
			}
			//check if falling
			if(rb.velocity.y < fallingVelocity && rpgCharacterState != RPGCharacterState.CLIMBING)
			{
				isFalling = true;
				animator.SetInteger("Jumping", 2);
				canJump = false;
			} 
			else
			{
				isFalling = false;
			}
		} 
		else
		{
			WaterControl();
		}
		moveSpeed = UpdateMovement();
	}

	//get velocity of rigid body and pass the value to the animator to control the animations
	void LateUpdate()
	{
		//Get local velocity of charcter
		float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
		float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
		//Update animator with movement values
		animator.SetFloat("Velocity X", velocityXel / runSpeed);
		animator.SetFloat("Velocity Z", velocityZel / runSpeed);
		//if character is alive and can move, set our animator
		if(!isDead && canMove)
		{
			if(moveSpeed > 0)
			{
				animator.SetBool("Moving", true);
				isMoving = true;
			}
			else
			{
				animator.SetBool("Moving", false);
				isMoving = false;
			}
		}
	}
	
	#endregion

	#region UpdateMovement

	float UpdateMovement()
	{
		Vector3 motion = inputVec;
		if(isGrounded && rpgCharacterState != RPGCharacterState.CLIMBING)
		{
			//reduce input for diagonal movement
			if(motion.magnitude > 1)
			{
				motion.Normalize();
			}
			if(canMove && !isBlocking)
			{
				//set speed by walking / running
				if(isStrafing && !isAiming)
				{
					newVelocity = motion * walkSpeed;
				}
				else
				{
					newVelocity = motion * runSpeed;
				}
				//if rolling use rolling speed and direction
				if(isRolling)
				{
					//force the dash movement to 1
					targetDashDirection.Normalize();
					newVelocity = rollSpeed * targetDashDirection;
				}
			}
		}
		else
		{
			if(rpgCharacterState != RPGCharacterState.SWIMMING)
			{
				//if we are falling use momentum
				newVelocity = rb.velocity;
			} 
			else
			{
				newVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			}
		}
		if(isStrafing && !isRelax)
		{
			//make character point at target
			Quaternion targetRotation;
			Vector3 targetPos = target.transform.position;
			targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
		} 
		else if(isAiming)
		{
			Aiming();
		}
		else
		{
			RotateTowardsMovementDir();
		}
		//if we are falling use momentum
		newVelocity.y = rb.velocity.y;
		rb.velocity = newVelocity;
		//return a movement value for the animator
		return inputVec.magnitude;
	}

	//rotate character towards direction moved
	void RotateTowardsMovementDir()
	{
		if(inputVec != Vector3.zero && !isStrafing && !isAiming && !isRolling && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	#endregion

	#region Aiming

	void Aiming()
	{
		for(int i = 0; i < Input.GetJoystickNames().Length; i++) 
		{
			//if the right joystick is moved, use that for facing
			float inputDashVertical = Input.GetAxisRaw("DashVertical");
			float inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
			if(Mathf.Abs(inputDashHorizontal) > 0.1 || Mathf.Abs(inputDashVertical) < -0.1)
			{
				Vector3 joyDirection = new Vector3(inputDashHorizontal, 0 , -inputDashVertical);
				joyDirection = joyDirection.normalized;
				Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
				transform.rotation = joyRotation;
			}
		}
		//no joysticks, use mouse aim
		if(Input.GetJoystickNames().Length == 0)
		{
		Plane characterPlane = new Plane(Vector3.up, transform.position);
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Vector3 mousePosition = new Vector3(0,0,0);
		float hitdist = 0.0f;
		if(characterPlane.Raycast(ray, out hitdist))
		{
			mousePosition = ray.GetPoint(hitdist);
		}
		mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
		Vector3 relativePos = transform.position - mousePosition;
		Quaternion rotation = Quaternion.LookRotation(-relativePos);
		transform.rotation = rotation;
		}
	}

	#endregion

	#region Swimming

	void OnTriggerEnter(Collider collide)
	{
		//If entering a water volume
		if(collide.gameObject.layer == 4){
			/*rpgCharacterState = RPGCharacterState.SWIMMING;
			canAction = false;
			rb.useGravity = false;
			animator.SetTrigger("SwimTrigger");
			animator.SetBool("Swimming", true);
			animator.SetInteger("Weapon", 0);
			weapon = Weapon.UNARMED;
			StartCoroutine(_WeaponVisibility(leftWeapon, 0, false));
			StartCoroutine(_WeaponVisibility(rightWeapon, 0, false));
			animator.SetInteger("RightWeapon", 0);
			animator.SetInteger("LeftWeapon", 0);
			animator.SetInteger("LeftRight", 0);
			FXSplash.Emit(30);*/
		}
		else if(collide.transform.name.Contains("Cliff"))
		{
			isNearCliff = true;
			cliff = collide.gameObject;
		}
	}

	void OnTriggerExit(Collider collide)
	{
		//If leaving a water volume
		if(collide.gameObject.layer == 4)
		{
			rpgCharacterState = RPGCharacterState.DEFAULT;
			canAction = true;
			rb.useGravity = true;
			animator.SetInteger("Jumping", 2);
			animator.SetBool("Swimming", false);
			capCollider.radius = 0.5f;
		}
		//If leaving a ladder
		else if(collide.transform.parent.name.Contains("Ladder"))
		{
			isNearLadder = false;
			ladder = null;
		}
	}

	void WaterControl()
	{
		AscendDescend();
		Vector3 motion = inputVec;
		//dampen vertical water movement
		Vector3 dampenVertical = new Vector3(rb.velocity.x, (rb.velocity.y * 0.985f), rb.velocity.z);
		rb.velocity = dampenVertical;
		Vector3 waterDampen = new Vector3((rb.velocity.x * 0.98f), rb.velocity.y, (rb.velocity.z * 0.98f));
		//If swimming, don't dampen movement, and scale capsule collider
		if(moveSpeed < 0.1f)
		{
			rb.velocity = waterDampen;
			capCollider.radius = 0.5f;
		} 
		else
		{
			capCollider.radius = 1.5f;
		}
		rb.velocity = waterDampen;
		//clamp diagonal movement so its not faster
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f:1;
		rb.AddForce(motion * inWaterSpeed, ForceMode.Acceleration);
		//limit the amount of velocity we can achieve to water speed
		float velocityX = 0;
		float velocityZ = 0;
		if(rb.velocity.x > inWaterSpeed)
		{
			velocityX = GetComponent<Rigidbody>().velocity.x - inWaterSpeed;
			if(velocityX < 0)
			{
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.x < minVelocity)
		{
			velocityX = rb.velocity.x - minVelocity;
			if(velocityX > 0)
			{
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.z > inWaterSpeed)
		{
			velocityZ = rb.velocity.z - maxVelocity;
			if(velocityZ < 0)
			{
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
		if(rb.velocity.z < minVelocity)
		{
			velocityZ = rb.velocity.z - minVelocity;
			if(velocityZ > 0)
			{
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
	}

	void AscendDescend()
	{
		if(doJump)
		{
			//swim down with left control
			if(isStrafing)
			{
				animator.SetBool("Strafing", true);
				animator.SetTrigger("JumpTrigger");
				rb.velocity -=  inWaterSpeed * Vector3.up;
			} 
			else
			{
				animator.SetTrigger("JumpTrigger");
				rb.velocity +=  inWaterSpeed * Vector3.up;
			}
		}
	}

	#endregion

	#region Jumping

	//checks if character is within a certain distance from the ground, and markes it IsGrounded
	void CheckForGrounded()
	{
		float distanceToGround;
		float threshold = .45f;
		RaycastHit hit;
		Vector3 offset = new Vector3(0, 0.4f,0);
		if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
		{
			distanceToGround = hit.distance;
			if(distanceToGround < threshold)
			{
				isGrounded = true;
				canJump = true;
				startFall = false;
				doublejumped = false;
				canDoubleJump = false;
				isFalling = false;
				if(!isJumping) 
				{
					animator.SetInteger("Jumping", 0);
				}
				//exit climbing on ground
				if(rpgCharacterState == RPGCharacterState.CLIMBING)
				{
					animator.SetTrigger("Climb-Off-BottomTrigger");
					gravity = gravityTemp;
					rb.useGravity = true;
					rpgCharacterState = RPGCharacterState.DEFAULT;
				}
			}
			else
			{
				isGrounded = false;
			}
		}
	}

	void Jumping()
	{
		if(isGrounded)
		{
			if(canJump && doJump)
			{
				StartCoroutine(_Jump());
			}
		}
		/*else
		{    
			canDoubleJump = true;
			canJump = false;
			if(isFalling)
			{
				//set the animation back to falling
				animator.SetInteger("Jumping", 2);
				//prevent from going into land animation while in air
				if(!startFall)
				{
					animator.SetTrigger("JumpTrigger");
					startFall = true;
				}
			}
			if(canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling)
			{
				// Apply the current movement to launch velocity
				rb.velocity += doublejumpSpeed * Vector3.up;
				animator.SetInteger("Jumping", 3);
				doublejumped = true;
			}
		}*/
	}

	public IEnumerator _Jump()
	{
		isJumping = true;
		animator.SetInteger("Jumping", 1);
		animator.SetTrigger("JumpTrigger");
		// Apply the current movement to launch velocity
		rb.velocity += jumpSpeed * Vector3.up;
		canJump = false;
		yield return new WaitForSeconds(0.5f);
		isJumping = false;
	}

	void AirControl()
	{
		if(!isGrounded)
		{
			Vector3 motion = inputVec;
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f:1;
			rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
			//limit the amount of velocity we can achieve
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > maxVelocity)
			{
				velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
				if(velocityX < 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity)
			{
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0)
				{
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > maxVelocity)
			{
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity)
			{
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0)
				{
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}
	}

	#endregion

	#region MiscMethods

	public void Climbing()
	{
		rpgCharacterState = RPGCharacterState.CLIMBING;
	}

	public void EndClimbing()
	{
		rpgCharacterState = RPGCharacterState.DEFAULT;
		gravity = gravityTemp;
		rb.useGravity = true;
		animator.applyRootMotion = false;
		canMove = true;
		isClimbing = false;
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	//weaponNumber 0 = Unarmed
	//weaponNumber 1 = 2H Sword
	//weaponNumber 2 = 2H Spear
	//weaponNumber 3 = 2H Axe
	//weaponNumber 4 = 2H Bow
	//weaponNumber 5 = 2H Crowwbow
	//weaponNumber 6 = 2H Staff
	//weaponNumber 7 = Shield
	//weaponNumber 8 = L Sword
	//weaponNumber 9 = R Sword
	//weaponNumber 10 = L Mace
	//weaponNumber 11 = R Mace
	//weaponNumber 12 = L Dagger
	//weaponNumber 13 = R Dagger
	//weaponNumber 14 = L Item
	//weaponNumber 15 = R Item
	//weaponNumber 16 = L Pistol
	//weaponNumber 17 = R Pistol
	//weaponNumber 18 = Rifle
	//weaponNumber 19 == Right Spear
	//weaponNumber 20 == 2H Club
	public void Attack(Attack lAttack)
	{
        int attackSide = lAttack.side;
        int attackNumber = lAttack.number;
        if (canAction)
		{
			if(weapon == Weapon.ARMED)
			{
				if(isGrounded)
				{
					if(attackSide != 3)
					{
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						StartCoroutine(_LockMovementAndAttack(0, 0.6f));
                        if (attackSide == 1) StartCoroutine(ActivateAttack(0.6f, HANDKIND.left));
                        else if (attackSide == 2) StartCoroutine(ActivateAttack(0.6f, HANDKIND.right));
					}
					else
					{
						animator.SetTrigger("AttackDual" + (attackNumber + 1).ToString() + "Trigger");
						StartCoroutine(_LockMovementAndAttack(0, 0.75f));
                        StartCoroutine(ActivateAttack(0.75f, HANDKIND.two_hand));
					}
				}
			}
			else
			{
			    if(isGrounded)
			    {
				    animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
				    if(weapon == Weapon.TWOHANDSWORD)
				    {
					    StartCoroutine(_LockMovementAndAttack(0, 0.85f));
				    }
				    else if(weapon == Weapon.TWOHANDAXE)
				    {
					    StartCoroutine(_LockMovementAndAttack(0, 1.5f));
				    }
				    else
				    {
                        
					    StartCoroutine(_LockMovementAndAttack(0, 0.75f));
                        StartCoroutine(ShootCorou(0.5f));
				    }
			    }
			}
		}
	}
    public IEnumerator ShootCorou(float time)
    {
        yield return new WaitForSeconds(time / animationSpeed);
        weaponController.curentShooter.Shoot(transform.rotation, transform.position + 1.5f * transform.forward);
    }
    public IEnumerator ActivateAttack(float time, HANDKIND hand)
    {
        weaponController.activateWeaponAttack(hand, true);
        yield return new WaitForSeconds(time / animationSpeed);
        weaponController.activateWeaponAttack(hand, false);

    }
	public void AttackKick(int kickSide)
	{
		if(isGrounded)
		{
			if(kickSide == 1)
			{
				animator.SetTrigger("AttackKick1Trigger");
			}
			else
			{
				animator.SetTrigger("AttackKick2Trigger");
			}
			StartCoroutine(_LockMovementAndAttack(0, 0.8f));
		}
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	public void CastAttack(int attackSide)
	{
		if(weapon == Weapon.UNARMED || weapon == Weapon.STAFF)
		{
			int maxAttacks = 3;
			if(attackSide == 1)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, 0.8f));
				}
			}
			if(attackSide == 2)
			{
				int attackNumber = Random.Range(3, maxAttacks + 3);
				if(isGrounded)
				{
					animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, 0.8f));
				}
			}
			if(attackSide == 3)
			{
				int attackNumber = Random.Range(0, maxAttacks);
				if(isGrounded)
				{
					animator.SetTrigger("CastDualAttack" + (attackNumber + 1).ToString() + "Trigger");
					StartCoroutine(_LockMovementAndAttack(0, 1f));
				}
			}
		} 
	}

	public void Blocking()
	{
		if(Input.GetAxisRaw("TargetBlock") < -0.1f && canAction && isGrounded)
		{
			if(!isBlocking)
			{
				animator.SetTrigger("BlockTrigger");
			}
			isBlocking = true;
			canJump = false;
			animator.SetBool("Blocking", true);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			inputVec = Vector3.zero;
		}
		else
		{
			isBlocking = false;
			canJump = true;
			animator.SetBool("Blocking", false);
		}
	}

	public void GetHit()
	{
		if(weapon != Weapon.RIFLE)
		{
			int hits = 5;
			int hitNumber = Random.Range(0, hits);
			animator.SetTrigger("GetHit" + (hitNumber + 1).ToString()+ "Trigger");
			StartCoroutine(_LockMovementAndAttack(0.1f, 0.4f));
			//apply directional knockback force
			if(hitNumber <= 1)
			{
				StartCoroutine(_Knockback(-transform.forward, 8, 4));
			} 
			else if(hitNumber == 2)
			{
				StartCoroutine(_Knockback(transform.forward, 8, 4));
			}
			else if(hitNumber == 3)
			{
				StartCoroutine(_Knockback(transform.right, 8, 4));
			}
			else if(hitNumber == 4)
			{
				StartCoroutine(_Knockback(-transform.right, 8, 4));
			}
		}
		else
		{
			animator.SetTrigger("GetHit1Trigger");
		}
	}

	IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		isKnockback = true;
		StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
        carac.setTouchable(TOUCHABLESTATE.god);
		yield return new WaitForSeconds(.1f);
        carac.setTouchable(TOUCHABLESTATE.normal);
		isKnockback = false;
	}

	IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
	{
		while(isKnockback)
		{
			rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
			yield return null;
		}
	}

	public IEnumerator _Death()
	{
		animator.SetTrigger("Death1Trigger");
		StartCoroutine(_LockMovementAndAttack(0.1f, 1.5f));
		isDead = true;
		animator.SetBool("Moving", false);
		inputVec = new Vector3(0, 0, 0);
		yield return null;
	}

	public IEnumerator _Revive()
	{
		animator.SetTrigger("Revive1Trigger");
		StartCoroutine(_LockMovementAndAttack(0f, 1.45f));
		isDead = false;
		yield return null;
	}

	#endregion

	#region Rolling

	void Rolling()
	{
		if(!isRolling && isGrounded && !isAiming)
		{
            /*if(Input.GetAxis("DashVertical") > 0.5f || Input.GetAxis("DashVertical") < -0.5f || Input.GetAxis("DashHorizontal") > 0.5f || Input.GetAxis("DashHorizontal") < -0.5f)
			{
				StartCoroutine(_DirectionalRoll(Input.GetAxis("DashVertical"), Input.GetAxis("DashHorizontal")));
			}*/
            if (Input.GetButtonDown("LightHit") /*|| Input.GetButtonDown("Death")*/) 
            {
				StartCoroutine(_DirectionalRoll(0,0));
			}
        }
    }

	public IEnumerator _DirectionalRoll(float x, float v)
	{
		//check which way the dash is pressed relative to the character facing
		float angle = Vector3.Angle(targetDashDirection,-transform.forward);
		float sign = Mathf.Sign(Vector3.Dot(transform.up,Vector3.Cross(targetDashDirection,transform.forward)));
		// angle in [-179,180]
		float signed_angle = angle * sign;
		//angle in 0-360
		float angle360 = (signed_angle + 180) % 360;
		//deternime the animation to play based on the angle
		if(angle360 > 315 || angle360 < 45)
		{
			StartCoroutine(_Roll(1));
		}
		if(angle360 > 45 && angle360 < 135)
		{
			StartCoroutine(_Roll(2));
		}
		if(angle360 > 135 && angle360 < 225)
		{
			StartCoroutine(_Roll(3));
		}
		if(angle360 > 225 && angle360 < 315)
		{
			StartCoroutine(_Roll(4));
		}
		yield return null;
	}

	public IEnumerator _Roll(int rollNumber)
	{
		if(rollNumber == 1)
		{
			animator.SetTrigger("RollForwardTrigger");
		}
		if(rollNumber == 2)
		{
			animator.SetTrigger("RollRightTrigger");
		}
		if(rollNumber == 3)
		{
			animator.SetTrigger("RollBackwardTrigger");
		}
		if(rollNumber == 4)
		{
			animator.SetTrigger("RollLeftTrigger");
		}
		isRolling = true;
        carac.setTouchable(TOUCHABLESTATE.god);
		yield return new WaitForSeconds(rollduration/animationSpeed);
        carac.setTouchable(TOUCHABLESTATE.normal);
        isRolling = false;
	}

	//Placeholder functions for Animation events
	public void Hit()
	{
	}

	public void Shoot()
	{
	}

	public void FootR()
	{
	}

	public void FootL()
	{
	}

	public void Land()
	{
	}

	#endregion
	
	#region _Coroutines

	//method to keep character from moveing while attacking, etc
	public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
	{
		yield return new WaitForSeconds(delayTime);
		canAction = false;
		canMove = false;
		animator.SetBool("Moving", false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		inputVec = new Vector3(0, 0, 0);
		animator.applyRootMotion = true;
		yield return new WaitForSeconds(lockTime / animationSpeed);
		canAction = true;
		canMove = true;
		animator.applyRootMotion = false;
	}

	//for controller weapon switching
	void SwitchWeaponTwoHand(int upDown)
	{
        currentKind = KIND.angelic;
		canAction = false;
		int weaponSwitch = (int)weapon;
        animationSpeed = 10;
        StartCoroutine(_SwitchWeapon(4));
     
	}

	//for controller weapon switching
	void SwitchWeaponLeftRight(int upDown)
	{
		int weaponSwitch = 0;
		canAction = false;
		if(upDown == 0)
		{
            weaponSwitch = 8;
		}
		if(upDown == 1)
		{
            weaponSwitch = 9;
		}
		StartCoroutine(_SwitchWeapon(weaponSwitch));
	}
 
    public IEnumerator _DoubleWeapon()
    {
        currentKind = KIND.demonic;
        animationSpeed = 10;
        SwitchWeaponLeftRight(0);
        yield return new WaitForSeconds(1.05f / animationSpeed);
        animationSpeed = 10;
        SwitchWeaponLeftRight(1);
        
    }

    public IEnumerator _EnterParadise()
    {
        rpgCharacterState = RPGCharacterState.CINEMATIC;
        animator.SetBool("Moving", true);
        animator.SetFloat("Velocity Z", 0.5f);
        yield break;
    }

    //function to switch weapons
    //weaponNumber 0 = Unarmed
    //weaponNumber 1 = 2H Sword
    //weaponNumber 2 = 2H Spear
    //weaponNumber 3 = 2H Axe
    //weaponNumber 4 = 2H Bow
    //weaponNumber 5 = 2H Crowwbow
    //weaponNumber 6 = 2H Staff
    //weaponNumber 7 = Shield
    //weaponNumber 8 = L Sword
    //weaponNumber 9 = R Sword
    //weaponNumber 10 = L Mace
    //weaponNumber 11 = R Mace
    //weaponNumber 12 = L Dagger
    //weaponNumber 13 = R Dagger
    //weaponNumber 14 = L Item
    //weaponNumber 15 = R Item
    //weaponNumber 16 = L Pistol
    //weaponNumber 17 = R Pistol
    //weaponNumber 18 = Rifle
    //weaponNumber 19 == Right Spear
    //weaponNumber 20 == 2H Club
    public IEnumerator _SwitchWeapon(int weaponNumber)
	{	
		//character is unarmed
		if(weapon == Weapon.UNARMED)
		{
			StartCoroutine(_UnSheathWeapon(weaponNumber));
		}
		//character has 2 handed weapon
		else if(weapon == Weapon.STAFF || weapon == Weapon.TWOHANDAXE || weapon == Weapon.TWOHANDBOW || weapon == Weapon.TWOHANDCROSSBOW || weapon == Weapon.TWOHANDSPEAR || weapon == Weapon.TWOHANDSWORD || weapon == Weapon.RIFLE || weapon == Weapon.TWOHANDCLUB)
		{
			StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
			yield return new WaitForSeconds(1.1f / animationSpeed);
			if(weaponNumber > 0)
			{
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switch to unarmed
			else
			{
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
		}
		//character has 1 or 2 1hand weapons and/or shield
		else if(weapon == Weapon.ARMED)
		{
			//character is switching to 2 hand weapon or unarmed, put put away all weapons
			if(weaponNumber < 7 || weaponNumber > 17 || weaponNumber == 20)
			{
				//check left hand for weapon
				if(leftWeapon != 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f / animationSpeed);
					if(rightWeapon != 0)
					{
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						yield return new WaitForSeconds(1.05f /animationSpeed);
						//and right hand weapon
						if(weaponNumber != 0)
						{
							StartCoroutine(_UnSheathWeapon(weaponNumber));
						}
					}
					if(weaponNumber != 0)
					{
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
				//check right hand for weapon if no left hand weapon
				if(rightWeapon != 0)
				{
					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f / animationSpeed);
					if(weaponNumber != 0)
					{
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
			}
			//using 1 handed weapon(s)
			else if(weaponNumber == 7)
			{
				if(leftWeapon > 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f / animationSpeed);
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching left weapon, put away left weapon if equipped
			else if((weaponNumber == 8))
			{
				if(leftWeapon > 0)
				{
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f / animationSpeed);
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching right weapon, put away right weapon if equipped
			else if((weaponNumber == 9))
			{
				if(rightWeapon > 0)
				{
					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
					yield return new WaitForSeconds(1.05f / animationSpeed);
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
		}
        animationSpeed = 2;
		yield return null;
	}

	public IEnumerator _SheathWeapon(int weaponNumber, int weaponDraw)
	{
		if((weaponNumber == 8))
		{
			animator.SetInteger("LeftRight", 1);
		}
		else if((weaponNumber == 9 ))
		{
			animator.SetInteger("LeftRight", 2);
		}
		if(weaponDraw == 0)
		{
			//if switching to unarmed, don't set "Armed" until after 2nd weapon sheath
			if(leftWeapon == 0 && rightWeapon != 0)
			{
				animator.SetBool("Armed", false);
			}
			if(rightWeapon == 0 && leftWeapon != 0)
			{
				animator.SetBool("Armed", false);
			}
		}
		animator.SetTrigger("WeaponSheathTrigger");
		yield return new WaitForSeconds(0.1f);
		if(weaponNumber < 7 || weaponNumber == 18 || weaponNumber == 19 || weaponNumber == 20)
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
			animator.SetBool("Shield", false);
			animator.SetBool("Armed", false);
		}
		else if(weaponNumber == 7)
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			animator.SetBool("Shield", false);
		}
		else if((weaponNumber == 8 ))
		{
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
		}
		else if((weaponNumber == 9))
		{
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
		}
		//if switched to unarmed
		if(leftWeapon == 0 && rightWeapon == 0)
		{
			animator.SetBool("Armed", false);
		}
		if(leftWeapon == 0 && rightWeapon == 0)
		{
			animator.SetInteger("LeftRight", 0);
			animator.SetInteger("Weapon", 0);
			animator.SetBool("Armed", false);
			weapon = Weapon.UNARMED;
		}
		StartCoroutine(_WeaponVisibility(weaponNumber, 0.4f / animationSpeed, false));
		StartCoroutine(_LockMovementAndAttack(0, 1));
		yield return null;
	}
		
	public IEnumerator _UnSheathWeapon(int weaponNumber)
	{
		animator.SetInteger("Weapon", -1);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		//two handed weapons
		if(weaponNumber < 7 || weaponNumber == 18 || weaponNumber == 20)
		{
			leftWeapon = weaponNumber;
			animator.SetInteger("LeftRight", 3);
		    if(weaponNumber == 4)
			{
				weapon = Weapon.TWOHANDBOW;
				StartCoroutine(_WeaponVisibility(weaponNumber, 0.55f / animationSpeed, true));
			}
			
		}
		//one handed weapons
		else
		{
			 if(weaponNumber == 8 )
			{
				animator.SetInteger("LeftRight", 1);
				animator.SetInteger("LeftWeapon", weaponNumber);
				StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f / animationSpeed, true));
				leftWeapon = weaponNumber;
				weaponNumber = 7;
			}
			else if(weaponNumber == 9 )
			{
				animator.SetInteger("LeftRight", 2);
				animator.SetInteger("RightWeapon", weaponNumber);
				rightWeapon = weaponNumber;
				StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f / animationSpeed, true));
				weaponNumber = 7;
				//set shield to false for animator, will reset later
				if(leftWeapon == 7)
				{
					animator.SetBool("Shield", false);
				}
			}
		}
		if(weapon == Weapon.RIFLE)
		{
			animator.SetInteger("Weapon", 8);
		}
		else if(weapon == Weapon.TWOHANDCLUB)
		{
			animator.SetInteger("Weapon", 9);
		}
		else
		{
			animator.SetInteger("Weapon", weaponNumber);
		}
		animator.SetTrigger("WeaponUnsheathTrigger");
		StartCoroutine(_LockMovementAndAttack(0, 1.1f));
		yield return new WaitForSeconds(0.1f);
		if(leftWeapon == 7)
		{
			animator.SetBool("Shield", true);
			weapon = Weapon.SHIELD;
		}
		if((leftWeapon > 6 || rightWeapon > 6) && weapon != Weapon.RIFLE && weapon != Weapon.TWOHANDCLUB)
		{
			animator.SetBool("Armed", true);
			if(leftWeapon != 7)
			{
				weapon = Weapon.ARMED;
			}
		}
		//For dual blocking
		if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 17)
		{
			if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16)
			{
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16)
		{
			if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 17)
			{
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		yield return null;
	}

	public IEnumerator _WeaponVisibility(int weaponNumber, float delayTime, bool visible)
	{
		yield return new WaitForSeconds(delayTime);
	
		if(weaponNumber == 4) 
		{
			twoHandBow.SetActive(visible);
		}
		
		if(weaponNumber == 8) 
		{
			swordL.SetActive(visible);
		}
        if (weaponNumber == 9)
        {
            swordR.SetActive(visible);
        }
		
		yield return null;
	}

	public IEnumerator _BlockHitReact()
	{
		int hits = 2;
		int hitNumber = Random.Range(0, hits);
		animator.SetTrigger("BlockGetHit" + (hitNumber + 1).ToString()+ "Trigger");
		StartCoroutine(_LockMovementAndAttack(0.1f, 0.4f));
		StartCoroutine(_Knockback(-transform.forward, 3, 3));
		yield return null;
	}

	public IEnumerator _BlockBreak()
	{
		animator.applyRootMotion = true;
		animator.SetTrigger("BlockBreakTrigger");
		yield return new WaitForSeconds(1f);
		animator.applyRootMotion = false;
	}

	public void Pickup()
	{
		animator.SetTrigger("PickupTrigger");
		StartCoroutine(_LockMovementAndAttack(0, 1.4f));
	}

	public void Activate()
	{
		animator.SetTrigger("ActivateTrigger");
		StartCoroutine(_LockMovementAndAttack(0, 1.2f));
	}
	
	#endregion

    private void Death()
    {
        StartCoroutine(_Death());
    }
    private void BeHit(int damage,int maxhp)
    {
        GetHit();
    }
}