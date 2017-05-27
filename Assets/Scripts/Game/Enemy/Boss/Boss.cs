using Rpg;
using UnityEngine;


public class Boss : Enemy {

    override public void Start() {
        DoAction = DoActionVoid;
        Init();
    }

    protected override void Destroy() {
        base.Destroy();
        Destroy(gameObject);
    }
}