using Rpg.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Rpg
{
    public enum ALIGN { angelic, demonic , both, none };
    public enum HAND { right, left, two_hand };
    public class WeaponCreationEvent : UnityEvent<Weapon>
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class Weapon : MonoBehaviour
    {
        public int currentEnnemyAimedIndex;
        
        [SerializeField]
        protected string m_unavailableTag;

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

        protected void Start()
        {

        }
        protected void Update()
        {
        }
        
        public void ActivateAttack(bool state)
        {
            _isAttack = state;
        }
    }
}