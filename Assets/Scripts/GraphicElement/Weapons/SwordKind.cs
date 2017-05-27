using Rpg.Characters;
using Rpg.Manager;
using UnityEngine;

namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    
    public class SwordKind : Weapon
    {

        protected virtual void OnCollisionEnter(Collision col)
        {
            if (!_isAttack) return;

            else
            {
                Debug.Log("Is Attacking");
                if (col.collider.CompareTag("Ennemy"))
                {
                    Debug.Log("Target is an enemy");
                }
                else if (col.collider.CompareTag("Player"))
                {
                    Debug.Log("Target is a Player");
                }
            }
        }
    }
}
            