using UnityEngine;

public class Boss : Enemy {

    void Start() {
        DoAction = DoActionVoid;
        Init();
    }

    protected override void Destroy() {
        base.Destroy();
        Destroy(gameObject);
    }
}