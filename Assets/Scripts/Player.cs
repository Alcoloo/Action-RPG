using Rpg.GraphicElement.Weapons;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg
{

    public class OnDamageEvent : UnityEvent<float> { }
    public class OnAttackEvent : UnityEvent<float> { }
    public class Player : MonoBehaviour
    {
        private static Player m_Instance;
        public static Player instance { get { return m_Instance; } }

        #region Events
        public UnityEvent OnDeath;
        public OnDamageEvent OnDamaged;
        public OnAttackEvent OnAttack;
        #endregion

        public GameObject weapon;

        Weapon currentWeapon;

        void Awake()
        {
            m_Instance = this;

            OnDamaged = new OnDamageEvent();
            OnAttack = new OnAttackEvent();
            OnDeath = new UnityEvent();
        }

        protected void Start()
        {
            gameObject.AddComponent<SwordKind>();
            currentWeapon = gameObject.GetComponent<Weapon>();
        }

        protected void Update()
        {

        }

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