using UnityEngine;
using System.Collections;
using Rpg;

public class ControllerInput : BaseManager<ControllerInput>
{
    private static ControllerInput m_Manager;
    
    #region Getter
    protected bool _melee;
    public bool melee
    {
        get { return _melee; }
    }

    protected bool _bow;
    public bool bow
    {
        get { return _bow; }
    }

    protected bool _jump;
    public bool jump
    {
        get { return _jump; }
    }

    protected bool _dash;
    public bool dash
    {
        get { return _dash; }
    }

    protected bool _aim;
    public bool aim
    {
        get { return _aim; }
    }

    protected bool _interact;
    public bool interact
    {
        get { return _interact; }
    }

    protected float _horizontal;
    public float horizontal
    {
        get { return _horizontal; }
    }

    protected float _vertical;
    public float vertical
    {
        get { return _vertical; }
    }

    protected float _camHorizontal;
    public float camHorizontal
    {
        get { return _camHorizontal; }
    }

    protected float _camVertical;
    public float camVertical
    {
        get { return _camVertical; }
    }
    #endregion

    // Use this for initialization

    protected override void Awake()
    {
        m_Manager = this;
        manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        _melee = Input.GetButtonDown("Melee");
        _bow = Input.GetButtonDown("Bow");
        _jump = Input.GetButtonDown("Jump");
        _dash = Input.GetButtonDown("Dash");
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        _aim = Input.GetButtonDown("Aiming");
        _interact = Input.GetButtonDown("Interact");
        if (Input.GetJoystickNames().Length > 0)
        {
            _camHorizontal = Input.GetAxis("CamHorizontal");
            _camVertical = Input.GetAxis("CamVertical");
        }
        else
        {
            _camHorizontal = Input.GetAxis("Mouse X");
            _camVertical = Input.GetAxis("Mouse Y");
        }

    }
}
