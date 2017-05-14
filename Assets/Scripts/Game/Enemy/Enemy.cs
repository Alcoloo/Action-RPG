using Assets.Scripts.CharactersScripts;
using Assets.Scripts.Utils.Timer;
using Rpg.Characters;
using Rpg.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    #region variable player
    public Transform player;
    #endregion

    #region variable enemy parametrable
    public float detectionRange;
    public float attackDistance;
    public float rotationSpeed;
    public float attackMovementSpeed;
    public float speed;
    protected float hurtDuration;
    protected float attackDuration;
    #endregion

    #region variable class
    protected System.Action DoAction;
    protected Rigidbody myRigidbody;
    protected MeshRenderer myMesh;
    protected NavMeshAgent nav;
    protected float playerDistance;
    protected EnemySpe carac;
    protected float startTime;
    protected GameObject healthBar;
    protected HealthRotation healthBarScript;
    protected WeaponController weapon;
    #endregion

    protected void Awake()
    {
        //pour eviter le "find" le manager envera le gameObject Player au enemy
        player = GameObject.FindGameObjectWithTag("Player").transform;
        myRigidbody = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        myMesh = GetComponent<MeshRenderer>();
        healthBar = transform.FindChild("HealthBarBackground").gameObject;
        healthBarScript = healthBar.GetComponent<HealthRotation>();
        if(GetComponent<WeaponController>()) weapon = GetComponent<WeaponController>();
    }
    // Use this for initialization
    void Start()
    {
        DoAction = DoActionVoid;
    }

    public void init()
    {
        if(carac == null) carac = EnemyCarac.instance.getEnemy(gameObject.name);
        if (carac != null)
        {
            speed = carac.speed;
            nav.speed = speed;
            rotationSpeed = carac.rotationSpeed;
            attackMovementSpeed = carac.attackMovementSpeed;
            detectionRange = carac.detectionRange;
            attackDistance = carac.attackDistance;
            hurtDuration = carac.hurtDuration;
            attackDuration = carac.attackDuration;
            Caracteristic lCarac = getCaracComponent(transform.gameObject);
            lCarac.setCarac(carac.life, carac.life, carac.armor);
            lCarac.isHit.AddListener(setHit);
            lCarac.isDeath.AddListener(SetModeDie); 
        }
        if(healthBar != null) healthBar.SetActive(false);
    }

    protected void setHit(int pLife, int pMaxLife)
    {
        if(healthBar != null && pLife > 0)
        {
            if (!healthBar.activeInHierarchy) healthBar.SetActive(true);
            float lPercent = (float) pLife / (float) pMaxLife;
            healthBarScript.changeLife(lPercent);
        }

        SetModeHurt();
    }

    #region ajout des componentes
    protected Caracteristic getCaracComponent(GameObject pEnemy)
    {
        if (pEnemy.GetComponent<Caracteristic>() == null) pEnemy.AddComponent<Caracteristic>();
        return pEnemy.GetComponent<Caracteristic>();
    }
    #endregion 

    protected virtual void FixedUpdate()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
        //DoAction();
    }

    #region State Machine

    protected void SetModeVoid() { DoAction = DoActionVoid; }

    protected void DoActionVoid() { }

    protected void SetModeChargePlayer()
    {
        myMesh.material.color = Color.red;
        DoAction = DoActionChargePlayer;
    }

    protected void DoActionChargePlayer()
    {
        transform.LookAt(player);
        nav.destination = player.position;
        nav.speed = attackMovementSpeed;
        nav.Resume();
        if (playerDistance < attackDistance) SetModeBasicAttack();
    }

    protected void SetModeBasicAttack()
    {
        nav.Stop();
        myMesh.material.color = Color.black;
        startTime = CustomTimer.instance.elapsedTime;
        DoAction = DoActionBasicAttack;
    }

    protected void DoActionBasicAttack()
    {
        if (CustomTimer.instance.isTime(startTime, hurtDuration)) SetModeChargePlayer();
    }

    protected virtual void SetModeHurt()
    {
        startTime = CustomTimer.instance.elapsedTime;
        myMesh.material.color = Color.yellow;
        DoAction = DoActionHurt;
    }

    protected virtual void DoActionHurt()
    {
        if (CustomTimer.instance.isTime(startTime, hurtDuration)) SetModeChargePlayer();
    }

    protected void SetModeDie() { DoAction = DoActionDie; }
    
    protected void DoActionDie()
    {
        Destroy();
        //si l'animation de mort et fini destroy()
    }

    public void createShoot()
    {
        weapon.curentShooter.Shoot(transform.rotation, transform.position + transform.forward);
    }

    public void lookPlayer()
    {
        transform.LookAt(player);
    }

    protected virtual void Destroy()
    {
        SetModeVoid();
        gameObject.SetActive(false);
    }
    #endregion

    #region test
    public bool isDetected() { return (playerDistance < detectionRange); }

    public bool isOnAttackRange() { return (playerDistance < attackDistance); }
    #endregion
}
