using UnityEngine;


namespace Rpg.GraphicElement.Weapons
{

    /// <summary>
    /// 
    /// </summary>
    public class GunKind : Weapon
    {

        [SerializeField]
        private GameObject bulletTemplate;
        [SerializeField]
        private Transform transformToInstantiate;

        protected override void Init()
        {
            base.Init();
            weaponController.curentShooter = this;
        }
        public override void DoAction()
        {
            base.DoAction();
        }

        public void Shoot(Quaternion rotation,Vector3 position)
        {
            GameObject go = Instantiate(bulletTemplate,transformToInstantiate);
            ThrowableKind currentShoot = go.GetComponent<ThrowableKind>();
            currentShoot.initialise(_weaponController,_unavailableTag);
            go.transform.position = new Vector3(position.x,position.y + 1.25f, position.z);
            go.transform.rotation = rotation;
        }
    }
}