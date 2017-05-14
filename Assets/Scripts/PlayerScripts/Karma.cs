using UnityEngine;
using Rpg;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class Karma : MonoBehaviour
    {
        private float startingKarma = 50;
        public float currentKarma;

        void Awake()
        {
            
        }

        protected void Start()
        {
            currentKarma = startingKarma;
        }

        protected void Update()
        {

        }

        public float ChangeKarmaValue()
        {
            /*if (Player.instance.GetWeapon().GetAlign() == KIND.demonic) currentKarma--;
            else currentKarma++;*/
            return currentKarma;
        }
    }
}