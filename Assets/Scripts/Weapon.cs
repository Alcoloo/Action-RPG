using Rpg.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg
{
    public enum ALIGN { angelic, demonic , both, none };
    public enum HAND { right, left, two_hand };
    public class EventCollision : UnityEvent<GameObject> { }

    /// <summary>
    ///  Classe Weapon
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        public int currentEnnemyAimedIndex;
        
        [SerializeField]
        protected string m_unavailableTag;

        public EventCollision OnHit;

        protected int m_damage = 10;
        protected bool _isAttack = false;

        [SerializeField]
        protected string m_weaponAsset;
        public string weaponAsset
        {
            get { return m_weaponAsset; }
        }

        [SerializeField]
        protected ALIGN m_align;
        public ALIGN align
        {
            get { return m_align; }
        }

        public Weapon()
        {
            OnHit = new EventCollision();
        }

        protected virtual void Awake()
        {
            
        }

        protected void Start()
        {

        }
        protected virtual void Update()
        {
        }
        
        public void ActivateAttack(bool state)
        {
            _isAttack = state;
        }

        protected virtual void OnCollisionEnter(Collision col)
        {
            if(_isAttack && (col.gameObject.GetComponent<Enemy>() || col.gameObject.GetComponent<Player>())) OnHit.Invoke(col.gameObject);
        }
    }
}