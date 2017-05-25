using Rpg.GraphicElement.Weapons;
using UnityEngine;
using UnityEngine.Events;
using Rpg.Characters;
using Rpg.Manager;

namespace Rpg
{

    public class OnDamageEvent : UnityEvent<float> { }
    public class OnAttackEvent : UnityEvent<float> { }
    public class Player : MonoBehaviour
    {
        private static Player m_Instance;
        public static Player instance { get { return m_Instance; } }

        #region Variables Class
        protected Caracteristic m_caracteristic;
        protected SwordKind m_swords;
        protected GunKind m_bow;
        protected Weapon m_currentWeapon;
        protected delegate void Action();
        protected Action DoAction;
        #endregion
        

        #region Events
        public UnityEvent OnDeath;
        public OnDamageEvent OnDamaged;
        public OnAttackEvent OnAttack;
        #endregion

        public GameObject weapon;
        public Camera camera;

        public bool animEnded;

        public Weapon currentWeapon;

        void Awake()
        {
            m_Instance = this;

            OnDamaged = new OnDamageEvent();
            OnAttack = new OnAttackEvent();
            OnDeath = new UnityEvent();

            DoAction = DoActionNormal;
        }

        protected void Start()
        {
            m_caracteristic = GetComponent<Caracteristic>();
            m_caracteristic.isDeath.AddListener(SetModeDeath);
            m_caracteristic.isHit.AddListener(SetModeHit);
            //gameObject.AddComponent<SwordKind>();
            //currentWeapon = gameObject.GetComponent<Weapon>();
        }

        protected void Update()
        {
            //Forward vector relative to the camera along the x-z plane   
            Vector3 forward = camera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;
            forward = forward.normalized;
            //Right vector relative to the camera always orthogonal to the forward vector
            Vector3 right = new Vector3(forward.z, 0, -forward.x);
            RPGCharacterController.instance.inputVec = ControllerInput.manager.horizontal * right + ControllerInput.manager.vertical * forward;

            DoAction();
        }

        #region StateMachine
        // <summary>
        /// Set le player dans l'état Idle prêt à être utilisé pour les cinématiques    
        /// </summary>
        private void SetModeCinematic()
        {
            DoAction = DoActionCinematic;
        }
        private void DoActionCinematic()
        {

        }

        // <summary>
        /// Set le player dans son état par défault    
        /// </summary>
        private void SetModeNormal()
        {
            m_caracteristic.setTouchable(TOUCHABLESTATE.normal);
            DoAction = DoActionNormal;
        }
        private void DoActionNormal()
        {

            // INPUT JUMP
            if (ControllerInput.manager.jump && RPGCharacterController.instance.canJump) SetModeJump();

            // INPUT MELEE
            if (ControllerInput.manager.melee && RPGCharacterController.instance.canAction)
            {
                if (RPGCharacterController.instance.currentKind != KIND.demonic) RPGCharacterController.instance.Sword();
                RPGCharacterController.instance.Attack(ComboManager.manager.Combo("X"));
            }

            // INPUT BOW
            if (ControllerInput.manager.bow && RPGCharacterController.instance.canAction)
            {
                if (RPGCharacterController.instance.currentKind != KIND.angelic) RPGCharacterController.instance.Bow();
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
        private void SetModeDeath()
        {
            DoAction = DoActionDeath;
            StartCoroutine(RPGCharacterController.instance._Death());
        }
        private void DoActionDeath()
        {

        }

        // <summary>
        /// Set le player dans l'état blessé 
        /// </summary>
        private void SetModeHit(int damage, int maxhp)
        {
            DoAction = DoActionHit;
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
            AirControl();
            if (animEnded) SetModeNormal();
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
                if (RPGCharacterController.instance.currentKind != KIND.demonic) RPGCharacterController.instance.Sword();
                RPGCharacterController.instance.JumpAttack(ComboManager.manager.Combo("X"));
            }

            // INPUT BOW
            if (ControllerInput.manager.bow)
            {
                if (RPGCharacterController.instance.currentKind != KIND.angelic) RPGCharacterController.instance.Bow();
                RPGCharacterController.instance.JumpAttack(ComboManager.manager.Combo("B"));
            }

        }
        

        // <summary>
        /// Set le player dans l'état Dash    
        /// </summary>
        private void SetModeDash()
        {
            m_caracteristic.setTouchable(TOUCHABLESTATE.god);
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

        public void ChangeHealthValue(float currentHealth)
        {
            OnDamaged.Invoke(currentHealth);
        }

        public void ChangeKarmaValue(float currentKarma)
        {
            OnAttack.Invoke(currentKarma);
        }

        public void OnGameOver()
        {
            OnDeath.Invoke();
        }


    }
}