using Rpg.GraphicElement.Weapons;
using UnityEngine;
using UnityEngine.Events;
using Rpg.Characters;
using Rpg.Manager;

namespace Rpg
{
   public class OnDamageEvent : UnityEvent<float> { }
   public class OnAttackEvent : UnityEvent<float> { }
   public enum TOUCHABLESTATE { god, normal }
   public enum STATE { NORMAL,JUMP,HIT,DEATH,DASH,CINEMATIC}
   public enum WEAPON { SWORDS,BOW}

   public class Player : MonoBehaviour
   {
       private static Player m_Instance;
       public static Player instance { get { return m_Instance; } }

       #region Variables Class
       protected WEAPON m_currentWeapon;
       protected delegate void Action();
       protected Action DoAction;
        [SerializeField]
       protected STATE m_state;
       #endregion

       #region Caracteristics
       private int _damage = 10;
       private int _health = 10;
       private int _maxHealth = 10;
       private int _armor = 2;
       private TOUCHABLESTATE _state = TOUCHABLESTATE.normal;
       #endregion

       #region Editor Links
       [SerializeField]
       private Camera _camera;
       [SerializeField]
       private GameObject _leftSword;
       [SerializeField]
       private GameObject _rightSword;
       [SerializeField]
       private GameObject _bow;
       #endregion

       #region ScriptWeapon
       private Weapon _weaponL;
       private Weapon _weaponR;
       private Weapon _weaponBow;
       #endregion

       #region Events
       public UnityEvent OnDeath;
       public OnDamageEvent OnDamaged;
       public OnAttackEvent OnAttack;
       #endregion
        
        public bool isInAir;
        public WEAPON currentWeapon
       {
           get { return m_currentWeapon; }
       }

       void Awake()
       {
           m_Instance = this;
           OnDamaged = new OnDamageEvent();
           OnAttack = new OnAttackEvent();
           OnDeath = new UnityEvent();
           DoAction = DoActionNormal;
           _weaponBow = _bow.GetComponent<BowKind>();
           _weaponR = _rightSword.GetComponent<SwordKind>();
           _weaponL = _leftSword.GetComponent<SwordKind>();
       }

       protected void Start()
       {
            _weaponBow.OnHit.AddListener(HitSomething);
            _weaponR.OnHit.AddListener(HitSomething);
            _weaponL.OnHit.AddListener(HitSomething);
            RPGCharacterController.instance.Sword();
        }

