namespace Rpg
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;


    public class PlayerHealth : MonoBehaviour
    {
        private float startingHealth = 100;
        public float currentHealth;
        

        private float startingKarma = 50;
        public float currentKarma;
        
        /*temporaire*/
        public GameObject gameOver;
        private PlayerAttack weapon;

        private const int sliderFactor = 100;
        private const float speed = 0.1f;

        private bool isDead;
    
        void Awake()
        {
            
        }

        // Use this for initialization
        void Start()
        {
            currentHealth = startingHealth;
            currentKarma = startingKarma;

            weapon = GetComponent<PlayerAttack>();
        }

        // Update is called once per frame
        void Update()
        {

            if(weapon.IsAttacking())
            {
                if(weapon.WeaponEquiped() == "Demon") currentKarma -= 0.1f;
                else currentKarma += 0.1f;
                Player.instance.ChangeKarmaValue(currentKarma);
            }
        }

        void OnCollisionEnter(Collision collider)
        {
            if (collider.gameObject.CompareTag("Ennemy")) IsDamage(10);
        }

        public void IsDamage(int damageReceived)
        {
            currentHealth -= damageReceived;

            if (currentHealth == 0 && !isDead)
            {
                Destroy(gameObject);
                gameOver.SetActive(true);
                isDead = true;
            }
        }
        


    }
}