       protected void Update()
       {
           //Forward vector relative to the camera along the x-z plane   
           Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
           forward.y = 0;
           forward = forward.normalized;
           //Right vector relative to the camera always orthogonal to the forward vector
           Vector3 right = new Vector3(forward.z, 0, -forward.x);
           if(m_state!=STATE.CINEMATIC)RPGCharacterController.instance.inputVec = ControllerInput.manager.horizontal * right + ControllerInput.manager.vertical * forward;
           DoAction();
        }
       #region StateMachine
       // <summary>
       /// Set le player dans l'état Idle prêt à être utilisé pour les cinématiques    
       /// </summary>
       public void SetModeCinematic()
       {
           m_state = STATE.CINEMATIC;
           DoAction = DoActionCinematic;
       }
       private void DoActionCinematic()
       {
       }
       // <summary>
       /// Set le player dans son état par défault    
       /// </summary>
       public void SetModeNormal()
       {
            isInAir = false;
            m_state = STATE.NORMAL;
           _state = TOUCHABLESTATE.normal;
           DoAction = DoActionNormal;
       }
       private void DoActionNormal()
       {
           // INPUT JUMP
           if (ControllerInput.manager.jump && RPGCharacterController.instance.canJump) SetModeJump();
           // INPUT MELEE
           if (ControllerInput.manager.melee && RPGCharacterController.instance.canAction)
           {
               if (RPGCharacterController.instance.currentKind != ALIGN.demonic)
               {
                   RPGCharacterController.instance.Sword();
                   m_currentWeapon = WEAPON.SWORDS;
               }
               RPGCharacterController.instance.Attack(ComboManager.manager.Combo("X"));
           }
           // INPUT BOW
           if (ControllerInput.manager.bow && RPGCharacterController.instance.canAction)
           {
               if (RPGCharacterController.instance.currentKind != ALIGN.angelic)
               {
                   RPGCharacterController.instance.Bow();
                   m_currentWeapon = WEAPON.BOW;
               }
               RPGCharacterController.instance.Attack(ComboManager.manager.Combo("B"));
           }
           // INPUT DASH
           if (ControllerInput.manager.dash)
           {
               SetModeDash();
           }
       }
       // <summary>
       /// Set le player dans l'état de mort  
       /// </summary>
       public void SetModeDeath()
       {
           m_state = STATE.DEATH;
           _health = 0;
           DoAction = DoActionDeath;
           PoolingManager.manager.resetPool();
           StartCoroutine(RPGCharacterController.instance._Death());
       }
       private void DoActionDeath()
       {
       }
       // <summary>
       /// Set le player dans l'état blessé 
       /// </summary>
       private void SetModeHit(int pDamage)
       {
           m_state = STATE.HIT;
           DoAction = DoActionHit;
           _health -= pDamage;
           OnDamaged.Invoke(_health);
           RPGCharacterController.instance.GetHit();
       }
       private void DoActionHit()
       {
       }
       // <summary>
       /// Set le player dans l'état Jump    
       /// </summary>
       private void SetModeJump()
       {
           m_state = STATE.JUMP;
           float coef = Mathf.Sqrt(Mathf.Pow(ControllerInput.manager.horizontal, 2) + Mathf.Pow(ControllerInput.manager.vertical, 2));
           if (coef > 1) coef = 1;
           if (coef < 0.2f) coef = 0.2f;
           if (coef < 0.75f) coef = 0.75f;
           RPGCharacterController.instance.inAirSpeed = coef * RPGCharacterController.instance.maxInAirSpeed;
           RPGCharacterController.instance.jumpSpeed = coef * RPGCharacterController.instance.maxJumpSpeed;
           StartCoroutine(RPGCharacterController.instance._Jump());
           DoAction = DoActionJump;
       }
       private void DoActionJump()
       {
            if (isInAir)
            {
                AirControl();
                if (RPGCharacterController.instance.isGrounded) SetModeNormal();
            }
       }
       private void AirControl()
       {
           Vector3 motion = RPGCharacterController.instance.inputVec;
           motion *= (Mathf.Abs(RPGCharacterController.instance.inputVec.x) == 1 && Mathf.Abs(RPGCharacterController.instance.inputVec.z) == 1) ? 0.7f : 1;
           RPGCharacterController.instance.rb.AddForce(motion * RPGCharacterController.instance.inAirSpeed, ForceMode.Acceleration);
           //limit the amount of velocity we can achieve
           float velocityX = 0;
           float velocityZ = 0;
           if (RPGCharacterController.instance.rb.velocity.x > RPGCharacterController.instance.maxVelocity)
           {
               velocityX = GetComponent<Rigidbody>().velocity.x - RPGCharacterController.instance.maxVelocity;
               if (velocityX < 0)
               {
                   velocityX = 0;
               }
               RPGCharacterController.instance.rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
           }
           if (RPGCharacterController.instance.rb.velocity.x < RPGCharacterController.instance.minVelocity)
           {
               velocityX = RPGCharacterController.instance.rb.velocity.x - RPGCharacterController.instance.minVelocity;
               if (velocityX > 0)
               {
                   velocityX = 0;
               }
               RPGCharacterController.instance.rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
           }
           if (RPGCharacterController.instance.rb.velocity.z > RPGCharacterController.instance.maxVelocity)
           {
               velocityZ = RPGCharacterController.instance.rb.velocity.z - RPGCharacterController.instance.maxVelocity;
               if (velocityZ < 0)
               {
                   velocityZ = 0;
               }
               RPGCharacterController.instance.rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
           }
           if (RPGCharacterController.instance.rb.velocity.z < RPGCharacterController.instance.minVelocity)
           {
               velocityZ = RPGCharacterController.instance.rb.velocity.z - RPGCharacterController.instance.minVelocity;
               if (velocityZ > 0)
               {
                   velocityZ = 0;
               }
               RPGCharacterController.instance.rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
           }
           // INPUT MELEE
           if (ControllerInput.manager.melee)
           {
               if (RPGCharacterController.instance.currentKind != ALIGN.demonic)
               {
                   RPGCharacterController.instance.Sword();
                   m_currentWeapon = WEAPON.SWORDS;
               }
               RPGCharacterController.instance.JumpAttack(ComboManager.manager.Combo("X"));
           }
           // INPUT BOW
           if (ControllerInput.manager.bow)
           {
               if (RPGCharacterController.instance.currentKind != ALIGN.angelic)
               {
                   RPGCharacterController.instance.Bow();
                   m_currentWeapon = WEAPON.BOW;
               }
               RPGCharacterController.instance.JumpAttack(ComboManager.manager.Combo("B"));
           }
       }
       // <summary>
       /// Set le player dans l'état Dash    
       /// </summary>
       private void SetModeDash()
       {
           m_state = STATE.DASH;
           _state = TOUCHABLESTATE.god;
           StartCoroutine(RPGCharacterController.instance._DirectionalRoll(0, 0));
           DoAction = DoActionDash;
       }
       private void DoActionDash()
       {
           if (!RPGCharacterController.instance.isRolling) SetModeNormal();
       }
       // <summary>
       /// Set le player dans l'état Aim    
       /// </summary>
       private void SetModeAim()
       {
           DoAction = DoActionAim;
       }
       private void DoActionAim()
       {
       }
       #endregion
       public void SetMaxHealth(int lNewMaxHealth)
        {
            _maxHealth = lNewMaxHealth;
        }

        public void ChangeKarmaValue(float currentKarma)
        {
            OnAttack.Invoke(currentKarma);
        }
        

        public void TakeDamage(int pDamage)
        {
            if (_health - pDamage <= 0)
            {
                SetModeDeath();
            }
            else SetModeHit(pDamage);
        }

        private void HitSomething(GameObject target)
        {
            Debug.Log("Player Hit " + target.name);
            if (target.GetComponent<Enemy>())
            {
                target.GetComponent<Enemy>().TakeDamage(_damage);
                ComboManager.manager.IncreaseCombo();
            }
        }
    }
}

